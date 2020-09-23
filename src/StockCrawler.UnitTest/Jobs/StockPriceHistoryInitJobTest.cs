using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz;
using StockCrawler.Dao;
using StockCrawler.Services;
using System;
using System.Linq;

namespace StockCrawler.UnitTest.Jobs
{
    /// <summary>
    ///This is a test class for StockPriceHistoryInitJobTest and is intended
    ///to contain all StockPriceHistoryInitJobTest Unit Tests
    ///</summary>
    [TestClass]
    public class StockPriceHistoryInitJobTest : UnitTestBase
    {
        /// <summary>
        ///A test for Execute StockPriceHistoryInit
        ///</summary>
        [TestMethod]
        public void StockPriceHistoryInitTest()
        {
            var today = new DateTime(2020, 4, 6);
            Services.SystemTime.SetFakeTime(today);
            StockPriceHistoryInitJob.Logger = new UnitTestLogger();
            var target = new StockPriceHistoryInitJob();
            IJobExecutionContext context = null;
            target.Execute(context);
            string stockNo = "2330"; // 台積電

            using (var db = new StockDataContext(ConnectionStringHelper.StockConnectionString))
            {
                {
                    var data = db.GetStocks().ToList();
                    Assert.AreEqual(1, data.Count);
                    Assert.AreEqual(stockNo, data.First().StockNo);
                }
                {
                    short period = 1;
                    int? pageCount = null;
                    var data = db.GetStockPriceHistoryPaging(stockNo, today, today, period, 100, 1, 10, ref pageCount).ToList();
                    Assert.AreEqual(1, data.Count);
                    Assert.AreEqual(1, pageCount);
                    var d1 = data.First();
                    _logger.DebugFormat("StockNo={0}\r\nStockDT={1}\r\nOpenPrice={2}\r\nHighPrice={3}\r\nLowPrice={4}\r\nClosePrice={5}\r\nVolume={6}\r\nDeltaPrice={7}\r\nDeltaPercent={8}%\r\nPE={9}",
                        d1.StockNo, d1.StockDT.ToShortDateString(), d1.OpenPrice, d1.HighPrice, d1.LowPrice, d1.ClosePrice, d1.Volume, d1.DeltaPrice, (d1.DeltaPercent * 100).ToString("#0.##"), d1.PE);
                    Assert.AreEqual(new DateTime(2020, 4, 6), d1.StockDT);
                    Assert.AreEqual(stockNo, d1.StockNo);
                    Assert.AreEqual(275.50M, d1.ClosePrice, "日收盤價");
                    Assert.AreEqual(273.00M, d1.OpenPrice, "日開盤價");
                    Assert.AreEqual(275.50M, d1.HighPrice, "日最高價");
                    Assert.AreEqual(270.00M, d1.LowPrice, "日最低價");
                    Assert.AreEqual(59712754, d1.Volume, "日成交量");
                    Assert.AreEqual(4, d1.DeltaPrice);
                    Assert.AreEqual(0.0146M, d1.DeltaPercent);
                    Assert.AreEqual(20.68M, d1.PE);
                }
                {
                    short period = 5;
                    int? pageCount = null;
                    var data = db.GetStockPriceHistoryPaging(stockNo, new DateTime(2020, 3, 1), new DateTime(2020, 3, 31), period, 100, 1, 10, ref pageCount).ToList();
                    Assert.AreEqual(5, data.Count);
                    Assert.AreEqual(1, pageCount);
                    var d1 = data.First();
                    _logger.DebugFormat("StockNo={0}\r\nStockDT={1}\r\nOpenPrice={2}\r\nHighPrice={3}\r\nLowPrice={4}\r\nClosePrice={5}\r\nVolume={6}\r\nDeltaPrice={7}\r\nDeltaPercent={8}%\r\nPE={9}",
                        d1.StockNo, d1.StockDT.ToShortDateString(), d1.OpenPrice, d1.HighPrice, d1.LowPrice, d1.ClosePrice, d1.Volume, d1.DeltaPrice, (d1.DeltaPercent * 100).ToString("#0.##"), d1.PE);
                    Assert.AreEqual(new DateTime(2020, 3, 30), d1.StockDT);
                    Assert.AreEqual(stockNo, d1.StockNo);
                    Assert.AreEqual(271.5M, d1.ClosePrice, "週收盤價");
                    Assert.AreEqual(263.5M, d1.OpenPrice, "週開盤價");
                    Assert.AreEqual(276.5M, d1.HighPrice, "週最高價");
                    Assert.AreEqual(262.5M, d1.LowPrice, "週最低價");
                    Assert.AreEqual(151819, d1.Volume, "週成交量");
                }
                {
                    short period = 20;
                    int? pageCount = null;
                    var data = db.GetStockPriceHistoryPaging("2330", new DateTime(2020, 3, 1), new DateTime(2020, 3, 31), period, 100, 1, 10, ref pageCount).ToList();
                    Assert.AreEqual(1, data.Count);
                    Assert.AreEqual(1, pageCount);
                    var d1 = data.First();
                    _logger.DebugFormat("StockNo={0}\r\nStockDT={1}\r\nOpenPrice={2}\r\nHighPrice={3}\r\nLowPrice={4}\r\nClosePrice={5}\r\nVolume={6}\r\nDeltaPrice={7}\r\nDeltaPercent={8}%\r\nPE={9}",
                        d1.StockNo, d1.StockDT.ToShortDateString(), d1.OpenPrice, d1.HighPrice, d1.LowPrice, d1.ClosePrice, d1.Volume, d1.DeltaPrice, (d1.DeltaPercent * 100).ToString("#0.##"), d1.PE);
                    Assert.AreEqual("2330", d1.StockNo);
                    Assert.AreEqual(new DateTime(2020, 3, 1), d1.StockDT);
                    Assert.AreEqual(274M, d1.ClosePrice);
                    Assert.AreEqual(308M, d1.OpenPrice);
                    Assert.AreEqual(326M, d1.HighPrice);
                    Assert.AreEqual(235.50M, d1.LowPrice);
                    Assert.AreEqual(1875940, d1.Volume);
                }
                {
                    short period = 5;
                    int? pageCount = null;
                    var data = db.GetStockPriceHistoryPaging(stockNo, new DateTime(2020, 3, 1), new DateTime(2020, 3, 28), period, 100, 1, 10, ref pageCount).ToList();
                    Assert.AreEqual(4, data.Count);
                    Assert.AreEqual(1, pageCount);
                    var d1 = data.First();
                    _logger.DebugFormat("StockNo={0}\r\nStockDT={1}\r\nOpenPrice={2}\r\nHighPrice={3}\r\nLowPrice={4}\r\nClosePrice={5}\r\nVolume={6}\r\nDeltaPrice={7}\r\nDeltaPercent={8}%\r\nPE={9}",
                        d1.StockNo, d1.StockDT.ToShortDateString(), d1.OpenPrice, d1.HighPrice, d1.LowPrice, d1.ClosePrice, d1.Volume, d1.DeltaPrice, (d1.DeltaPercent * 100).ToString("#0.##"), d1.PE);
                    Assert.AreEqual(new DateTime(2020, 3, 23), d1.StockDT);
                    Assert.AreEqual(stockNo, d1.StockNo);
                    Assert.AreEqual(273M, d1.ClosePrice, "週收盤價");
                    Assert.AreEqual(257M, d1.OpenPrice, "週開盤價");
                    Assert.AreEqual(286M, d1.HighPrice, "週最高價");
                    Assert.AreEqual(252M, d1.LowPrice, "週最低價");
                    Assert.AreEqual(360030, d1.Volume, "週成交量");
                }
            }
        }
    }
}
