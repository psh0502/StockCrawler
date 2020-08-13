using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz;
using StockCrawler.Dao;
using StockCrawler.Services;
using StockCrawler.Services.StockDailyPrice;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StockCrawler.UnitTest.JobUnitTest
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
            StockPriceHistoryInitJob.Logger = new UnitTestLogger();
            StockPriceHistoryInitJob target = new StockPriceHistoryInitJob();
            IJobExecutionContext context = null;
            target.StockInfoCollector = new MockCollector();
            target.Execute(context);

            using (var db = new StockDataContext(ConnectionStringHelper.StockConnectionString))
            {
                {
                    var data = db.ExecuteQuery<GetStocksResult>("SELECT * FROM Stock(NOLOCK)").ToList();
                    Assert.AreEqual(1, data.Count);
                    Assert.AreEqual("2330", data.First().StockNo);
                }
                {
                    int? pageCount = null;
                    var data = db.GetStockHistory("2330", Services.SystemTime.Today.AddYears(-1), Services.SystemTime.Today, 100, 1, 10, ref pageCount).ToList();
                    Assert.AreEqual(10, data.Count);
                    Assert.AreEqual(10, pageCount);
                    var d1 = data.First();
                    Assert.AreEqual("2330", d1.StockNo);
                    StockPriceHistoryInitJob.Logger.InfoFormat("{0}, {1}", d1.OpenPrice, d1.ClosePrice);
                }
            }
        }

        internal class MockCollector : IStockDailyInfoCollector
        {
            public StockDailyPriceInfo GetStockDailyPriceInfo(string stockNo)
            {
                throw new NotImplementedException();
            }

            public IList<StockDailyPriceInfo> GetStockDailyPriceInfo()
            {
                return new List<StockDailyPriceInfo>() { new StockDailyPriceInfo() { StockNo = "2330", StockName = "台積電" } };
            }
        }
    }
}
