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
            var collector = new YahooStockHistoryPriceCollector
            {
                _logger = new UnitTestLogger()
            };
            SystemTime.SetFakeTime(new DateTime(2020, 4, 6));

            var r = collector.GetStockHistoryPriceInfo("2330", SystemTime.Today.AddYears(-1), SystemTime.Today.AddDays(1));

            Assert.AreEqual(1220, r.Count());
            var d1 = r.Where(d => d.StockNo == "2330").Last();
            _logger.DebugFormat("StockNo={0}\r\nStockDT={1}\r\nOpenPrice={2}\r\nHighPrice={3}\r\nLowPrice={4}\r\nClosePrice={5}\r\nVolume={6}\r\nDeltaPrice={7}\r\nDeltaPercent={8}%\r\nPE={9}",
                d1.StockNo, d1.StockDT.ToShortDateString(), d1.OpenPrice, d1.HighPrice, d1.LowPrice, d1.ClosePrice, d1.Volume, d1.DeltaPrice, (d1.DeltaPercent * 100).ToString("#0.##"), d1.PE);
        }
    }
}
