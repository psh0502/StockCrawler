using Common.Logging;
using Quartz;
using ServiceStack.Text;
using StockCrawler.Dao;
using StockCrawler.Dao.Schema;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace StockCrawler.Services
{
    public class StockPriceHistoryInitJob : JobBase, IJob
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(StockPriceHistoryInitJob));
#if(DEBUG)
        private static readonly string _dbType = "MYSQL";
#else
        private static readonly string _dbType = ConfigurationManager.AppSettings["DB_TYPE"];
#endif
        public StockPriceHistoryInitJob() : base() { }

        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            // init stock list
            downloadTwselatestInfo();

            using (var db = StockDataService.GetServiceInstance(_dbType))
            {
                foreach (var d in db.GetStocks())
                {
                    initializeHistoricData(d.StockNo, DateTime.Today.AddYears(-5), DateTime.Today, d.StockID);
                    _logger.Info(string.Format("Finish the {0} stock history task.", d.StockNo));
                }
            }
        }

        #endregion

        private void downloadTwselatestInfo()
        {
            byte[] downloaded_data = null;
            using (var wc = new WebClient())
#if(DEBUG)
                downloaded_data = wc.DownloadData(string.Format("http://www.twse.com.tw/exchangeReport/MI_INDEX?response=csv&date={0}&type=ALLBUT0999", new DateTime(2017, 5, 26).ToString("yyyyMMdd")));
#else
                downloaded_data = wc.DownloadData(string.Format("http://www.twse.com.tw/exchangeReport/MI_INDEX?response=csv&date={0}&type=ALLBUT0999", DateTime.Today.ToString("yyyyMMdd")));
#endif
            if (downloaded_data.Length == 0) return; // no data means there's no closed pricing data by the date.It could be caused by national holidays.

            string csv_data = Encoding.Default.GetString(downloaded_data);
            // twse csv has some corrupt lines make parse fail.
            csv_data = csv_data.Replace("\"\"漲跌價差\"為", "\"\"\"漲跌價差\"\"為").Replace("\"無比價\"含", "\"\"\"無比價\"\"含");

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
                    dr.DateCreated = DateTime.Now;
                    dt.AddStockRow(dr);
                }
                else
                {
                    if ("證券代號" == data[0])
                        found_stock_list = true;
                }
            }
            if (dt.Count > 0)
                StockDataService.GetServiceInstance(_dbType).RenewStockList(dt);
        }

        private string downloadYahooStockCSV(string stockNo, DateTime startDT, DateTime endDT)
        {
            DateTime base_date = new DateTime(1970, 1, 1);
            HttpWebRequest req = null;
            string url = string.Format("https://finance.yahoo.com/quote/{0}.TW/history?period1={1}&period2={2}&interval=1d&filter=history&frequency=1d",
                stockNo, (startDT - base_date).TotalSeconds, (endDT - base_date).TotalSeconds);
            Cookie c1 = null;
            req = HttpWebRequest.CreateHttp(url);
            req.Method = "GET";
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            Uri target = new Uri("https://query1.finance.yahoo.com/");
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

            req = HttpWebRequest.CreateHttp(string.Format("https://query1.finance.yahoo.com/v7/finance/download/{0}.TW?period1={1}&period2={2}&interval=1d&events=history&crumb={3}",
                stockNo, (startDT - base_date).TotalSeconds, (endDT - base_date).TotalSeconds, crumb));
            req.Method = "GET";
            req.CookieContainer = new CookieContainer();
            req.CookieContainer.Add(c1);
            var res = req.GetResponse();
            using (var sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8))
                return sr.ReadToEnd();
        }

        private void initializeHistoricData(string stockNo, DateTime startDT, DateTime endDT, int stockID)
        {
            try
            {
                var csv_data = downloadYahooStockCSV(stockNo, startDT, endDT);

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
                            dr.StockID = stockID;
                            dr.DateCreated = DateTime.Now;
                            dt.AddStockPriceHistoryRow(dr);
                        }
                    }
                    catch (FormatException)
                    {
                        Debug.WriteLine(string.Format("Got invalid format data...[{0}]", ln));
                        _logger.WarnFormat("Got invalid format data...[{0}]", ln);
                    }
                }
                StockDataService.GetServiceInstance(_dbType).UpdateStockPriceHistoryDataTable(dt);
            }
            catch (WebException wex)
            {
                _logger.Error("Got web error but will continue...", wex);
                return;
            }
            catch (Exception ex)
            {
                _logger.Fatal("Got unrecoverable error!", ex);
                throw;
            }
        }
    }
}