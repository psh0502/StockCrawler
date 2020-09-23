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
            var collector = new TwseStockHistoryPriceCollector
            {
                _logger = new UnitTestLogger()
            };
            SystemTime.SetFakeTime(new DateTime(2020, 4, 6));

            var r = collector.GetStockHistoryPriceInfo("2330", SystemTime.Today.AddDays(-1), SystemTime.Today.AddDays(1));
            Assert.AreEqual(1, r.Count());
            Assert.IsTrue(r.Where(d => d.StockNo == "2330").Any());
            var d1 = r.Where(x => x.StockDT == new DateTime(2020, 4, 6)).First();
            _logger.DebugFormat("StockNo={0}\r\nStockDT={1}\r\nOpenPrice={2}\r\nHighPrice={3}\r\nLowPrice={4}\r\nClosePrice={5}\r\nVolume={6}\r\nDeltaPrice={7}\r\nDeltaPercent={8}%\r\nPE={9}",
                d1.StockNo, d1.StockDT.ToShortDateString(), d1.OpenPrice, d1.HighPrice, d1.LowPrice, d1.ClosePrice, d1.Volume, d1.DeltaPrice, (d1.DeltaPercent * 100).ToString("#0.##"), d1.PE);
            Assert.AreEqual("2330", d1.StockNo);
            Assert.AreEqual(new DateTime(2020, 4, 6), d1.StockDT);
            Assert.AreEqual(273M, d1.ClosePrice);
            Assert.AreEqual(284M, d1.OpenPrice);
            Assert.AreEqual(286M, d1.HighPrice);
            Assert.AreEqual(273M, d1.LowPrice);
            Assert.AreEqual(69320, d1.Volume);
            Assert.AreEqual(0, d1.DeltaPrice);
            Assert.AreEqual(0, d1.DeltaPercent);
            Assert.AreEqual(0, d1.PE);
        }
    }
}
