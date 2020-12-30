using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockCrawler.Services.Collectors;

#if (DEBUG)
namespace StockCrawler.UnitTest.Collectors
{
    [TestClass]
    public class LazyStockCollectorTest : UnitTestBase
    {
        [TestMethod]
        public void CollectorTestMethod_GetData_2330()
        {
            var collector = new LazyStockCollector()
            {
                _logger = new UnitTestLogger()
            };
            var r = collector.GetData("2330");
            Assert.AreEqual("成功", r.Msg);
            Assert.AreEqual("2330", r.Result.StockNum);
        }
    }
}
#endif