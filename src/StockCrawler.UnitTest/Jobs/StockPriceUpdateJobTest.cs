using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz;
using StockCrawler.Dao;
using StockCrawler.Services;
using System;
using System.Linq;

#if (DEBUG)
namespace StockCrawler.UnitTest.Jobs
{
    /// <summary>
    ///This is a test class for StockPriceUpdateJobTest and is intended
    ///to contain all StockPriceUpdateJobTest Unit Tests
    ///</summary>
    [TestClass]
    public class StockPriceUpdateJobTest : UnitTestBase
    {
        [TestInitialize]
        public override void InitBeforeTest()
        {
            base.InitBeforeTest();
            SqlTool.ExecuteSqlFile(@"..\..\..\..\database\MSSQL\20_initial_data\Stock.data.sql");
        }
        /// <summary>
        ///A test for Execute StockPriceUpdate
        ///</summary>
        [TestMethod]
        public void StockPriceUpdateTest()
        {
            Services.SystemTime.SetFakeTime(new DateTime(2020, 4, 6));
            StockPriceUpdateJob.Logger = new UnitTestLogger();
            var target = new StockPriceUpdateJob();
            IJobExecutionContext context = null;
            target.Execute(context);

            using (var db = new StockDataContext(ConnectionStringHelper.StockConnectionString))
            {
                {
                    var data = db.GetStocks(null).ToList();
                    Assert.AreEqual(1155, data.Count);
                    Assert.IsTrue(data.Where(d => d.StockNo == TEST_STOCKNO_台積電).Any());
                }
                {
                    int? pageCount = null;
                    var data = db.GetStockPriceHistoryPaging(TEST_STOCKNO_台積電, Services.SystemTime.Today, Services.SystemTime.Today, 1, 100, 1, 10, ref pageCount).ToList();
                    Assert.AreEqual(1, data.Count);
                    Assert.AreEqual(1, pageCount);
                    var d1 = data.First();
                    _logger.DebugFormat("StockNo={0}\r\nStockDT={1}\r\nOpenPrice={2}\r\nHighPrice={3}\r\nLowPrice={4}\r\nClosePrice={5}\r\nVolume={6}\r\nDeltaPrice={7}\r\nDeltaPercent={8}%\r\nPE={9}",
                        d1.StockNo, d1.StockDT.ToShortDateString(), d1.OpenPrice, d1.HighPrice, d1.LowPrice, d1.ClosePrice, d1.Volume, d1.DeltaPrice, (d1.DeltaPercent * 100).ToString("#0.##"), d1.PE);
                    Assert.AreEqual(TEST_STOCKNO_台積電, d1.StockNo);
                    Assert.AreEqual(new DateTime(2020, 4, 6), d1.StockDT);
                    Assert.AreEqual(273M, d1.OpenPrice);
                    Assert.AreEqual(275.5M, d1.HighPrice);
                    Assert.AreEqual(270M, d1.LowPrice);
                    Assert.AreEqual(275.5M, d1.ClosePrice);
                    Assert.AreEqual(59712754, d1.Volume);
                    Assert.AreEqual(4, d1.DeltaPrice);
                    Assert.AreEqual(0.0146M, d1.DeltaPercent);
                    Assert.AreEqual(20.68M, d1.PE);

                    data = db.GetStockPriceHistoryPaging("0000", Services.SystemTime.Today, Services.SystemTime.Today, 1, 100, 1, 10, ref pageCount).ToList();
                    Assert.AreEqual(1, data.Count);
                    Assert.AreEqual(1, pageCount);
                    d1 = data.First();
                    _logger.DebugFormat("StockNo={0}\r\nStockDT={1}\r\nOpenPrice={2}\r\nHighPrice={3}\r\nLowPrice={4}\r\nClosePrice={5}\r\nVolume={6}\r\nDeltaPrice={7}\r\nDeltaPercent={8}%\r\nPE={9}",
                        d1.StockNo, d1.StockDT.ToShortDateString(), d1.OpenPrice, d1.HighPrice, d1.LowPrice, d1.ClosePrice, d1.Volume, d1.DeltaPrice, (d1.DeltaPercent * 100).ToString("#0.##"), d1.PE);
                    Assert.AreEqual("0000", d1.StockNo);
                    Assert.AreEqual(new DateTime(2020, 4, 6), d1.StockDT);
                    Assert.AreEqual(9663.6300M, d1.OpenPrice);
                    Assert.AreEqual(9818.74M, d1.HighPrice);
                    Assert.AreEqual(9663.6300M, d1.LowPrice);
                    Assert.AreEqual(9818.74M, d1.ClosePrice);
                    Assert.AreEqual(4521499478, d1.Volume);
                    Assert.AreEqual(155.11M, d1.DeltaPrice);
                    Assert.AreEqual(0.0160M, d1.DeltaPercent);
                }
            }
        }
        [TestMethod]
        public void StockPriceUpdateForSaturdayOrSundayNoDataOn0331Test()
        {
            int? pageCount = null;
            Services.SystemTime.SetFakeTime(new DateTime(2020, 3, 30));
            {
                StockPriceHistoryInitJob.Logger = new UnitTestLogger();
                var target = new StockPriceHistoryInitJob();
                IJobExecutionContext context = null;
                target.Execute(context);

                using (var db = new StockDataContext(ConnectionStringHelper.StockConnectionString))
                {
                    Assert.IsFalse(db.GetStockPriceHistoryPaging(TEST_STOCKNO_台積電, new DateTime(2020, 3, 31), new DateTime(2020, 3, 31), 1, 100, 1, 10, ref pageCount).Any());
                    Assert.IsFalse(db.GetStockPriceHistoryPaging(TEST_STOCKNO_台積電, new DateTime(2020, 3, 1), new DateTime(2020, 3, 1), 20, 100, 1, 10, ref pageCount).Any());
                }
            }
            Services.SystemTime.SetFakeTime(new DateTime(2020, 3, 31));
            {
                StockPriceUpdateJob.Logger = new UnitTestLogger();
                var target = new StockPriceUpdateJob();
                IJobExecutionContext context = null;
                target.Execute(context);
            }
            using (var db = new StockDataContext(ConnectionStringHelper.StockConnectionString))
            {
                var data = db.GetStockPriceHistoryPaging(
                    TEST_STOCKNO_台積電
                    , new DateTime(2020, 3, 1)
                    , new DateTime(2020, 3, 1)
                    , 20
                    , 100
                    , 1
                    , 10
                    , ref pageCount).ToList();

                Assert.AreEqual(1, data.Count);
                Assert.AreEqual(1, pageCount);
                var d1 = data.First();
                _logger.DebugFormat("StockNo={0}\r\nStockDT={1}\r\nOpenPrice={2}\r\nHighPrice={3}\r\nLowPrice={4}\r\nClosePrice={5}\r\nVolume={6}\r\nDeltaPrice={7}\r\nDeltaPercent={8}%\r\nPE={9}",
                    d1.StockNo, d1.StockDT.ToShortDateString(), d1.OpenPrice, d1.HighPrice, d1.LowPrice, d1.ClosePrice, d1.Volume, d1.DeltaPrice, (d1.DeltaPercent * 100).ToString("#0.##"), d1.PE);
                Assert.AreEqual(TEST_STOCKNO_台積電, d1.StockNo, "StockNo");
                Assert.AreEqual(new DateTime(2020, 3, 1), d1.StockDT, "StockDT");
                Assert.AreEqual(308M, d1.OpenPrice, "OpenPrice");
                Assert.AreEqual(326M, d1.HighPrice, "HighPrice");
                Assert.AreEqual(235.5M, d1.LowPrice, "LowPrice");
                Assert.AreEqual(267.5M, d1.ClosePrice, "ClosePrice");
                Assert.AreEqual(1852911081, d1.Volume, "Volume");
                Assert.AreEqual(0, d1.DeltaPrice, "DeltaPrice");
                Assert.AreEqual(0M, d1.DeltaPercent, "DeltaPercent");
                Assert.AreEqual(0M, d1.PE, "PE");
            }
        }
        [TestMethod]
        public void StockPriceUpdateForSaturdayOrSundayNoDataOn0405Test()
        {
            base.InitBeforeTest();
            SqlTool.ExecuteSqlFile(@"..\..\..\..\database\MSSQL\20_initial_data\Stock.data.sql");
            StockHelper.Reload();
            int? pageCount = null;
            Services.SystemTime.SetFakeTime(new DateTime(2020, 4, 2));
            {
                StockPriceHistoryInitJob.Logger = new UnitTestLogger();
                var target = new StockPriceHistoryInitJob();
                IJobExecutionContext context = null;
                target.Execute(context);

                using (var db = new StockDataContext(ConnectionStringHelper.StockConnectionString))
                {
                    Assert.IsFalse(db.GetStockPriceHistoryPaging(TEST_STOCKNO_台積電, new DateTime(2020, 4, 3), new DateTime(2020, 4, 3), 1, 100, 1, 10, ref pageCount).Any());
                    Assert.IsFalse(db.GetStockPriceHistoryPaging(TEST_STOCKNO_台積電, new DateTime(2020, 3, 30), new DateTime(2020, 3, 30), 5, 100, 1, 10, ref pageCount).Any());
                }
            }
            Services.SystemTime.SetFakeTime(new DateTime(2020, 4, 3));
            {
                StockPriceUpdateJob.Logger = new UnitTestLogger();
                var target = new StockPriceUpdateJob();
                IJobExecutionContext context = null;
                target.Execute(context);
            }
            using (var db = new StockDataContext(ConnectionStringHelper.StockConnectionString))
            {
                var data = db.GetStockPriceHistoryPaging(TEST_STOCKNO_台積電, new DateTime(2020, 3, 30), new DateTime(2020, 3, 30), 5, 100, 1, 10, ref pageCount).ToList();
                Assert.AreEqual(1, data.Count);
                Assert.AreEqual(1, pageCount);
                var d1 = data.First();
                _logger.DebugFormat("StockNo={0}\r\nStockDT={1}\r\nOpenPrice={2}\r\nHighPrice={3}\r\nLowPrice={4}\r\nClosePrice={5}\r\nVolume={6}\r\nDeltaPrice={7}\r\nDeltaPercent={8}%\r\nPE={9}",
                    d1.StockNo, d1.StockDT.ToShortDateString(), d1.OpenPrice, d1.HighPrice, d1.LowPrice, d1.ClosePrice, d1.Volume, d1.DeltaPrice, (d1.DeltaPercent * 100).ToString("#0.##"), d1.PE);
                Assert.AreEqual(TEST_STOCKNO_台積電, d1.StockNo);
                Assert.AreEqual(new DateTime(2020, 3, 30), d1.StockDT);
                Assert.AreEqual(263.5M, d1.OpenPrice);
                Assert.AreEqual(276.5M, d1.HighPrice);
                Assert.AreEqual(262.5M, d1.LowPrice);
                Assert.AreEqual(271.5M, d1.ClosePrice);
                Assert.AreEqual(154877913, d1.Volume);
                Assert.AreEqual(0, d1.DeltaPrice);
                Assert.AreEqual(0M, d1.DeltaPercent);
                Assert.AreEqual(0M, d1.PE);
            }
        }
    }
}
#endif