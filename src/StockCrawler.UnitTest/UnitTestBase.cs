﻿using Common.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockCrawler.Dao;
using StockCrawler.Services;
using System;

#if (DEBUG)
namespace StockCrawler.UnitTest
{
    public class UnitTestBase
    {
        /// <summary>
        /// 測試用股票代碼 1, 台積電
        /// </summary>
        internal const string TEST_STOCKNO_台積電 = "2330";
        internal const string TEST_STOCKNO_彰銀 = "2801";
        internal const string TEST_STOCKNO_聚陽 = "1477";
        internal const string TEST_STOCKNO_國光生 = "4142";
        protected static readonly ILog _logger = new UnitTestLogger();
        [ClassInitialize]
        public static void ClassInitInit(TestContext param)
        {
            TwseCollectorBase._breakInternval = 0;
            Tools._logger = new UnitTestLogger();
            SystemTime.SetFakeTime(new DateTime(2020, 4, 6));

            SqlTool.ConnectionString = ConnectionStringHelper.StockConnectionString;
            SqlTool.ExecuteSql("TRUNCATE TABLE StockForumRelations");
            SqlTool.ExecuteSql("TRUNCATE TABLE StockAveragePrice");
            SqlTool.ExecuteSql("TRUNCATE TABLE StockPriceHistory");
            SqlTool.ExecuteSql("TRUNCATE TABLE StockBasicInfo");
            SqlTool.ExecuteSql("TRUNCATE TABLE StockFinancialReport");
            SqlTool.ExecuteSql("TRUNCATE TABLE StockMarketNews");
            SqlTool.ExecuteSql("TRUNCATE TABLE StockInterestIssuedInfo");
            SqlTool.ExecuteSql("TRUNCATE TABLE StockAnalysisData");
            SqlTool.ExecuteSql("DELETE Stock");
            SqlTool.ExecuteSql("DELETE StockForums");


            using (var db = RepositoryProvider.GetRepositoryInstance())
            {
                db.InsertOrUpdateStock(TEST_STOCKNO_台積電, "台積電", "0029");
                db.InsertOrUpdateStock("9945", "潤泰新", "0043");
                db.InsertOrUpdateStock("2888", "新光金", "0040");
                db.InsertOrUpdateStock(TEST_STOCKNO_聚陽, "聚陽", "0019");
                db.InsertOrUpdateStock(TEST_STOCKNO_國光生, "國光生", "0024");
            }

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
#endif