using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz;
using StockCrawler.Dao;
using StockCrawler.Services;

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
            using (var db = RepositoryProvider.GetRepositoryInstance())
            {
                var data = db.GetLazyStockData(TEST_STOCKNO_台積電);
                Assert.IsNotNull(data);
                Assert.AreEqual(TEST_STOCKNO_台積電, data.StockNo);
            }
        }
    }
}
#endif