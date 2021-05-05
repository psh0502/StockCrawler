using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using StockCrawler.Services;
using StockCrawler.Services.Collectors;
using System;
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
        public void GetStockInterestIssuedInfoTest_2330_109Q4()
        {
            SystemTime.SetFakeTime(new DateTime(2021, 3, 16));
            var collector = new TwseInterestIssuedCollector
            {
                _logger = new UnitTestLogger()
            };
            var data = collector.GetStockInterestIssuedInfo(TEST_STOCKNO_台積電);
            Assert.IsTrue(data.Any());
            Assert.IsTrue(data.Count > 5, "data count");
            var d = data[0];
            _logger.Debug(JsonConvert.SerializeObject(d));
            Assert.AreEqual(TEST_STOCKNO_台積電, d.StockNo);
            Assert.AreEqual(109, d.Year, "年度錯誤");
            Assert.AreEqual(4, d.Season, "季度錯誤");
            Assert.AreEqual(new DateTime(2021, 2, 9), d.DecisionDate, "董事會決議（擬議）日期錯誤");
            Assert.AreEqual(2.5M, d.ProfitCashIssued, "盈餘分配之現金股利錯誤");
            Assert.AreEqual(0M, d.ProfitStockIssued, "盈餘分配之股票股利錯誤");
            Assert.AreEqual(0M, d.ProfitStockIssued, "法定盈餘公積發放之現金錯誤");
            Assert.AreEqual(0M, d.ProfitStockIssued, "法定盈餘公積轉增資配股(元/股)錯誤");
            Assert.AreEqual(0M, d.ProfitStockIssued, "資本公積發放之現金(元/股)錯誤");
            Assert.AreEqual(0M, d.ProfitStockIssued, "資本公積轉增資配股(元/股)錯誤");

            d = data[1];
            _logger.Debug(JsonConvert.SerializeObject(d));
            Assert.AreEqual(TEST_STOCKNO_台積電, d.StockNo);
            // TODO: validate all fields

            d = data[2];
            _logger.Debug(JsonConvert.SerializeObject(d));
            Assert.AreEqual(TEST_STOCKNO_台積電, d.StockNo);
            // TODO: validate all fields
        }
        [TestMethod]
        public void GetStockInterestIssuedInfoTest_1477_109Q4()
        {
            SystemTime.SetFakeTime(new DateTime(2021, 3, 16));
            var collector = new TwseInterestIssuedCollector
            {
                _logger = new UnitTestLogger()
            };
            var data = collector.GetStockInterestIssuedInfo(TEST_STOCKNO_聚陽);
            Assert.IsTrue(data.Any());
            Assert.IsTrue(data.Count > 5, "data count");
            var d = data[0];
            _logger.Debug(JsonConvert.SerializeObject(d));
            Assert.AreEqual(TEST_STOCKNO_聚陽, d.StockNo);
            Assert.AreEqual(109, d.Year, "年度錯誤");
            Assert.AreEqual(-1, d.Season, "季度錯誤");
            Assert.AreEqual(new DateTime(2021, 3, 22), d.DecisionDate, "董事會決議（擬議）日期錯誤");
            Assert.AreEqual(8M, d.ProfitCashIssued, "盈餘分配之現金股利錯誤");
            Assert.AreEqual(0M, d.ProfitStockIssued, "盈餘分配之股票股利錯誤");
            Assert.AreEqual(0M, d.ProfitStockIssued, "法定盈餘公積發放之現金錯誤");
            Assert.AreEqual(0M, d.ProfitStockIssued, "法定盈餘公積轉增資配股(元/股)錯誤");
            Assert.AreEqual(0M, d.ProfitStockIssued, "資本公積發放之現金(元/股)錯誤");
            Assert.AreEqual(0M, d.ProfitStockIssued, "資本公積轉增資配股(元/股)錯誤");

            d = data[1];
            _logger.Debug(JsonConvert.SerializeObject(d));
            Assert.AreEqual(TEST_STOCKNO_聚陽, d.StockNo);
            // TODO: validate all fields

            d = data[2];
            _logger.Debug(JsonConvert.SerializeObject(d));
            Assert.AreEqual(TEST_STOCKNO_聚陽, d.StockNo);
            // TODO: validate all fields
        }
    }
}
#endif