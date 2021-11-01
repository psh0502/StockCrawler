﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            IJobExecutionContext context = new ArgumentJobExecutionContext(target);
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
                    var data = db.GetStockPriceHistoryPaging(TEST_STOCKNO_台積電, Services.SystemTime.Today, Services.SystemTime.Today, 1, 100, 10, ref pageCount).ToList();
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
                    Assert.AreEqual(0.0147M, d1.DeltaPercent);
                    Assert.AreEqual(20.68M, d1.PE);

                    data = db.GetStockPriceHistoryPaging("0000", Services.SystemTime.Today, Services.SystemTime.Today, 1, 100, 10, ref pageCount).ToList();
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
                    Assert.AreEqual(0.0161M, d1.DeltaPercent);
                }
            }
        }
    }
}
#endif