using Common.Logging;
using Quartz;
using ServiceStack.Text;
using StockCrawler.Dao;
using StockCrawler.Dao.Schema;
using System;
using System.Data;
using System.IO;
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
                    InitializeHistoricData(d.StockNo, SystemTime.Today.AddYears(-2), SystemTime.Today);
                    Logger.InfoFormat("Finish the {0} stock history task.", d.StockNo);
                }
            }
        }
        #endregion

        private void DownloadTwseLatestInfo()
        {
            string url = string.Format("https://www.twse.com.tw/exchangeReport/MI_INDEX?response=csv&date={0}&type=ALLBUT0999", SystemTime.Today.ToString("yyyyMMdd"));
            Logger.DebugFormat("url=[{0}]", url);
            
            var csv_data = Tools.DownloadStringData(url, Encoding.Default, out Cookie[] _);

            // Usage of CsvReader: http://blog.darkthread.net/post-2017-05-13-servicestack-text-csvserializer.aspx
            var csv_lines = CsvReader.ParseLines(csv_data);
            var dt = new StockDataSet.StockDataTable();
            bool found_stock_list = false;
            foreach (var ln in csv_lines)
            {
                //證券代號	證券名稱	成交股數	成交筆數	成交金額	開盤價	最高價	最低價	收盤價	漲跌(+/-)	漲跌價差	最後揭示買價	最後揭示買量	最後揭示賣價	最後揭示賣量	本益比
                string[] data = CsvReader.ParseFields(ln).ToArray();
                if (found_stock_list)
                {
                    if ("備註:" == data[0].Trim())
                    {
                        found_stock_list = false;
                        break;
                    }

                    var dr = dt.NewStockRow();
                    dr.StockNo = data[0].Replace("=\"", string.Empty).Replace("\"", string.Empty);
                    dr.StockName = data[1];
                    dr.Enable = true;
                    dt.AddStockRow(dr);
                    Logger.DebugFormat("StockNo={0} - StockName={1}", dr.StockNo, dr.StockName);
                }
                else
                {
                    if ("證券代號" == data[0])
                        found_stock_list = true;
                }
            }
            Logger.DebugFormat("dt.Count={0}", dt.Count);
            if (dt.Count > 0)
                StockDataServiceProvider.GetServiceInstance().RenewStockList(dt);
        }
        private string DownloadYahooStockCSV(string stockNo, DateTime startDT, DateTime endDT)
        {
            DateTime base_date = new DateTime(1970, 1, 1);
            string url = string.Format("https://finance.yahoo.com/quote/{0}.TW/history?period1={1}&period2={2}&interval=1d&filter=history&frequency=1d",
                stockNo, (startDT - base_date).TotalSeconds, (endDT - base_date).TotalSeconds);
            Cookie c1 = null;
            var req = WebRequest.CreateHttp(url);
            req.Method = "GET";
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            string crumb = null;
            using (var res1 = req.GetResponse())
            {
                string s2 = res1.Headers["Set-Cookie"];
                c1 = new Cookie("B", s2.Split(';')[0].Substring(2)) { Domain = ".yahoo.com" };
                string key = "\"CrumbStore\":{\"crumb\":\"";
                using (var sr = new StreamReader(res1.GetResponseStream(), Encoding.UTF8))
                {
                    var data = sr.ReadToEnd();
                    int sub_beg = data.IndexOf(key) + key.Length;
                    data = data.Substring(sub_beg);
                    int sub_end = data.IndexOf("\"");
                    crumb = data.Substring(0, sub_end);
                }
            }

            req = WebRequest.CreateHttp(string.Format("https://query1.finance.yahoo.com/v7/finance/download/{0}.TW?period1={1}&period2={2}&interval=1d&events=history&crumb={3}",
                stockNo, (startDT - base_date).TotalSeconds, (endDT - base_date).TotalSeconds, crumb));
            req.Method = "GET";
            req.CookieContainer = new CookieContainer();
            req.CookieContainer.Add(c1);
            var res = req.GetResponse();
            using (var sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8))
                return sr.ReadToEnd();
        }
        private void InitializeHistoricData(string stockNo, DateTime startDT, DateTime endDT)
        {
            try
            {
                var csv_data = DownloadYahooStockCSV(stockNo, startDT, endDT);

                var csv_lines = CsvReader.ParseLines(csv_data).Skip(1);

                var dt = new StockDataSet.StockPriceHistoryDataTable();
                foreach (var ln in csv_lines)
                {
                    string[] data = CsvReader.ParseFields(ln).ToArray();
                    try
                    {
                        if (data.Length == 7)
                        {
                            var dr = dt.NewStockPriceHistoryRow();
                            dr.StockDT = DateTime.Parse(data[0]);
                            dr.OpenPrice = decimal.Parse(data[1]);
                            dr.HighPrice = decimal.Parse(data[2]);
                            dr.LowPrice = decimal.Parse(data[3]);
                            dr.ClosePrice = decimal.Parse(data[4]);
                            dr.AdjClosePrice = decimal.Parse(data[5]);
                            dr.Volume = long.Parse(data[6]) / 1000;
                            dr.StockNo = stockNo;
                            dt.AddStockPriceHistoryRow(dr);
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
                StockDataServiceProvider.GetServiceInstance().UpdateStockPriceHistoryDataTable(dt);
            }
            catch (WebException wex)
            {
                Logger.Error(string.Format("Got web error but will continue...(StockNo={0})", stockNo), wex);
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