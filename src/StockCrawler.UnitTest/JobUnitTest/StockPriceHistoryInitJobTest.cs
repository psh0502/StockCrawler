using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz;
using StockCrawler.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace StockCrawler.UnitTest.JobUnitTest
{
    /// <summary>
    ///This is a test class for StockPriceHistoryInitJobTest and is intended
    ///to contain all StockPriceHistoryInitJobTest Unit Tests
    ///</summary>
    [TestClass]
    public class StockPriceHistoryInitJobTest
    {
        /// <summary>
        ///A test for Execute StockPriceHistoryInit
        ///</summary>
        [TestMethod]
        public void StockPriceHistoryInitTest()
        {
            StockPriceHistoryInitJob.Logger = new UnitTestLogger();
            StockPriceHistoryInitJob target = new StockPriceHistoryInitJob();
            IJobExecutionContext context = null;
            target.Execute(context);
        }
    }
}
