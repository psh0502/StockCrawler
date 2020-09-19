using Common.Logging;
using ServiceStack.Text;
using StockCrawler.Dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;

namespace StockCrawler.Services.Collectors
{
    internal class YahooStockHistoryPriceCollector : IStockHistoryPriceCollector
    {
        internal static ILog _logger = LogManager.GetLogger(typeof(YahooStockHistoryPriceCollector));
        public virtual IEnumerable<GetStockPeriodPriceResult> GetStockDailyPriceInfo(string stockNo, DateTime bgnDate, DateTime endDate)
        {
            try
            {
                var csv_data = DownloadYahooStockCSV(stockNo, bgnDate, endDate);
                var csv_lines = CsvReader.ParseLines(csv_data).Skip(1);

                var list = new List<GetStockPeriodPriceResult>();
                foreach (var ln in csv_lines)
                {
                    string[] data = CsvReader.ParseFields(ln).ToArray();
                    try
                    {
                        if (data.Length == 7)
                        {
                            list.Add(new GetStockPeriodPriceResult()
                            {
                                StockDT = DateTime.Parse(data[0]),
                                Period = 1,
                                OpenPrice = decimal.Parse(data[1]),
                                HighPrice = decimal.Parse(data[2]),
                                LowPrice = decimal.Parse(data[3]),
                                ClosePrice = decimal.Parse(data[4]),
                                Volume = long.Parse(data[6]) / 1000,
                                StockNo = stockNo
                            });
                        }
                    }
                    catch (ConstraintException ex)
                    {
                        _logger.Warn(string.Format("Got duplicate data, skip it...[{0}]", stockNo), ex);
                    }
                    catch (FormatException)
                    {
                        _logger.WarnFormat("Got invalid format data...[{0}]", ln);
                    }
                }
                return list;
            }
            catch (WebException wex)
            {
                _logger.WarnFormat("Got web error[{1}] but will continue...(StockNo={0})", stockNo, wex.Status);
                throw;
            }
            catch (Exception ex)
            {
                _logger.Fatal(string.Format("Got unrecoverable error!(StockNo={0})", stockNo), ex);
                throw;
            }
        }
        protected virtual string DownloadYahooStockCSV(string stockNo, DateTime startDT, DateTime endDT)
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
    }
}
