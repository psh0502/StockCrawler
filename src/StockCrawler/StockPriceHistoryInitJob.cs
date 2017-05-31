using Common.Logging;
using Quartz;
using ServiceStack.Text;
using StockCrawler.Dao;
using StockCrawler.Dao.Schema;
using System;
using System.Net;
using System.Text;

namespace StockCrawler.Services
{
    public class StockPriceHistoryInitJob : JobBase, IJob
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(StockPriceHistoryInitJob));
        public StockPriceHistoryInitJob() : base() { }

        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            // init stock list
            downloadTwselatestInfo();

            using (var db = StockDataService.GetServiceInstance(StockDataService.EnumDBType.MYSQL))
            {
                foreach (var d in db.GetStocks())
                {
                    downloadYahooStockCSV(d.StockNo + ".tw", DateTime.Today.AddYears(-5), DateTime.Today, d.StockID);
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
                StockDataService.GetServiceInstance(StockDataService.EnumDBType.MYSQL).RenewStockList(dt);
        }

        private void downloadYahooStockCSV(string stockNo, DateTime startDT, DateTime endDT, int stockID)
        {
            byte[] downloaded_data = null;
            try
            {
                using (var wc = new WebClient())
                    downloaded_data = wc.DownloadData(string.Format("http://ichart.finance.yahoo.com/table.csv?s={0}&d={1}&e={2}&f={3}&g=d&a={4}&b={5}&c={6}&ignore=.csv",
                        stockNo,
                        endDT.Day - 1, endDT.Month - 1, endDT.Year,
                        startDT.Day - 1, startDT.Month - 1, startDT.Year));

                string csv_data = Encoding.UTF8.GetString(downloaded_data);
                var csv_lines = CsvReader.ParseLines(csv_data);

                var dt = new StockDataSet.StockPriceHistoryDataTable();
                foreach (var ln in csv_lines)
                {
                    string[] data = CsvReader.ParseFields(ln).ToArray();
                    if (data.Length == 7)
                    {
                        var dr = dt.NewStockPriceHistoryRow();
                        dr.StockDT = DateTime.Parse(data[0]);
                        dr.OpenPrice = decimal.Parse(data[1]);
                        dr.HighPrice = decimal.Parse(data[2]);
                        dr.LowPrice = decimal.Parse(data[3]);
                        dr.ClosePrice = decimal.Parse(data[4]);
                        dr.Volumn = long.Parse(data[5]) / 1000;
                        dr.AdjClosePrice = decimal.Parse(data[6]);
                        dr.StockID = stockID;
                        dr.DateCreated = DateTime.Now;
                        dt.AddStockPriceHistoryRow(dr);
                    }
                }
                StockDataService.GetServiceInstance(StockDataService.EnumDBType.MYSQL).UpdateStockPriceHistoryDataTable(dt);
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