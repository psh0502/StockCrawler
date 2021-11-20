using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz;
using StockCrawler.Dao;
using StockCrawler.Services;
using System;
using System.Linq;
using SystemTime = StockCrawler.Services.SystemTime;

#if (DEBUG)
namespace StockCrawler.UnitTest.Jobs
{
    [TestClass]
    public class StockCaculationJobTests: UnitTestBase
    {
        private static bool IsExecuted = false;
        private const string stockNo = TEST_STOCKNO_台積電; // 台積電
        private static readonly DateTime bgnDate = new DateTime(2020, 1, 2);
        private static readonly DateTime today = new DateTime(2020, 4, 6);
        [TestInitialize]
        public override void InitBeforeTest()
        {
            base.InitBeforeTest();
            SqlTool.ExecuteSqlFile(@"..\..\..\..\database\MSSQL\20_initial_data\Stock.data.sql");
            if (!IsExecuted)
            {
                SystemTime.SetFakeTime(today);
                var target = new StockPriceHistoryInitJob();
                var jobContext = new ArgumentJobExecutionContext(target);
                jobContext.Put("args", new string[] { stockNo });
                target.Execute(jobContext);

                IsExecuted = true;
            }
        }
        [TestMethod]
        public void ExecuteTest()
        {
            StockCaculationJob.Logger = new UnitTestLogger();
            var target = new StockCaculationJob();
            IJobExecutionContext context = new ArgumentJobExecutionContext(target);
            context.Put("args", new string[] { bgnDate.ToDateText(), "2330" });
            target.Execute(context);
            using (var db = new StockDataContext(ConnectionStringHelper.StockConnectionString))
            {
                var q = db.GetStockAveragePrice(
                    TEST_STOCKNO_台積電
                    , SystemTime.Today
                    , SystemTime.Today, -1)
                    .ToDictionary(d => d.Period);
                Assert.AreEqual(272.3M, q[5].ClosePrice);
                Assert.AreEqual(271.1M, q[10].ClosePrice);
                Assert.AreEqual(278.85M, q[20].ClosePrice);
                Assert.AreEqual(311.3621M, q[60].ClosePrice);

                var q1 = db.GetStockTechnicalIndicators(
                    TEST_STOCKNO_台積電
                    , SystemTime.Today
                    , SystemTime.Today, null)
                    .ToDictionary(d => d.Type);
                Assert.AreEqual(65.75M, q1["K"].Value);
                Assert.AreEqual(61.75M, q1["D"].Value);
                //Assert.AreEqual(-12.69M, q1["MACD"].Value);
                //Assert.AreEqual(-10.93M, q1["DIF"].Value);
                //Assert.AreEqual(1.76M, q1["OSC"].Value);
            }
        }
    }
}
#endif