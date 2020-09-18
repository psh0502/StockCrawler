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
        [TestInitialize]
        public override void InitBeforeTest()
        {
            base.InitBeforeTest();
            SqlTool.ExecuteSqlFile(@"..\..\..\StockCrawler.UnitTest\TestData\DailyPriceTestingData_1090326.sql");
        }
        /// <summary>
        ///A test for Execute StockPriceUpdate
        ///</summary>
        [TestMethod]
        public void StockPriceUpdateTest()
        {
            Services.SystemTime.SetFakeTime(new DateTime(2020, 3, 27));
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
                    var data = db.GetStockPriceHistoryPaging("2330", Services.SystemTime.Today, Services.SystemTime.Today, 1, 100, 1, 10, ref pageCount).ToList();
                    Assert.AreEqual(1, data.Count);
                    Assert.AreEqual(1, pageCount);
                    var d1 = data.First();
                    Assert.AreEqual("2330", d1.StockNo);
                    Assert.AreEqual(new DateTime(2020, 3, 27), d1.StockDT);
                    Assert.AreEqual(273M, d1.ClosePrice);
                    Assert.AreEqual(284M, d1.OpenPrice);
                    Assert.AreEqual(286M, d1.HighPrice);
                    Assert.AreEqual(273M, d1.LowPrice);
                    Assert.AreEqual(69320, d1.Volume);
                }
                {
                    int? pageCount = null;
                    var data = db.GetStockPriceHistoryPaging("2330", new DateTime(2020, 3, 23), new DateTime(2020, 3, 27), 5, 100, 1, 10, ref pageCount).ToList();
                    Assert.AreEqual(1, data.Count);
                    Assert.AreEqual(1, pageCount);
                    var d1 = data.First();
                    Assert.AreEqual("2330", d1.StockNo);
                    Assert.AreEqual(new DateTime(2020, 3, 23), d1.StockDT);
                    Assert.AreEqual(273M, d1.ClosePrice);
                    Assert.AreEqual(257M, d1.OpenPrice);
                    Assert.AreEqual(286M, d1.HighPrice);
                    Assert.AreEqual(252M, d1.LowPrice);
                    Assert.AreEqual(360030, d1.Volume);
                }
                Services.SystemTime.SetFakeTime(new DateTime(2020, 3, 30));
                target.Execute(context);
                Services.SystemTime.SetFakeTime(new DateTime(2020, 3, 31));
                target.Execute(context);
                {
                    int? pageCount = null;
                    var data = db.GetStockPriceHistoryPaging("2330", new DateTime(2020, 3, 1), new DateTime(2020, 3, 31), 20, 100, 1, 10, ref pageCount).ToList();
                    Assert.AreEqual(1, data.Count);
                    Assert.AreEqual(1, pageCount);
                    var d1 = data.First();
                    Assert.AreEqual("2330", d1.StockNo);
                    Assert.AreEqual(new DateTime(2020, 3, 1), d1.StockDT);
                    Assert.AreEqual(274M, d1.ClosePrice);
                    Assert.AreEqual(308M, d1.OpenPrice);
                    Assert.AreEqual(326M, d1.HighPrice);
                    Assert.AreEqual(235.50M, d1.LowPrice);
                    Assert.AreEqual(1875940, d1.Volume);
                }
            }
        }
    }
}
