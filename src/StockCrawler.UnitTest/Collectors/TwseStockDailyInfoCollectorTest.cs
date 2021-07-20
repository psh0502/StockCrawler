using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockCrawler.Services;
using StockCrawler.Services.Collectors;
using System;
using System.Linq;

#if (DEBUG)
namespace StockCrawler.UnitTest.Collectors
{
    [TestClass]
    public class TwseStockDailyInfoCollectorTest : UnitTestBase
    {
        [TestInitialize]
        public override void InitBeforeTest()
        {
            base.InitBeforeTest();
            SqlTool.ExecuteSqlFile(@"..\..\..\..\database\MSSQL\20_initial_data\Stock.data.sql");
        }
        [TestMethod]
        public void GetStockDailyPriceInfoTest()
        {
            TwseCollectorBase._breakInternval = 5 * 1000;
            var collector = new TwseStockDailyInfoCollector
            {
                _logger = new UnitTestLogger()
            };

            var r = collector.GetStockDailyPriceInfo(new DateTime(2020, 3, 27));

            Assert.AreEqual(1154, r.Count());
            Assert.IsTrue(r.Where(d => d.StockNo == TEST_STOCKNO_台積電).Any());
            var d1 = r.Where(d => d.StockNo == TEST_STOCKNO_台積電).First();
            _logger.DebugFormat("StockNo={0}\r\nStockDT={1}\r\nOpenPrice={2}\r\nHighPrice={3}\r\nLowPrice={4}\r\nClosePrice={5}\r\nVolume={6}\r\nDeltaPrice={7}\r\nDeltaPercent={8}%\r\nPE={9}",
                d1.StockNo, d1.StockDT.ToShortDateString(), d1.OpenPrice, d1.HighPrice, d1.LowPrice, d1.ClosePrice, d1.Volume, d1.DeltaPrice, (d1.DeltaPercent * 100).ToString("#0.##"), d1.PE);
            Assert.AreEqual("台積電", d1.StockName);
            Assert.AreEqual(new DateTime(2020, 3, 27), d1.StockDT);
            Assert.AreEqual(284M, d1.OpenPrice);
            Assert.AreEqual(286M, d1.HighPrice);
            Assert.AreEqual(273M, d1.LowPrice);
            Assert.AreEqual(273M, d1.ClosePrice);
            Assert.AreEqual(69320306, d1.Volume);
            Assert.AreEqual(-7M, d1.DeltaPrice);
            Assert.AreEqual(-0.025M, d1.DeltaPercent);
            Assert.AreEqual(20.5M, d1.PE);

            d1 = r.Where(d => d.StockNo == "0000").First();
            _logger.DebugFormat("StockNo={0}\r\nStockDT={1}\r\nOpenPrice={2}\r\nHighPrice={3}\r\nLowPrice={4}\r\nClosePrice={5}\r\nVolume={6}\r\nDeltaPrice={7}\r\nDeltaPercent={8}%\r\nPE={9}",
                d1.StockNo, d1.StockDT.ToShortDateString(), d1.OpenPrice, d1.HighPrice, d1.LowPrice, d1.ClosePrice, d1.Volume, d1.DeltaPrice, (d1.DeltaPercent * 100).ToString("#0.##"), d1.PE);
            Assert.AreEqual("加權股價指數", d1.StockName);
            Assert.AreEqual(new DateTime(2020, 3, 27), d1.StockDT);
            Assert.AreEqual(9736.36M, d1.OpenPrice);
            Assert.AreEqual(9736.36M, d1.HighPrice);
            Assert.AreEqual(9698.92M, d1.LowPrice);
            Assert.AreEqual(9698.92M, d1.ClosePrice);
            Assert.AreEqual(4977844822, d1.Volume);
            Assert.AreEqual(-37.44M, d1.DeltaPrice);
            Assert.AreEqual(-0.0038M, d1.DeltaPercent);
        }
        [TestMethod]
        public void GetStockDailyPriceInfoTopBottomTest_4142()
        {
            TwseCollectorBase._breakInternval = 5 * 1000;
            var collector = new TwseStockDailyInfoCollector
            {
                _logger = new UnitTestLogger()
            };

            var r = collector.GetStockDailyPriceInfo(new DateTime(2021, 5, 12));

            Assert.IsTrue(r.Where(d => d.StockNo == TEST_STOCKNO_國光生).Any());
            var d1 = r.Where(d => d.StockNo == TEST_STOCKNO_國光生).First();
            _logger.DebugFormat("StockNo={0}\r\nStockDT={1}\r\nOpenPrice={2}\r\nHighPrice={3}\r\nLowPrice={4}\r\nClosePrice={5}\r\nVolume={6}\r\nDeltaPrice={7}\r\nDeltaPercent={8}%\r\nPE={9}",
                d1.StockNo, d1.StockDT.ToShortDateString(), d1.OpenPrice, d1.HighPrice, d1.LowPrice, d1.ClosePrice, d1.Volume, d1.DeltaPrice, (d1.DeltaPercent * 100).ToString("#0.##"), d1.PE);
            Assert.AreEqual("國光生", d1.StockName);
            Assert.AreEqual(new DateTime(2021, 5, 12), d1.StockDT);
            Assert.AreEqual(0.1M, d1.DeltaPercent);
        }
    }
}
#endif