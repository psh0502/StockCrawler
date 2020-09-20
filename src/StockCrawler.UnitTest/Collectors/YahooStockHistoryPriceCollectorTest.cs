using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockCrawler.Services;
using StockCrawler.Services.Collectors;
using System;
using System.Linq;

namespace StockCrawler.UnitTest.Collectors
{
    [TestClass]
    public class YahooStockHistoryPriceCollectorTest : UnitTestBase
    {
        [TestMethod]
        public void GetStockDailyPriceInfoTest()
        {
            var collector = new YahooStockHistoryPriceCollector();
            YahooStockHistoryPriceCollector._logger = new UnitTestLogger();
            SystemTime.SetFakeTime(new DateTime(2020, 4, 6));

            var r = collector.GetStockHistoryPriceInfo("2330", SystemTime.Today.AddYears(-5), SystemTime.Today.AddDays(1));

            Assert.AreEqual(1220, r.Count());
            Assert.IsTrue(r.Where(d => d.StockNo == "2330").Any());
        }
    }
}
