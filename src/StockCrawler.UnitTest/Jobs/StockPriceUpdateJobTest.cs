using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz;
using StockCrawler.Dao;
using StockCrawler.Services;
using System;
using System.Linq;

namespace StockCrawler.UnitTest.Jobs
{
    /// <summary>
    ///This is a test class for StockPriceUpdateJobTest and is intended
    ///to contain all StockPriceUpdateJobTest Unit Tests
    ///</summary>
    [TestClass]
    public class StockPriceUpdateJobTest : UnitTestBase
    {
        /// <summary>
        ///A test for Execute StockPriceUpdate
        ///</summary>
        [TestMethod()]
        public void StockPriceUpdateTest()
        {
            Services.SystemTime.SetFakeTime(new DateTime(2020, 7, 31));
            StockPriceUpdateJob.Logger = new UnitTestLogger();
            StockPriceUpdateJob target = new StockPriceUpdateJob();
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
                    var data = db.GetStockHistory("2330", Services.SystemTime.Today, Services.SystemTime.Today, 100, 1, 10, ref pageCount).ToList();
                    Assert.AreEqual(1, data.Count);
                    Assert.AreEqual(1, pageCount);
                    var d1 = data.First();
                    Assert.AreEqual("2330", d1.StockNo);
                    Assert.AreEqual(425.50M, d1.ClosePrice);
                    Assert.AreEqual(426M, d1.OpenPrice);
                    Assert.AreEqual(432.00M, d1.HighPrice);
                    Assert.AreEqual(425.50M, d1.LowPrice);
                    Assert.AreEqual(50484, d1.Volume);
                    Assert.AreEqual(new DateTime(2020, 7, 31), d1.StockDT);
                }
            }
        }
    }
}
