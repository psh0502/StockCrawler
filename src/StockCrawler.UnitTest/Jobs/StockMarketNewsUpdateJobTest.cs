using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz;
using StockCrawler.Dao;
using StockCrawler.Services;
using System;
using System.Linq;

#if (DEBUG)
namespace StockCrawler.UnitTest.Jobs
{
    /// <summary>
    ///This is a test class for StockPriceUpdateJobTest and is intended
    ///to contain all StockPriceUpdateJobTest Unit Tests
    ///</summary>
    [TestClass]
    public class StockMarketNewsUpdateJobTest : UnitTestBase
    {
        [TestInitialize]
        public override void InitBeforeTest()
        {
            base.InitBeforeTest();
            var collector = CollectorServiceProvider.GetMarketNewsCollector();
            foreach (var d in collector.GetLatestStockNews())
                using (var db = RepositoryProvider.GetRepositoryInstance())
                {
                    db.InsertOrUpdateStock(d.StockNo, string.Empty, string.Empty);
                    db.InsertOrUpdateStock("0000", "加權指數", "0000");
                }
            StockHelper.Reload();
        }
        /// <summary>
        ///A test for Execute StockPriceUpdate
        ///</summary>
        [TestMethod]
        public void ExecutionTest()
        {
            StockMarketNewsUpdateJob.Logger = new UnitTestLogger();
            var target = new StockMarketNewsUpdateJob();
            IJobExecutionContext context = new ArgumentJobExecutionContext(target);
            target.Execute(context);

            using (var db = RepositoryProvider.GetRepositoryInstance())
            {
                var q = db.GetStockMarketNews(10, "0000", "twse", new DateTime(2020, 10, 27), new DateTime(2020, 10, 27)).ToList();
                Assert.AreEqual(3, q.Count);
                foreach (var d in q)
                {
                    _logger.DebugFormat("[{0}][{1}][{2}]", d.NewsDate.ToShortDateString(), d.Subject, d.Url);
                    Assert.AreEqual("0000", d.StockNo);
                    Assert.AreEqual("twse", d.Source);
                }

                q = db.GetStockMarketNews(10, null, "mops", new DateTime(2020, 12, 01), new DateTime(2020, 12, 31)).ToList();
                Assert.AreEqual(10, q.Count);
                foreach (var d in q)
                {
                    _logger.DebugFormat("[{0}][{1}][{2}]", d.NewsDate.ToShortDateString(), d.Subject, d.Url);
                    Assert.AreNotEqual("0000", d.StockNo);
                    Assert.AreEqual("mops", d.Source);
                }
            }
        }
    }
}
#endif