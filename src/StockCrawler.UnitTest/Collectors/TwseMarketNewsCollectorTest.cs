using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockCrawler.Services.Collectors;
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
                _logger.DebugFormat("[{0}][{1}][{2}]", d.NewsDate.ToShortDateString(), d.Subject, d.Url);
        }
    }
}
#endif