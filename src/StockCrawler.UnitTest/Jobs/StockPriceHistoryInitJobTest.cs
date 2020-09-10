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
            Services.SystemTime.SetFakeTime(new DateTime(2020, 4, 6));
            StockPriceHistoryInitJob.Logger = new UnitTestLogger();
            StockPriceHistoryInitJob target = new StockPriceHistoryInitJob();
            IJobExecutionContext context = null;
            target.Execute(context);

            using (var db = new StockDataContext(ConnectionStringHelper.StockConnectionString))
            {
                {
                    var data = db.GetStocks().ToList();
                    Assert.AreEqual(1, data.Count);
                    Assert.AreEqual("2330", data.First().StockNo);
                }
                {
                    short period = 1;
                    int? pageCount = null;
                    var data = db.GetStockPriceHistory("2330", Services.SystemTime.Today.AddYears(-1), Services.SystemTime.Today.AddDays(1), period, 100, 1, 10, ref pageCount).ToList();
                    Assert.AreEqual(10, data.Count);
                    Assert.AreEqual(10, pageCount);
                    var d1 = data.First();
                    Assert.AreEqual(new DateTime(2020, 4, 6), d1.StockDT);
                    Assert.AreEqual("2330", d1.StockNo);
                    Assert.AreEqual(275.50M, d1.ClosePrice, "日收盤價");
                    Assert.AreEqual(273.00M, d1.OpenPrice, "日開盤價");
                    Assert.AreEqual(275.50M, d1.HighPrice, "日最高價");
                    Assert.AreEqual(270.00M, d1.LowPrice, "日最低價");
                    Assert.AreEqual(56392, d1.Volume, "日成交量");
                }
                {
                    short period = 5;
                    int? pageCount = null;
                    var data = db.GetStockPriceHistory("2330", Services.SystemTime.Today.AddYears(-1), Services.SystemTime.Today.AddDays(1), period, 100, 1, 10, ref pageCount).ToList();
                    Assert.AreEqual(10, data.Count);
                    Assert.AreEqual(10, pageCount);
                    var d1 = data.First();
                    Assert.AreEqual(new DateTime(2020, 4, 6), d1.StockDT);
                    Assert.AreEqual("2330", d1.StockNo);
                    Assert.AreEqual(279.5M, d1.ClosePrice, "週收盤價");
                    Assert.AreEqual(273.00M, d1.OpenPrice, "週開盤價");
                    Assert.AreEqual(288.00M, d1.HighPrice, "週最高價");
                    Assert.AreEqual(270.00M, d1.LowPrice, "週最低價");
                    Assert.AreEqual(197478, d1.Volume, "週成交量");
                }
                {
                    short period = 20;
                    int? pageCount = null;
                    var data = db.GetStockPriceHistory("2330", Services.SystemTime.Today.AddYears(-1), Services.SystemTime.Today.AddDays(1), period, 100, 1, 10, ref pageCount).ToList();
                    Assert.AreEqual(10, data.Count);
                    Assert.AreEqual(10, pageCount);
                    var d1 = data.First();
                    Assert.AreEqual(new DateTime(2020, 4, 6), d1.StockDT);
                    Assert.AreEqual("2330", d1.StockNo);
                    Assert.AreEqual(304.5M, d1.ClosePrice, "月收盤價");
                    Assert.AreEqual(276.50M, d1.OpenPrice, "月開盤價");
                    Assert.AreEqual(309.50M, d1.HighPrice, "月最高價");
                    Assert.AreEqual(270.00M, d1.LowPrice, "月最低價");
                    Assert.AreEqual(916713, d1.Volume, "月成交量");
                }
            }
        }
    }
}
