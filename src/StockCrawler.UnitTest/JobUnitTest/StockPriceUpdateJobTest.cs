using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz;
using StockCrawler.Services;
using System;

namespace StockCrawler.UnitTest.JobUnitTest
{
    /// <summary>
    ///This is a test class for StockPriceUpdateJobTest and is intended
    ///to contain all StockPriceUpdateJobTest Unit Tests
    ///</summary>
    [TestClass]
    public class StockPriceUpdateJobTest // : UnitTestBase
    {
        /// <summary>
        ///A test for Execute StockPriceUpdate
        ///</summary>
        [TestMethod()]
        public void StockPriceUpdateTest()
        {
            Services.SystemTime.SetFakeTime(new DateTime(2020, 6, 29));
            StockPriceUpdateJob.Logger = new UnitTestLogger();
            StockPriceUpdateJob target = new StockPriceUpdateJob();
            IJobExecutionContext context = null;
            target.Execute(context);
        }
    }
}
