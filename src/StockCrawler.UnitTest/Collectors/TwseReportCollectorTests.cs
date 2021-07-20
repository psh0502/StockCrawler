using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using StockCrawler.Services.Collectors;
using System.Linq;

#if (DEBUG)
namespace StockCrawler.UnitTest.Collectors
{
    [TestClass]
    public class TwseReportCollectorTests : UnitTestBase
    {
        /// <summary>
        /// 本測試僅能在 110 年二月底對應線上財報公布 109 全年度財報方能成功
        /// 當時的資料下載如 StockCrawler.UnitTest\TestData\TWSE\2330_109_-1_3_2020-04-06_1.html
        /// </summary>
        [TestMethod]
        public void GetStockFinancialReportTest_109Q3()
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
    }
}
#endif