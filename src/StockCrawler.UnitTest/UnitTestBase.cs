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
        public virtual void InitBeforeTest()
        {
            SqlTool.ConnectionString = ConnectionStringHelper.StockConnectionString;
            SqlTool.ExecuteSql("TRUNCATE TABLE StockAveragePrice");
            SqlTool.ExecuteSql("TRUNCATE TABLE StockPriceHistory");
            SqlTool.ExecuteSql("TRUNCATE TABLE StockBasicInfo");
            SqlTool.ExecuteSql("TRUNCATE TABLE StockReportCashFlow");
            SqlTool.ExecuteSql("TRUNCATE TABLE StockReportIncome");
            SqlTool.ExecuteSql("TRUNCATE TABLE StockReportBalance");
            SqlTool.ExecuteSql("TRUNCATE TABLE StockReportMonthlyNetProfitTaxed");
            SqlTool.ExecuteSql("DELETE Stock");

            using (var db = new StockDataContext(ConnectionStringHelper.StockConnectionString))
                db.InsertOrUpdateStock("2330", "台積電");

            Tools._logger = new UnitTestLogger();
        }
    }
}
