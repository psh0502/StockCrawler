using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockCrawler.Dao;
using StockCrawler.Services;

namespace StockCrawler.UnitTest
{
    public class UnitTestBase
    {
        [TestInitialize]
        public void Init()
        {
            using (var db = new StockDataContext(ConnectionStringHelper.StockConnectionString))
            {
                db.ExecuteCommand("TRUNCATE TABLE StockPriceHistory");
                db.ExecuteCommand("TRUNCATE TABLE StockBasicInfo");
                db.ExecuteCommand("TRUNCATE TABLE StockReportCashFlow");
                db.ExecuteCommand("DELETE Stock");
            }
            Tools._logger = new UnitTestLogger();
        }
    }
}
