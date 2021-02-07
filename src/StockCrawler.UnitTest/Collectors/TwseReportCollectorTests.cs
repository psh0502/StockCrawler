using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using StockCrawler.Services.Collectors;
#if (DEBUG)
namespace StockCrawler.UnitTest.Collectors
{
    [TestClass]
    public class TwseReportCollectorTests : UnitTestBase
    {
        [TestMethod]
        public void GetStockFinancialReportTest_109Q3()
        {
            var collector = new TwseReportCollector
            {
                _logger = new UnitTestLogger()
            };
            var data = collector.GetStockFinancialReport(TEST_STOCKNO_台積電, 109, 3);
            Assert.IsNotNull(data);
            Assert.AreEqual(TEST_STOCKNO_台積電, data.StockNo);
            Assert.AreEqual(108, data.Year);
            Assert.AreEqual(4, data.Season);
            Assert.AreEqual(TEST_STOCKNO_台積電, data.StockNo);
            Assert.AreEqual(109, data.Year);
            Assert.AreEqual(1, data.Season);
            Assert.AreEqual(2635572214, data.TotalAssets, "資產總計");
            Assert.AreEqual(847305839, data.TotalLiability, "負債總計");
            Assert.AreEqual(1788266375, data.NetWorth, "權益總計");
            Assert.AreEqual(68.93, data.NAV, "每股淨值");
            Assert.AreEqual(977721754, data.Revenue, "營業收入");
            Assert.AreEqual(409663524, data.BusinessInterest, "營業利益");
            Assert.AreEqual(423669819, data.NetProfitTaxFree, "稅前淨利");
            Assert.AreEqual(14.47, data.EPS, "每股盈餘");
            Assert.AreEqual(563535628, data.BusinessCashflow, "營業活動之淨現金流入");
            Assert.AreEqual(-414823101, data.InvestmentCashflow, "投資活動之淨現金流入");
            Assert.AreEqual(12588708, data.FinancingCashflow, "籌資活動之淨現金流入");
        }
    }
}
#endif