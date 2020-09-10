using Common.Logging;
using Quartz;
using ServiceStack.Text;
using StockCrawler.Dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace StockCrawler.Services
{
    public class StockPriceHistoryInitJob : JobBase, IJob
    {
        internal static ILog Logger { get; set; } = LogManager.GetLogger(typeof(StockPriceHistoryInitJob));
        public StockPriceHistoryInitJob()
            : base()
        {
            if (null == Logger)
                Logger = LogManager.GetLogger(typeof(StockPriceHistoryInitJob));
        }
        internal string ProcessingStockNo { get; set; }

        #region IJob Members
        public void Execute(IJobExecutionContext context)
        {
            Logger.InfoFormat("Invoke [{0}]...", MethodBase.GetCurrentMethod().Name);
            // init stock list
            DownloadTwseLatestInfo();

            using (var db = StockDataServiceProvider.GetServiceInstance())
            {
                foreach (var d in db.GetStocks().Where(d => string.IsNullOrEmpty(ProcessingStockNo) || d.StockNo == ProcessingStockNo))
                {
                    db.DeleteStockPriceHistoryData(d.StockNo, null);
                    InitializeHistoricData(d.StockNo, SystemTime.Today.AddYears(-2), SystemTime.Today.AddDays(1));
                    Logger.InfoFormat("Finish the {0} stock history task.", d.StockNo);
                }
            }
        }
        #endregion

        private void DownloadTwseLatestInfo()
        {
            var list = CollectorProviderService.GetStockDailyPriceCollector().GetStockDailyPriceInfo()
                .Select(d => new GetStocksResult()
                {
                    StockNo = d.StockNo,
                    StockName = d.StockName,
                    Enable = true
                }).ToList();

            if (list.Count > 0)
                StockDataServiceProvider.GetServiceInstance().RenewStockList(list);
        }
        private string DownloadYahooStockCSV(string stockNo, DateTime startDT, DateTime endDT)
        {
            DateTime base_date = new DateTime(1970, 1, 1);
            string url = string.Format("https://finance.yahoo.com/quote/{0}.TW/history?period1={1}&period2={2}&interval=1d&filter=history&frequency=1d",
                stockNo, (startDT - base_date).TotalSeconds, (endDT - base_date).TotalSeconds);
            var data = Tools.DownloadStringData(new Uri(url), Encoding.UTF8, out IList<Cookie> respCookie);
            IList<Cookie> cookies = new List<Cookie>
            {
                new Cookie() {
                    Name = "B",
                    Value = respCookie[0].Value,
                    Domain = ".yahoo.com",
                    Path = "/"
                }
            };
            string key = "\"CrumbStore\":{\"crumb\":\"";
            int sub_beg = data.IndexOf(key) + key.Length;
            data = data.Substring(sub_beg);
            int sub_end = data.IndexOf("\"");
            string crumb = data.Substring(0, sub_end);

            url = string.Format("https://query1.finance.yahoo.com/v7/finance/download/{0}.TW?period1={1}&period2={2}&interval=1d&events=history&crumb={3}",
                stockNo, (startDT - base_date).TotalSeconds, (endDT - base_date).TotalSeconds, crumb);
            return Tools.DownloadStringData(new Uri(url), Encoding.UTF8, out _, null, cookies);
        }
        private void InitializeHistoricData(string stockNo, DateTime startDT, DateTime endDT)
        {
            try
            {
                var csv_data = DownloadYahooStockCSV(stockNo, startDT, endDT);

                var csv_lines = CsvReader.ParseLines(csv_data).Skip(1);

                var list = new List<GetStockPriceHistoryResult>();
                foreach (var ln in csv_lines)
                {
                    string[] data = CsvReader.ParseFields(ln).ToArray();
                    try
                    {
                        if (data.Length == 7)
                        {
                            list.Add(new GetStockPriceHistoryResult()
                            {
                                StockDT = DateTime.Parse(data[0]),
                                Period = 1,
                                OpenPrice = decimal.Parse(data[1]),
                                HighPrice = decimal.Parse(data[2]),
                                LowPrice = decimal.Parse(data[3]),
                                ClosePrice = decimal.Parse(data[4]),
                                AdjClosePrice = decimal.Parse(data[5]),
                                Volume = long.Parse(data[6]) / 1000,
                                StockNo = stockNo
                            });
                        }
                    }
                    catch (ConstraintException ex)
                    {
                        Logger.Warn(string.Format("Got duplicate data, skip it...[{0}]", stockNo), ex);
                    }
                    catch (FormatException)
                    {
                        Logger.WarnFormat("Got invalid format data...[{0}]", ln);
                    }
                }
                if (list.Any())
                    using (var db = StockDataServiceProvider.GetServiceInstance())
                    {
                        db.InsertOrUpdateStockPriceHistory(list);
                        list.Clear();
                    }
            }
            catch (WebException wex)
            {
                Logger.WarnFormat("Got web error[{1}] but will continue...(StockNo={0})", stockNo, wex.Status);
                return;
            }
            catch (Exception ex)
            {
                Logger.Fatal(string.Format("Got unrecoverable error!(StockNo={0})", stockNo), ex);
                throw;
            }
        }
    }
}