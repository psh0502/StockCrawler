using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockCrawler.Services;
using StockCrawler.Services.Collectors;
using System;
using System.Linq;

namespace StockCrawler.UnitTest.Collectors
{
    [TestClass]
    public class TwseStockHistoryPriceCollectorTest : UnitTestBase
    {
        [TestMethod]
        public void GetStockHistoryPriceInfoTest()
        {
            var collector = new TwseStockHistoryPriceCollector();
            TwseCollectorBase._logger = new UnitTestLogger();
            SystemTime.SetFakeTime(new DateTime(2020, 4, 6));

            var r = collector.GetStockHistoryPriceInfo("2330", SystemTime.Today.AddYears(-1), SystemTime.Today.AddDays(1));
            Assert.AreEqual(318, r.Count());
            Assert.IsTrue(r.Where(d => d.StockNo == "2330").Any());
            var d1 = r.Where(x => x.StockDT == new DateTime(2020, 3, 27)).First();
            Assert.AreEqual("2330", d1.StockNo);
            Assert.AreEqual(new DateTime(2020, 3, 27), d1.StockDT);
            Assert.AreEqual(273M, d1.ClosePrice);
            Assert.AreEqual(284M, d1.OpenPrice);
            Assert.AreEqual(286M, d1.HighPrice);
            Assert.AreEqual(273M, d1.LowPrice);
            Assert.AreEqual(69320, d1.Volume);
        }
    }
}
