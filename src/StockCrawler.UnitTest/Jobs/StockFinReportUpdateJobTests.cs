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
                var data = db.GetStockFinancialReport(1, TEST_STOCKNO_台積電, (short)(Services.SystemTime.Today.Year - 1911), 1).ToList();
                Assert.AreEqual(1, data.Count, "資料筆數");
                var d1 = data.First();
                Assert.AreEqual(TEST_STOCKNO_台積電, d1.StockNo);
                Assert.AreEqual(109, d1.Year);
                Assert.AreEqual(1, d1.Season);
                Assert.AreEqual(2635572214, d1.TotalAssets);
                Assert.AreEqual(847305839, d1.TotalLiability);
                Assert.AreEqual(1788266375, d1.NetWorth);
                Assert.AreEqual(68.93, d1.NAV);
                Assert.AreEqual(977721754, d1.Revenue);
                Assert.AreEqual(409663524, d1.BusinessInterest);
                Assert.AreEqual(423669819, d1.NetProfitTaxFree);
                Assert.AreEqual(14.47, d1.EPS);
                Assert.AreEqual(563535628, d1.BusinessCashflow);
                Assert.AreEqual(-414823101,  d1.InvestmentCashflow);
                Assert.AreEqual(12588708, d1.FinancingCashflow);
            }
        }
        public void ExecuteTest()
        {
            if (!IsExecuted)
            {
                Services.SystemTime.SetFakeTime(new DateTime(1911 + 109, 10, 1));
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