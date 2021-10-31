using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using StockCrawler.Services.Collectors;
using System;
using System.Linq;

#if (DEBUG)
namespace StockCrawler.UnitTest.Collectors
{
    [TestClass]
    public class TwseReportCollectorTests : UnitTestBase
    {
        [TestMethod]
        public void GetStockFinancialReportTest_2330()
        {
            var collector = new TwseReportCollector
            {
                _logger = new UnitTestLogger()
            };
            var data = collector.GetStockFinancialReport(TEST_STOCKNO_台積電);
            Assert.IsTrue(data.Any(), "data count");
            var d = data[0];
            _logger.Debug(JsonConvert.SerializeObject(d));
            Assert.AreEqual(TEST_STOCKNO_台積電, d.StockNo);

            d = data[1];
            _logger.Debug(JsonConvert.SerializeObject(d));

            d = data[2];
            _logger.Debug(JsonConvert.SerializeObject(d));
        }
        [TestMethod]
        public void GetStockFinancialReportTest_9945()
        {
            var collector = new TwseReportCollector
            {
                _logger = new UnitTestLogger()
            };
            var data = collector.GetStockFinancialReport("9945");
            Assert.IsTrue(data.Any(), "data count");
            var d = data[0];
            _logger.Debug(JsonConvert.SerializeObject(d));
            Assert.AreEqual("9945", d.StockNo);

            d = data[1];
            _logger.Debug(JsonConvert.SerializeObject(d));

            d = data[2];
            _logger.Debug(JsonConvert.SerializeObject(d));
        }
        /// <summary>
        /// [美德醫療-DR]存託憑證類型股票應不在國內有財務報表
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetStockFinancialReportTest_9103()
        {
            var collector = new TwseReportCollector
            {
                _logger = new UnitTestLogger()
            };
            collector.GetStockFinancialReport("9103");
        }
    }
}
#endif