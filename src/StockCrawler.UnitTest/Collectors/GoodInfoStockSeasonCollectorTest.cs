using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockCrawler.Services.StockSeasonReport;

namespace StockCrawler.UnitTest.Collectors
{
    [TestClass]
    public class GoodInfoStockSeasonCollectorTest : UnitTestBase
    {
        [TestMethod]
        public void CollectorTestMethod_2330_109Q1()
        {
            var collector = new GoodInfoStockSeasonCollector();
            var eps = collector.GetStockSeasonEPS("2330", 109, 1);
            var value = collector.GetStockSeasonNetValue("2330", 109, 1);

            Assert.AreEqual(4.51M, eps, "每股盈餘(EPS = 單季稅後淨利 / 已發行股數)");
            Assert.AreEqual(64.64M, value, "每股淨值(淨值(股東權益) / 在外流通股數)");
        }
    }
}
