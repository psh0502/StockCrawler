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
                    int? pageCount = null;
                    var data = db.GetStockPriceHistory("2330", Services.SystemTime.Today.AddYears(-1), Services.SystemTime.Today.AddDays(1), 1, 100, 1, 10, ref pageCount).ToList();
                    Assert.AreEqual(10, data.Count);
                    Assert.AreEqual(10, pageCount);
                    var d1 = data.First();
                    Assert.AreEqual(new DateTime(2020, 4, 6), d1.StockDT);
                    Assert.AreEqual("2330", d1.StockNo);
                    Assert.AreEqual(275.50M, d1.ClosePrice);
                    Assert.AreEqual(273.00M, d1.OpenPrice);
                    Assert.AreEqual(275.50M, d1.HighPrice);
                    Assert.AreEqual(270.00M, d1.LowPrice);
                    Assert.AreEqual(56392, d1.Volume);
                }
            }
        }
    }
}
