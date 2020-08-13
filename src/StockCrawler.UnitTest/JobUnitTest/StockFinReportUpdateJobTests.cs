using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz;
using StockCrawler.Services;
using System;

namespace StockCrawler.UnitTest.JobUnitTest
{
    [TestClass()]
    public class StockFinReportUpdateJobTests : UnitTestBase
    {
        [TestMethod()]
        public void ExecuteTest()
        {
            Services.SystemTime.SetFakeTime(new DateTime(2020, 8, 1));
            StockPriceUpdateJob.Logger = new UnitTestLogger();
            StockPriceUpdateJob target = new StockPriceUpdateJob();
            IJobExecutionContext context = null;
            target.Execute(context);
            Assert.Fail();
        }
    }
}