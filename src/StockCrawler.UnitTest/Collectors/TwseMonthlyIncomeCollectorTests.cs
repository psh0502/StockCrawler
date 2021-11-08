using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using StockCrawler.Services.Collectors;
using System.Linq;

#if (DEBUG)
namespace StockCrawler.UnitTest.Collectors
{
    [TestClass]
    public class TwseMonthlyIncomeCollectorTests : UnitTestBase
    {
        [TestMethod]
        public void GetStockMonthlyIncomeTest_2330()
        {
            var collector = new TwseMonthlyIncomeCollector
            {
                _logger = new UnitTestLogger()
            };
            var data = collector.GetStockMonthlyIncome(TEST_STOCKNO_台積電);
            Assert.IsTrue(data.Any(), "No data!");
            Assert.AreEqual(12, data.Count, "Count is not right!");
            var d = data[0];
            _logger.Debug(JsonConvert.SerializeObject(d));
            Assert.AreEqual(TEST_STOCKNO_台積電, d.StockNo);
            Assert.AreEqual(110, d.Year, "年份錯誤");
            Assert.AreEqual(9, d.Month, "月份錯誤");
            Assert.AreEqual(152685418, d.Income, "[當月營收]錯誤");
            Assert.AreEqual(127584492, d.PreIncome, "[去年當月營收]錯誤");
            Assert.AreEqual(0.1967M, d.DeltaPercent, "[去年同月增減(%)]錯誤");
            Assert.AreEqual(1149225731, d.CumMonthIncome, "[當月累計營收]錯誤");
            Assert.AreEqual(977721754, d.PreCumMonthIncome, "[去年累計營收]錯誤");
            Assert.AreEqual(0.1754M, d.DeltaCumMonthIncomePercent, "[前期比較增減(%)]錯誤");

            d = data[1];
            _logger.Debug(JsonConvert.SerializeObject(d));

            d = data[2];
            _logger.Debug(JsonConvert.SerializeObject(d));
        }
    }
}
#endif