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
    public class StockForumsUpdateJobTests : UnitTestBase
    {
        [TestInitialize]
        public override void InitBeforeTest()
        {
            base.InitBeforeTest();
            SqlTool.ExecuteSql("TRUNCATE TABLE StockBasicInfo");
            SqlTool.ExecuteSql("DELETE Stock");
            SqlTool.ExecuteSqlFile(@"..\..\..\StockCrawler.UnitTest\TestData\Sql\Stock.data.sql");
        }
        [TestMethod]
        public void ExecuteTest()
        {
            StockForumsUpdateJob.Logger = new UnitTestLogger();
            var target = new StockForumsUpdateJob();
            Services.SystemTime.SetFakeTime(new DateTime(2021, 1, 5));
            IJobExecutionContext context = null;
            target.Execute(context);
            using (var db = StockDataServiceProvider.GetServiceInstance())
            {
                var data = db.GetStockMarketNews(10, TEST_STOCK_NO_1, "mops", new DateTime(2021, 1, 5), new DateTime(2021, 1, 5));
                Assert.IsTrue(data.Any());
                foreach (var d in data)
                {
                    _logger.DebugFormat("{0}\t{1}\t{2}", d.StockNo, d.Subject, d.Url);
                    Assert.AreEqual(TEST_STOCK_NO_1, d.StockNo);
                }
                data = db.GetStockMarketNews(10, null, "twse", new DateTime(2021, 1, 5), new DateTime(2021, 1, 5));
                Assert.IsTrue(data.Any());
                foreach (var d in data)
                {
                    _logger.DebugFormat("{0}\t{1}\t{2}", d.StockNo, d.Subject, d.Url);
                    Assert.AreEqual("0000", d.StockNo);
                }
                var data2 = db.GetStockForumData(1000, new DateTime(2021, 1, 5), new DateTime(2021, 1, 5), null, "2340");
                Assert.IsTrue(data2.Any());
                foreach (var d in data2)
                {
                    _logger.DebugFormat("{0}\t{1}\t{2}", d.StockNo, d.Subject, d.Url);
                    Assert.AreEqual("光磊", d.StockName);
                    Assert.AreEqual("2340", d.StockNo);
                }
            }
        }
    }
}
#endif