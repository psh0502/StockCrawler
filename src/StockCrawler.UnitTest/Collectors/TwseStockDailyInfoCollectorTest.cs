using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockCrawler.Services;
using StockCrawler.Services.Collectors;
using System;
using System.Linq;

namespace StockCrawler.UnitTest.Collectors
{
    [TestClass]
    public class TwseStockDailyInfoCollectorTest : UnitTestBase
    {
        [TestMethod]
        public void GetStockDailyPriceInfoTest()
        {
            var collector = new TwseStockDailyInfoCollector();
            TwseStockDailyInfoCollector._logger = new UnitTestLogger();
            SystemTime.SetFakeTime(new DateTime(2020, 8, 21));

            var r = collector.GetStockDailyPriceInfo();

            Assert.AreEqual(1118, r.Count());
            Assert.IsTrue(r.Where(d => d.StockNo == "2330").Any());
            Assert.AreEqual("台積電", r.Where(d => d.StockNo == "2330").First().StockName);
        }
    }
}
