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
    public class LazyStockUpdateJobTests : UnitTestBase
    {
        [TestInitialize]
        public override void InitBeforeTest()
        {
            base.InitBeforeTest();
            SqlTool.ExecuteSqlFile(@"..\..\..\..\database\MSSQL\20_initial_data\Stock.data.sql");
        }
        [TestMethod]
        public void ExecuteTest()
        {
            LazyStockUpdateJob.Logger = new UnitTestLogger();
            var target = new LazyStockUpdateJob();
            IJobExecutionContext context = null;
            target.Execute(context);
            using (var db = new StockDataContext(ConnectionStringHelper.StockConnectionString))
            {
                var data = db.GetLazyStockData(TEST_STOCK_NO_1).ToList();
                Assert.AreEqual(1, data.Count);
                Assert.AreEqual(TEST_STOCK_NO_1, data[0].StockNo);
            }
        }
    }
}
#endif