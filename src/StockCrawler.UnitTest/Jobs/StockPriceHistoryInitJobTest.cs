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
            StockPriceHistoryInitJob target = new StockPriceHistoryInitJob();
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
                    var data = db.GetStockPriceHistory(stockNo, today.AddYears(-1), today, period, 100, 1, 10, ref pageCount).ToList();
                    Assert.AreEqual(10, data.Count);
                    Assert.AreEqual(10, pageCount);
                    var d1 = data.First();
                    Assert.AreEqual(new DateTime(2020, 4, 6), d1.StockDT);
                    Assert.AreEqual(stockNo, d1.StockNo);
                    Assert.AreEqual(275.50M, d1.ClosePrice, "日收盤價");
                    Assert.AreEqual(273.00M, d1.OpenPrice, "日開盤價");
                    Assert.AreEqual(275.50M, d1.HighPrice, "日最高價");
                    Assert.AreEqual(270.00M, d1.LowPrice, "日最低價");
                    Assert.AreEqual(56392, d1.Volume, "日成交量");
                }
                {
                    short period = 5;
                    int? pageCount = null;
                    var data = db.GetStockPriceHistory(stockNo, new DateTime(2020, 3, 1), new DateTime(2020, 3, 31), period, 100, 1, 10, ref pageCount).ToList();
                    Assert.AreEqual(5, data.Count);
                    Assert.AreEqual(1, pageCount);
                    var d1 = data.First();
                    Assert.AreEqual(new DateTime(2020, 3, 30), d1.StockDT);
                    Assert.AreEqual(stockNo, d1.StockNo);
                    Assert.AreEqual(271.5M, d1.ClosePrice, "週收盤價");
                    Assert.AreEqual(263.5M, d1.OpenPrice, "週開盤價");
                    Assert.AreEqual(276.5M, d1.HighPrice, "週最高價");
                    Assert.AreEqual(262.5M, d1.LowPrice, "週最低價");
                    Assert.AreEqual(142511, d1.Volume, "週成交量");
                }
                {
                    short period = 20;
                    int? pageCount = null;
                    var data = db.GetStockPriceHistory(stockNo, new DateTime(2019, 12, 1), new DateTime(2019, 12, 31), period, 100, 1, 10, ref pageCount).ToList();
                    Assert.AreEqual(1, data.Count);
                    Assert.AreEqual(1, pageCount);
                    var d1 = data.First();
                    Assert.AreEqual(new DateTime(2019, 12, 1), d1.StockDT);
                    Assert.AreEqual(stockNo, d1.StockNo);
                    Assert.AreEqual(331M, d1.ClosePrice, "月收盤價");
                    Assert.AreEqual(307M, d1.OpenPrice, "月開盤價");
                    Assert.AreEqual(345M, d1.HighPrice, "月最高價");
                    Assert.AreEqual(304M, d1.LowPrice, "月最低價");
                    Assert.AreEqual(659082, d1.Volume, "月成交量");
                }
                {
                    short period = 5;
                    int? pageCount = null;
                    var data = db.GetStockPriceHistory(stockNo, new DateTime(2020, 3, 1), new DateTime(2020, 3, 28), period, 100, 1, 10, ref pageCount).ToList();
                    Assert.AreEqual(4, data.Count);
                    Assert.AreEqual(1, pageCount);
                    var d1 = data.First();
                    Assert.AreEqual(new DateTime(2020, 3, 23), d1.StockDT);
                    Assert.AreEqual(stockNo, d1.StockNo);
                    Assert.AreEqual(273M, d1.ClosePrice, "週收盤價");
                    Assert.AreEqual(257M, d1.OpenPrice, "週開盤價");
                    Assert.AreEqual(286M, d1.HighPrice, "週最高價");
                    Assert.AreEqual(252M, d1.LowPrice, "週最低價");
                    Assert.AreEqual(359462, d1.Volume, "週成交量");
                }
            }
        }
    }
}
