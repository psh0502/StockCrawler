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
            var data = collector.GetStockFinanceReportCashFlow("2330", 109, 1);
            Assert.IsNotNull(data);
        }
    }
}