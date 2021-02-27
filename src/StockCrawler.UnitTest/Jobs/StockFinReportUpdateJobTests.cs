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
    public class StockFinReportUpdateJobTests : UnitTestBase
    {
        private static bool IsExecuted = false;
        [TestInitialize]
        public override void InitBeforeTest()
        {
            if (!IsExecuted)
                ExecuteTest();
        }
        [TestMethod]
        public void 簡易財務報表_Test()
        {
            using (var db = new StockDataContext(ConnectionStringHelper.StockConnectionString))
            {
                var data = db.GetStockFinancialReport(100, TEST_STOCKNO_台積電, -1, -1).ToList();
                Assert.AreEqual(3, data.Count, "資料筆數");
                var d1 = data.First();
                Assert.AreEqual(TEST_STOCKNO_台積電, d1.StockNo);
                Assert.AreEqual(109, d1.Year);
                Assert.AreEqual(4, d1.Season);
                Assert.AreEqual(2760711405m, d1.TotalAssets, "資產總計");
                Assert.AreEqual(910089406m, d1.TotalLiability, "負債總計");
                Assert.AreEqual(1850621999m, d1.NetWorth, "權益總計");
                Assert.AreEqual(71.33m, d1.NAV, "每股淨值");
                Assert.AreEqual(1339254811m, d1.Revenue, "營業收入");
                Assert.AreEqual(566783698m, d1.BusinessInterest, "營業利益");
                Assert.AreEqual(584777180m, d1.NetProfitTaxFree, "稅前淨利");
                Assert.AreEqual(19.97m, d1.EPS, "每股盈餘");
                Assert.AreEqual(822666212m, d1.BusinessCashflow, "營業活動之淨現金流入");
                Assert.AreEqual(-505781714m, d1.InvestmentCashflow, "投資活動之淨現金流入");
                Assert.AreEqual(-88615087m, d1.FinancingCashflow, "籌資活動之淨現金流入");
            }
        }
        public static void ExecuteTest()
        {
            if (!IsExecuted)
            {
                StockFinReportUpdateJob.Logger = new UnitTestLogger();
                var target = new StockFinReportUpdateJob();
                IJobExecutionContext context = null;
                target.Execute(context);
                IsExecuted = true;
            }
        }
    }
}
#endif