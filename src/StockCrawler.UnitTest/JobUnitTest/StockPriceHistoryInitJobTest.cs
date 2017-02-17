using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz;
using StockCrawler.Services;
using System.Threading;
using TomTang.DbAccess;

namespace StockCrawler.UnitTest.JobUnitTest
{
    /// <summary>
    ///This is a test class for StockPriceHistoryInitJobTest and is intended
    ///to contain all StockPriceHistoryInitJobTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StockPriceHistoryInitJobTest : UnitTestBase
    {
        /// <summary>
        ///A test for Execute StockPriceHistoryInit
        ///</summary>
        [TestMethod()]
        public void StockPriceHistoryInitTest()
        {
            #region testing data
            _stockDb.ExecuteCommand.CommandText = string.Format("INSERT INTO Stock(StockNo, StockName) VALUES({0}, {1})",
                OleDbStrHelper.getParamStr("1101"),
                OleDbStrHelper.getParamStr("台泥"));
            _stockDb.Execute();
            _stockDb.ExecuteCommand.CommandText = string.Format("INSERT INTO Stock(StockNo, StockName) VALUES({0}, {1})",
                OleDbStrHelper.getParamStr("1102"),
                OleDbStrHelper.getParamStr("亞泥"));
            _stockDb.Execute();
            _stockDb.ExecuteCommand.CommandText = string.Format("INSERT INTO Stock(StockNo, StockName) VALUES({0}, {1})",
                OleDbStrHelper.getParamStr("1103"),
                OleDbStrHelper.getParamStr("嘉泥"));
            _stockDb.Execute();

            Thread.Sleep(5 * 1000);
            #endregion

            StockPriceHistoryInitJob target = new StockPriceHistoryInitJob();
            IJobExecutionContext context = null;
            target.Execute(context);
        }
    }
}
