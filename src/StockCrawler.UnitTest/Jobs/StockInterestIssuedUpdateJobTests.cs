using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz;
using StockCrawler.Dao;
using StockCrawler.Services;
using System;
using System.Linq;

#if (DEBUG)
namespace StockCrawler.UnitTest.Jobs
{
    [TestClass]
    public class StockInterestIssuedUpdateJobTests : UnitTestBase
    {
        private static bool IsExecuted = false;
        [TestInitialize]
        public override void InitBeforeTest()
        {
            if (!IsExecuted)
            {
                base.InitBeforeTest();
                ExecuteTest();
            }
        }
        [TestMethod]
        public void 五年內除權息資料_2330_Test()
        {
            using (var db = new StockDataContext(ConnectionStringHelper.StockConnectionString))
            {
                var data = db.GetStockInterestIssuedInfo(100, TEST_STOCKNO_台積電, -1, -1).ToList();
                Assert.IsTrue(data.Any(), "資料筆數");
                var d = data.First();
                Assert.AreEqual(TEST_STOCKNO_台積電, d.StockNo);
                Assert.AreEqual(109, d.Year, "年度錯誤");
                Assert.AreEqual(4, d.Season, "季度錯誤");
                Assert.AreEqual(new DateTime(2021, 2, 9), d.DecisionDate, "董事會決議（擬議）日期錯誤");
                Assert.AreEqual(2.5M, d.ProfitCashIssued, "盈餘分配之現金股利錯誤");
                Assert.AreEqual(0M, d.ProfitStockIssued, "盈餘分配之股票股利錯誤");
                Assert.AreEqual(0M, d.ProfitStockIssued, "法定盈餘公積發放之現金錯誤");
                Assert.AreEqual(0M, d.ProfitStockIssued, "法定盈餘公積轉增資配股(元/股)錯誤");
                Assert.AreEqual(0M, d.ProfitStockIssued, "資本公積發放之現金(元/股)錯誤");
                Assert.AreEqual(0M, d.ProfitStockIssued, "資本公積轉增資配股(元/股)錯誤");
            }
        }
        [TestMethod]
        public void 五年內除權息資料_1477_Test()
        {
            using (var db = new StockDataContext(ConnectionStringHelper.StockConnectionString))
            {
                var data = db.GetStockInterestIssuedInfo(100, TEST_STOCKNO_聚陽, -1, -1).ToList();
                Assert.IsTrue(data.Any(), "資料筆數");
                var d = data.First();
                Assert.AreEqual(TEST_STOCKNO_聚陽, d.StockNo);
                Assert.AreEqual(109, d.Year, "年度錯誤");
                Assert.AreEqual(-1, d.Season, "季度錯誤");
                Assert.AreEqual(new DateTime(2021, 3, 22), d.DecisionDate, "董事會決議（擬議）日期錯誤");
                Assert.AreEqual(8M, d.ProfitCashIssued, "盈餘分配之現金股利錯誤");
                Assert.AreEqual(0M, d.ProfitStockIssued, "盈餘分配之股票股利錯誤");
                Assert.AreEqual(0M, d.ProfitStockIssued, "法定盈餘公積發放之現金錯誤");
                Assert.AreEqual(0M, d.ProfitStockIssued, "法定盈餘公積轉增資配股(元/股)錯誤");
                Assert.AreEqual(0M, d.ProfitStockIssued, "資本公積發放之現金(元/股)錯誤");
                Assert.AreEqual(0M, d.ProfitStockIssued, "資本公積轉增資配股(元/股)錯誤");
            }
        }
        public static void ExecuteTest()
        {
            if (!IsExecuted)
            {
                StockInterestIssuedUpdateJob.Logger = new UnitTestLogger();
                var target = new StockInterestIssuedUpdateJob();
                IJobExecutionContext context = new ArgumentJobExecutionContext(target);
                target.Execute(context);
                IsExecuted = true;
            }
        }
    }
}
#endif