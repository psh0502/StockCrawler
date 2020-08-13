using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockCrawler.Services.StockFinanceReport;

namespace StockCrawler.UnitTest.Collectors
{
    [TestClass()]
    public class TwseReportCollectorTests : UnitTestBase
    {
        [TestMethod()]
        public void GetStockFinanceReportCashFlowTest_109Q1()
        {
            var collector = new TwseReportCollector();
            TwseReportCollector._logger = new UnitTestLogger();
            var data = collector.GetStockFinanceReportCashFlow("2330", 109, 1);
            Assert.IsNotNull(data);
            Assert.AreEqual("2330", data.StockNo);
            Assert.AreEqual(109, data.Year);
            Assert.AreEqual(1, data.Season);
            Assert.AreEqual(67083741, data.Depreciation);
            Assert.AreEqual(1470736, data.AmortizationFee);
            Assert.AreEqual(203029442, data.BusinessCashflow);
            Assert.AreEqual(-188993268, data.InvestmentCashflow);
            Assert.AreEqual(-40757411, data.FinancingCashflow);
            //Assert.AreEqual(-192063846, data.CapitalExpenditures);
            Assert.AreEqual(14036174, data.FreeCashflow);
            Assert.AreEqual(-26721237, data.NetCashflow);
        }
        [TestMethod()]
        public void GetStockFinanceReportCashFlowTest_108Q4()
        {
            var collector = new TwseReportCollector();
            TwseReportCollector._logger = new UnitTestLogger();
            var data = collector.GetStockFinanceReportCashFlow("2330", 108, 4);
            Assert.IsNotNull(data);
            Assert.AreEqual("2330", data.StockNo);
            Assert.AreEqual(108, data.Year);
            Assert.AreEqual(4, data.Season);
            Assert.AreEqual(66137308, data.Depreciation);
            Assert.AreEqual(1394477, data.AmortizationFee);
            Assert.AreEqual(202954417, data.BusinessCashflow);
            Assert.AreEqual(-171605106, data.InvestmentCashflow);
            Assert.AreEqual(-17182776, data.FinancingCashflow);
            //Assert.AreEqual(-170009539, data.CapitalExpenditures);
            Assert.AreEqual(31349311, data.FreeCashflow);
            Assert.AreEqual(14166535, data.NetCashflow);
        }
    }
}