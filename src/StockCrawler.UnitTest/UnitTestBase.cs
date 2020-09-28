using Common.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockCrawler.Dao;
using StockCrawler.Services;
using System;

namespace StockCrawler.UnitTest
{
    public class UnitTestBase
    {
        /// <summary>
        /// 測試用股票代碼 1, 台積電
        /// </summary>
        protected const string TEST_STOCK_NO_1 = "2330";
        protected static readonly ILog _logger = new UnitTestLogger();
        [ClassInitialize]
        public static void ClassInitInit(TestContext param)
        {
            TwseCollectorBase._breakInternval = 0;
            Tools._logger = new UnitTestLogger();
            SystemTime.SetFakeTime(new DateTime(2020, 4, 6));

            SqlTool.ConnectionString = ConnectionStringHelper.StockConnectionString;
            SqlTool.ExecuteSql("TRUNCATE TABLE StockAveragePrice");
            SqlTool.ExecuteSql("TRUNCATE TABLE StockPriceHistory");
            SqlTool.ExecuteSql("TRUNCATE TABLE StockBasicInfo");
            SqlTool.ExecuteSql("TRUNCATE TABLE StockReportCashFlow");
            SqlTool.ExecuteSql("TRUNCATE TABLE StockReportIncome");
            SqlTool.ExecuteSql("TRUNCATE TABLE StockReportBalance");
            SqlTool.ExecuteSql("TRUNCATE TABLE StockReportMonthlyNetProfitTaxed");
            SqlTool.ExecuteSql("DELETE Stock");
            SqlTool.ExecuteSqlFile(@"..\..\..\..\database\MSSQL\20_initial_data\Stock.data.sql");

            using (var db = new StockDataContext(ConnectionStringHelper.StockConnectionString))
                db.InsertOrUpdateStock("2330", "台積電");

            SqlTool.ExecuteSql(@"INSERT [dbo].[StockBasicInfo]
                    ([StockNo],[Category],[CompanyName],[CompanyID]
                    ,[BuildDate],[PublishDate],[Capital]
                    ,[ReleaseStockCount],[Chairman],[CEO], [Url]
                    ,[Business])
                VALUES
                    ('2330', N'半導體業', N'台灣積體電路製造股份有限公司', '22099131'
                    , '1987-02-21', '1994-09-05', 259303804580.00
                    , 25930380458, N'劉德音', N'總裁: 魏哲家', 'http://www.tsmc.com'
                    , N'依客戶之訂單與其提供之產品設計說明，以從事製造與銷售積體電路以及其他晶圓半導體裝置。提供前述產品之封裝與測試服務、積體電路之電腦輔助設計技術服務。提供製造光罩及其設計服務。')
                ");
        }
        [TestInitialize]
        public virtual void InitBeforeTest()
        {
            ClassInitInit(null);
        }
    }
}
