using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using StockCrawler.Services.Collectors;
using System.Linq;
#if (DEBUG)
namespace StockCrawler.UnitTest.Collectors
{
    [TestClass]
    public class TwseInterestIssuedCollectorTests : UnitTestBase
    {
        /// <summary>
        /// 本測試僅能在 110 年二月底對應股東會公布方能成功
        /// 當時的資料下載如 StockCrawler.UnitTest\TestData\TWSE\Issued\2330_109_-1_3_2020-04-06_1.html
        /// </summary>
        [TestMethod]
        public void GetStockInterestIssuedInfoTest_109Q4()
        {
            var collector = new TwseInterestIssuedCollector
            {
                _logger = new UnitTestLogger()
            };
            var data = collector.GetStockInterestIssuedInfo(TEST_STOCKNO_台積電);
            Assert.IsTrue(data.Any());
            Assert.AreEqual(5, data.Count, "data count");
            var d = data[0];
            _logger.Debug(JsonConvert.SerializeObject(d));
            Assert.AreEqual(TEST_STOCKNO_台積電, d.StockNo);
            Assert.AreEqual(109, d.Year);
            Assert.AreEqual(4, d.Season);
            // TODO: validate all fields

            d = data[1];
            _logger.Debug(JsonConvert.SerializeObject(d));
            Assert.AreEqual(TEST_STOCKNO_台積電, d.StockNo);
            // TODO: validate all fields

            d = data[2];
            _logger.Debug(JsonConvert.SerializeObject(d));
            Assert.AreEqual(TEST_STOCKNO_台積電, d.StockNo);
            // TODO: validate all fields
        }
    }
}
#endif