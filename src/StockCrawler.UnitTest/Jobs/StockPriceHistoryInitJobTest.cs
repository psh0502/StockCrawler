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
    /// <summary>
    ///This is a test class for StockPriceHistoryInitJobTest and is intended
    ///to contain all StockPriceHistoryInitJobTest Unit Tests
    ///</summary>
    [TestClass]
    public class StockPriceHistoryInitJobTest : UnitTestBase
    {
        private static bool IsExecuted = false;
        private const string stockNo = TEST_STOCKNO_台積電; // 台積電
        private static readonly DateTime today = new DateTime(2020, 4, 6);
        [TestInitialize]
        public override void InitBeforeTest()
        {
            if (!IsExecuted)
            {
                base.InitBeforeTest();
                SqlTool.ExecuteSqlFile(@"..\..\..\..\database\MSSQL\20_initial_data\Stock.data.sql");
                ExecuteTest();
            }
        }
        [TestMethod]
        public void ExecuteTest()
        {
            if (!IsExecuted)
            {
                SystemTime.SetFakeTime(today);
                StockPriceHistoryInitJob.Logger = new UnitTestLogger();
                var target = new StockPriceHistoryInitJob();
                IJobExecutionContext context = null;
                target.Execute(context);
                IsExecuted = true;
            }
        }

        /// <summary>
        ///A test for Execute StockPriceHistoryInit
        ///</summary>
        [TestMethod]
        public void PriceDailyDataInit2330Test()
        {
            using (var db = new StockDataContext(ConnectionStringHelper.StockConnectionString))
            {
                int? pageCount = null;
                var data = db.GetStockPriceHistoryPaging(stockNo, today, today, 100, 1, 10, ref pageCount).ToList();
                Assert.AreEqual(1, data.Count);
                Assert.AreEqual(1, pageCount);
                var d1 = data.First();
                _logger.DebugFormat("StockNo={0}\r\nStockDT={1}\r\nOpenPrice={2}\r\nHighPrice={3}\r\nLowPrice={4}\r\nClosePrice={5}\r\nVolume={6}\r\nDeltaPrice={7}\r\nDeltaPercent={8}%\r\nPE={9}",
                    d1.StockNo, d1.StockDT.ToShortDateString(), d1.OpenPrice, d1.HighPrice, d1.LowPrice, d1.ClosePrice, d1.Volume, d1.DeltaPrice, (d1.DeltaPercent * 100).ToString("#0.##"), d1.PE);
                Assert.AreEqual(new DateTime(2020, 4, 6), d1.StockDT);
                Assert.AreEqual(stockNo, d1.StockNo);
                Assert.AreEqual(273.00M, d1.OpenPrice, "日開盤價");
                Assert.AreEqual(275.50M, d1.HighPrice, "日最高價");
                Assert.AreEqual(270.00M, d1.LowPrice, "日最低價");
                Assert.AreEqual(275.50M, d1.ClosePrice, "日收盤價");
                Assert.AreEqual(59712754, d1.Volume, "日成交量");
                Assert.AreEqual(4, d1.DeltaPrice);
                Assert.AreEqual(0.0147M, d1.DeltaPercent);
                Assert.AreEqual(20.68M, d1.PE);
            }
        }
        [TestMethod]
        public void PriceDailyDataInit2888Test()
        {
            using (var db = new StockDataContext(ConnectionStringHelper.StockConnectionString))
            {
                short period = 1;
                int? pageCount = null;
                var data = db.GetStockPriceHistoryPaging(
                    "2888",
                    new DateTime(2020, 3, 1),
                    new DateTime(2020, 3, 31),
                    period, 100, 10, ref pageCount).ToList();

                Assert.AreEqual(10, data.Count);
                Assert.AreEqual(3, pageCount);
                var d1 = data.First();
                _logger.DebugFormat("StockNo={0}\r\nStockDT={1}\r\nOpenPrice={2}\r\nHighPrice={3}\r\nLowPrice={4}\r\nClosePrice={5}\r\nVolume={6}\r\nDeltaPrice={7}\r\nDeltaPercent={8}%\r\nPE={9}",
                    d1.StockNo, d1.StockDT.ToShortDateString(), d1.OpenPrice, d1.HighPrice, d1.LowPrice, d1.ClosePrice, d1.Volume, d1.DeltaPrice, (d1.DeltaPercent * 100).ToString("#0.##"), d1.PE);
                Assert.AreEqual("2888", d1.StockNo);
                Assert.AreEqual(new DateTime(2020, 3, 31), d1.StockDT);
                Assert.AreEqual(7.92M, d1.OpenPrice);
                Assert.AreEqual(7.95M, d1.HighPrice);
                Assert.AreEqual(7.61M, d1.LowPrice);
                Assert.AreEqual(7.63M, d1.ClosePrice);
                Assert.AreEqual(78073651, d1.Volume);
            }
        }
    }
}
#endif