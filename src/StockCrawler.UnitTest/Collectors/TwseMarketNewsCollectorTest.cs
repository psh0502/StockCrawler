using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockCrawler.Services;
using StockCrawler.Services.Collectors;
using System;
using System.Linq;

#if (DEBUG)
namespace StockCrawler.UnitTest.Collectors
{
    [TestClass]
    public class TwseMarketNewsCollectorTest : UnitTestBase
    {
        [TestMethod]
        public void CollectorTestMethod_GetLatestNews()
        {
            var collector = new TwseMarketNewsCollector()
            {
                _logger = new UnitTestLogger()
            };
            var r = collector.GetLatestNews();
            Assert.IsTrue(r.Any(), "查無有效資料筆數");
            Assert.IsTrue(r.Count() > 10, "資料比數不足十筆");
            foreach (var d in r)
            {
                _logger.DebugFormat("[{0}][{1}][{2}]", d.NewsDate.ToShortDateString(), d.Subject, d.Url);
                Assert.AreEqual("0000", d.StockNo);
                Assert.AreEqual("twse", d.Source);
            }
        }
        [TestMethod]
        public void CollectorTestMethod_GetLatestStockNews()
        {
            SystemTime.Reset();

            var collector = new TwseMarketNewsCollector()
            {
                _logger = new UnitTestLogger()
            };
            var r = collector.GetLatestStockNews();
            Assert.IsTrue(r.Any(), "查無有效資料筆數");
            foreach (var d in r)
            {
                _logger.DebugFormat("[{0}][{1}][{2}][{3}][{4}]", d.StockNo, d.Source, d.NewsDate, d.Subject, d.Url);
                Assert.AreNotEqual("0000", d.StockNo);
                Assert.AreEqual("mops", d.Source);
            }
        }
    }
}
#endif