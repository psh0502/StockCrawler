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
            var collector = new TwseStockDailyInfoCollector
            {
                _logger = new UnitTestLogger()
            };
            SystemTime.SetFakeTime(new DateTime(2020, 3, 27));

            var r = collector.GetStockDailyPriceInfo();

            Assert.AreEqual(1115, r.Count());
            Assert.IsTrue(r.Where(d => d.StockNo == "2330").Any());
            var d1 = r.Where(d => d.StockNo == "2330").First();
            _logger.DebugFormat("StockNo={0}\r\nStockDT={1}\r\nOpenPrice={2}\r\nHighPrice={3}\r\nLowPrice={4}\r\nClosePrice={5}\r\nVolume={6}\r\nDeltaPrice={7}\r\nDeltaPercent={8}%\r\nPE={9}",
                d1.StockNo, d1.StockDT.ToShortDateString(), d1.OpenPrice, d1.HighPrice, d1.LowPrice, d1.ClosePrice, d1.Volume, d1.DeltaPrice, (d1.DeltaPercent * 100).ToString("#0.##"), d1.PE);
            Assert.AreEqual("台積電", d1.StockName);
            Assert.AreEqual(new DateTime(2020, 3, 27), d1.StockDT);
            Assert.AreEqual(273M, d1.ClosePrice);
            Assert.AreEqual(284M, d1.OpenPrice);
            Assert.AreEqual(286M, d1.HighPrice);
            Assert.AreEqual(273M, d1.LowPrice);
            Assert.AreEqual(69320306, d1.Volume);
            Assert.AreEqual(0, d1.DeltaPrice);
            Assert.AreEqual(0, d1.DeltaPercent);
            Assert.AreEqual(0, d1.PE);
        }
    }
}
