using Common.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz;
using StockCrawler.Dao;
using StockCrawler.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockCrawler.UnitTest.Jobs
{
    [TestClass]
    public class StockCaculationJobTests: UnitTestBase
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
            StockCaculationJob.Logger = new UnitTestLogger();
            var target = new StockCaculationJob();
            IJobExecutionContext context = new ArgumentJobExecutionContext(target);
            target.Execute(context);
            using (var db = new StockDataContext(ConnectionStringHelper.StockConnectionString))
            {
                var q = db.GetStockAveragePrice(TEST_STOCKNO_台積電, new DateTime())
            }
        }
    }
}