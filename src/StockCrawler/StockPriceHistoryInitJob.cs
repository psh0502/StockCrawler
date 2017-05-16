using Quartz;
using StockCrawler.Dao;
using StockCrawler.Dao.Schema;
using System;
using System.IO;
using System.Net;

namespace StockCrawler.Services
{
    public class StockPriceHistoryInitJob : JobBase, IJob, IDisposable
    {
        private readonly WebClient _wc = new WebClient();
        public StockPriceHistoryInitJob() : base() { }
        ~StockPriceHistoryInitJob() { Dispose(); }

        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            // init stock list
            downloadTwselatestInfo();

            using (var db = StockDataService.GetServiceInstance())
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
            using (WebClient wc = new WebClient())
            {
                var bin = wc.DownloadData(string.Format("http://www.twse.com.tw/ch/trading/exchange/MI_INDEX/MI_INDEX.php?download=csv&qdate={0}/{1}&selectType=ALLBUT0999", DateTime.Today.Year - 1911, DateTime.Today.ToString("MM/dd")));
                using (StreamReader sr = new StreamReader(new MemoryStream(bin, false)))
                {
                    var dt = new StockDataSet.StockDataTable();
                    sr.ReadLine();
                    while (true)
                    {
                        string s = sr.ReadLine();
                        if (string.IsNullOrEmpty(s)) break;
                        string[] datas = s.Split(',');
                        if (datas.Length == 7)
                        {
                            var dr = dt.NewStockRow();
                            dr.StockDT = DateTime.Parse(datas[0]);
                            dr.OpenPrice = decimal.Parse(datas[1]);
                            dr.HighPrice = decimal.Parse(datas[2]);
                            dr.LowPrice = decimal.Parse(datas[3]);
                            dr.ClosePrice = decimal.Parse(datas[4]);
                            dr.Volumn = long.Parse(datas[5]) / 1000;
                            dr.AdjClosePrice = decimal.Parse(datas[6]);
                            dr.StockID = stockID;
                            dr.DateCreated = DateTime.Now;
                            dt.AddStockRow(dr);
                        }
                    }
                    StockDataService.GetServiceInstance().RenewStockList(dt);
                }
            }
        }

        private void downloadYahooStockCSV(string stockNo, DateTime startDT, DateTime endDT, int stockID)
        {
            byte[] csvBin = null;
            try
            {
                csvBin = _wc.DownloadData(string.Format("http://ichart.finance.yahoo.com/table.csv?s={0}&d={1}&e={2}&f={3}&g=d&a={4}&b={5}&c={6}&ignore=.csv",
                    stockNo,
                    endDT.Day - 1, endDT.Month - 1, endDT.Year,
                    startDT.Day - 1, startDT.Month - 1, startDT.Year));

                using (StreamReader sr = new StreamReader(new MemoryStream(csvBin, false)))
                {
                    var dt = new StockDataSet.StockPriceHistoryDataTable();
                    sr.ReadLine();
                    while (true)
                    {
                        string s = sr.ReadLine();
                        if (string.IsNullOrEmpty(s)) break;
                        string[] datas = s.Split(',');
                        if (datas.Length == 7)
                        {
                            var dr = dt.NewStockPriceHistoryRow();
                            dr.StockDT = DateTime.Parse(datas[0]);
                            dr.OpenPrice = decimal.Parse(datas[1]);
                            dr.HighPrice = decimal.Parse(datas[2]);
                            dr.LowPrice = decimal.Parse(datas[3]);
                            dr.ClosePrice = decimal.Parse(datas[4]);
                            dr.Volumn = long.Parse(datas[5]) / 1000;
                            dr.AdjClosePrice = decimal.Parse(datas[6]);
                            dr.StockID = stockID;
                            dr.DateCreated = DateTime.Now;
                            dt.AddStockPriceHistoryRow(dr);
                        }
                    }
                    StockDataService.GetServiceInstance().UpdateStockPriceHistoryDataTable(dt);
                }
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

        #region IDisposable Members

        public void Dispose()
        {
            _wc.Dispose();
        }

        #endregion
    }
}