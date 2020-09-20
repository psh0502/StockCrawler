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
            SystemTime.SetFakeTime(new DateTime(2020, 3, 27));

            var r = collector.GetStockDailyPriceInfo();

            Assert.AreEqual(1115, r.Count());
            Assert.IsTrue(r.Where(d => d.StockNo == "2330").Any());
            var d1 = r.Where(d => d.StockNo == "2330").First();
            Assert.AreEqual("台積電", d1.StockName);
            Assert.AreEqual(new DateTime(2020, 3, 27), d1.StockDT);
            Assert.AreEqual(273M, d1.ClosePrice);
            Assert.AreEqual(284M, d1.OpenPrice);
            Assert.AreEqual(286M, d1.HighPrice);
            Assert.AreEqual(273M, d1.LowPrice);
            Assert.AreEqual(69320, d1.Volume);
        }
    }
}
