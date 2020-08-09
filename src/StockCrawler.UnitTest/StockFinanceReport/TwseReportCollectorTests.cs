using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StockCrawler.Services.StockFinanceReport.Tests
{
    [TestClass()]
    public class TwseReportCollectorTests
    {
        [TestMethod()]
        public void GetStockFinanceReportCashFlowTest()
        {
            var collector = new TwseReportCollector();
            var data = collector.GetStockFinanceReportCashFlow("2330", 2020, 1);
            Assert.Equals(1, data.Count);
        }
    }
}