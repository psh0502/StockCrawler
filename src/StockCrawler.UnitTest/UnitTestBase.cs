using Common.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockCrawler.Dao;
using StockCrawler.Services;

namespace StockCrawler.UnitTest
{
    public class UnitTestBase
    {
        protected static readonly ILog _logger = new UnitTestLogger();
        [TestInitialize]
        public virtual void Init()
        {
            using (var db = new StockDataContext(ConnectionStringHelper.StockConnectionString))
            {
                db.ExecuteCommand("TRUNCATE TABLE StockPriceHistory");
                db.ExecuteCommand("TRUNCATE TABLE StockBasicInfo");
                db.ExecuteCommand("TRUNCATE TABLE StockReportCashFlow");
                db.ExecuteCommand("TRUNCATE TABLE StockReportIncome");
                db.ExecuteCommand("TRUNCATE TABLE StockReportBalance");
                db.ExecuteCommand("TRUNCATE TABLE StockReportMonthlyNetProfitTaxed");
                db.ExecuteCommand("DELETE Stock");
                db.InsertOrUpdateStock("2330", "台積電");
            }
            Tools._logger = new UnitTestLogger();
            TwseCollectorBase._logger = new UnitTestLogger();
        }
    }
}
