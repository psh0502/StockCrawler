using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz;
using StockCrawler.Dao;
using StockCrawler.Services;
using System;
using System.Linq;

namespace StockCrawler.UnitTest.Jobs
{
    /// <summary>
    ///This is a test class for StockPriceUpdateJobTest and is intended
    ///to contain all StockPriceUpdateJobTest Unit Tests
    ///</summary>
    [TestClass]
    public class StockPriceUpdateJobTest : UnitTestBase
    {
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
                    var data = db.GetStocks().ToList();
                    Assert.AreEqual(1, data.Count);
                    Assert.AreEqual("2330", data.First().StockNo);
                }
                {
                    int? pageCount = null;
                    var data = db.GetStockPriceHistoryPaging("2330", Services.SystemTime.Today, Services.SystemTime.Today, 1, 100, 1, 10, ref pageCount).ToList();
                    Assert.AreEqual(1, data.Count);
                    Assert.AreEqual(1, pageCount);
                    var d1 = data.First();
                    _logger.DebugFormat("StockNo={0}\r\nStockDT={1}\r\nOpenPrice={2}\r\nHighPrice={3}\r\nLowPrice={4}\r\nClosePrice={5}\r\nVolume={6}\r\nDeltaPrice={7}\r\nDeltaPercent={8}%\r\nPE={9}",
                        d1.StockNo, d1.StockDT.ToShortDateString(), d1.OpenPrice, d1.HighPrice, d1.LowPrice, d1.ClosePrice, d1.Volume, d1.DeltaPrice, (d1.DeltaPercent * 100).ToString("#0.##"), d1.PE);
                    Assert.AreEqual("2330", d1.StockNo);
                    Assert.AreEqual(new DateTime(2020, 4, 6), d1.StockDT);
                    Assert.AreEqual(275.5M, d1.ClosePrice);
                    Assert.AreEqual(273M, d1.OpenPrice);
                    Assert.AreEqual(275.5M, d1.HighPrice);
                    Assert.AreEqual(270M, d1.LowPrice);
                    Assert.AreEqual(59712754, d1.Volume);
                    Assert.AreEqual(4, d1.DeltaPrice);
                    Assert.AreEqual(0.0146M, d1.DeltaPercent);
                    Assert.AreEqual(20.68M, d1.PE);
                }
            }
        }
        [TestMethod]
        public void StockPriceUpdateForSatdayOrSundayNoDataOn0331Test()
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
                    Assert.IsFalse(db.GetStockPriceHistoryPaging("2330", new DateTime(2020, 3, 31), new DateTime(2020, 3, 31), 1, 100, 1, 10, ref pageCount).Any());
                    Assert.IsFalse(db.GetStockPriceHistoryPaging("2330", new DateTime(2020, 3, 1), new DateTime(2020, 3, 1), 20, 100, 1, 10, ref pageCount).Any());
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
                var data = db.GetStockPriceHistoryPaging("2330", new DateTime(2020, 3, 1), new DateTime(2020, 3, 1), 20, 100, 1, 10, ref pageCount).ToList();
                Assert.AreEqual(1, data.Count);
                Assert.AreEqual(1, pageCount);
                var d1 = data.First();
                _logger.DebugFormat("StockNo={0}\r\nStockDT={1}\r\nOpenPrice={2}\r\nHighPrice={3}\r\nLowPrice={4}\r\nClosePrice={5}\r\nVolume={6}\r\nDeltaPrice={7}\r\nDeltaPercent={8}%\r\nPE={9}",
                    d1.StockNo, d1.StockDT.ToShortDateString(), d1.OpenPrice, d1.HighPrice, d1.LowPrice, d1.ClosePrice, d1.Volume, d1.DeltaPrice, (d1.DeltaPercent * 100).ToString("#0.##"), d1.PE);
                Assert.AreEqual("2330", d1.StockNo);
                Assert.AreEqual(new DateTime(2020, 3, 1), d1.StockDT);
                Assert.AreEqual(274M, d1.ClosePrice);
                Assert.AreEqual(308M, d1.OpenPrice);
                Assert.AreEqual(326M, d1.HighPrice);
                Assert.AreEqual(235.5M, d1.LowPrice);
                Assert.AreEqual(1906813004, d1.Volume);
                Assert.AreEqual(0, d1.DeltaPrice);
                Assert.AreEqual(0M, d1.DeltaPercent);
                Assert.AreEqual(0M, d1.PE);
            }
        }
        [TestMethod]
        public void StockPriceUpdateForSatdayOrSundayNoDataOn0405Test()
        {
            int? pageCount = null;
            Services.SystemTime.SetFakeTime(new DateTime(2020, 4, 2));
            {
                StockPriceHistoryInitJob.Logger = new UnitTestLogger();
                var target = new StockPriceHistoryInitJob();
                IJobExecutionContext context = null;
                target.Execute(context);

                using (var db = new StockDataContext(ConnectionStringHelper.StockConnectionString))
                {
                    Assert.IsFalse(db.GetStockPriceHistoryPaging("2330", new DateTime(2020, 4, 3), new DateTime(2020, 4, 3), 1, 100, 1, 10, ref pageCount).Any());
                    Assert.IsFalse(db.GetStockPriceHistoryPaging("2330", new DateTime(2020, 3, 30), new DateTime(2020, 3, 30), 5, 100, 1, 10, ref pageCount).Any());
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
                var data = db.GetStockPriceHistoryPaging("2330", new DateTime(2020, 3, 30), new DateTime(2020, 3, 30), 5, 100, 1, 10, ref pageCount).ToList();
                Assert.AreEqual(1, data.Count);
                Assert.AreEqual(1, pageCount);
                var d1 = data.First();
                _logger.DebugFormat("StockNo={0}\r\nStockDT={1}\r\nOpenPrice={2}\r\nHighPrice={3}\r\nLowPrice={4}\r\nClosePrice={5}\r\nVolume={6}\r\nDeltaPrice={7}\r\nDeltaPercent={8}%\r\nPE={9}",
                    d1.StockNo, d1.StockDT.ToShortDateString(), d1.OpenPrice, d1.HighPrice, d1.LowPrice, d1.ClosePrice, d1.Volume, d1.DeltaPrice, (d1.DeltaPercent * 100).ToString("#0.##"), d1.PE);
                Assert.AreEqual("2330", d1.StockNo);
                Assert.AreEqual(new DateTime(2020, 3, 30), d1.StockDT);
                Assert.AreEqual(274M, d1.ClosePrice);
                Assert.AreEqual(308M, d1.OpenPrice);
                Assert.AreEqual(326M, d1.HighPrice);
                Assert.AreEqual(235.5M, d1.LowPrice);
                Assert.AreEqual(1906813004, d1.Volume);
                Assert.AreEqual(0, d1.DeltaPrice);
                Assert.AreEqual(0M, d1.DeltaPercent);
                Assert.AreEqual(0M, d1.PE);
            }
        }
    }
}
