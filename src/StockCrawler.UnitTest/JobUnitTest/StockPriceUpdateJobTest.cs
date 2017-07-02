using System;
using System.Data;
using System.Threading;
using StockCrawler.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz;

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
#if(ACCESS)
            #region prepare testing data
            _stockDb.ExecuteCommand.CommandText = "INSERT INTO Stock(StockNo, StockName) VALUES('2002', '中鋼')";
            _stockDb.Execute();
            _stockDb.ExecuteCommand.CommandText = "SELECT MAX(StockID) FROM Stock";
            #endregion

            Thread.Sleep(5 * 1000);
#endif
#if(UNITTEST)
            StockPriceUpdateJob._logger = new UnitTestLogger();
#endif
            StockPriceUpdateJob target = new StockPriceUpdateJob("StockCrawler.Services.StockDailyPrice.TwseStockDailyInfoCollector, StockCrawler.Services");
            IJobExecutionContext context = null;
            target.Execute(context);

#if(ACCESS)
            Thread.Sleep(5 * 1000);

            #region verifying
            _stockDb.SelectCommand.CommandText = "SELECT sph.* FROM Stock s INNER JOIN StockPriceHistory sph ON s.StockID = sph.StockID WHERE s.StockNo = '2002'";
            DataTable dt = new DataTable();
            _stockDb.Fill(dt);
            Assert.AreEqual<int>(1, dt.Rows.Count, "Date result count is incorrect!");
            DataRow dr = dt.Rows[0];
            Assert.AreEqual<long>(18935, Convert.ToInt64(dr["Volumn"]), "Volumn value is incorrect!");
            Assert.AreEqual<DateTime>(DateTime.Today, Convert.ToDateTime(dr["StockDT"]), "StockDT is incorrect!");
            dt.Clear();
            #endregion
#endif
        }
    }
}
