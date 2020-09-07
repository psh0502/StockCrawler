﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz;
using StockCrawler.Dao;
using StockCrawler.Services;
using StockCrawler.Services.StockBasicInfo;
using System;
using System.Linq;

namespace StockCrawler.UnitTest.Jobs
{
    [TestClass]
    public class StockBasicInfoUpdateJobTests : UnitTestBase
    {
        [TestInitialize]
        public override void Init()
        {
            base.Init();
            var data = new GoodInfoStockBasicInfoCollector().GetStockBasicInfo("2330");
            using(var db = new StockDataContext(ConnectionStringHelper.StockConnectionString))
            {

                var sql = @"INSERT INTO[dbo].[StockBasicInfo]
                   ([StockNo], [Category], [CompanyName], [CompanyID]
                    , [BuildDate], [PublishDate]
                    , [Capital], [MarketValue], [ReleaseStockCount], [Chairman]
                    , [CEO], [Url], [Business])
                 VALUES
                       ('2330', N'" + data.Category + "', N'" + data.CompanyName + "','" + data.CompanyID + @"'
                        ,'" + data.BuildDate.ToString("yyyy-MM-dd") + "', '" + data.PublishDate.ToString("yyyy-MM-dd") + @"'
                       ," + data.Capital + ", " + data.MarketValue + ", " + data.ReleaseStockCount + ", N'" + data.Chairman + @"'
                       ,N'" + data.CEO + "', '" + data.Url + "', N'" + data.Business + "')";
                _logger.Info("sql=" + sql);
                db.ExecuteCommand(sql);
            }
        }
        [TestMethod]
        public void ExecuteTest()
        {
            StockBasicInfoUpdateJob.Logger = new UnitTestLogger();
            StockBasicInfoUpdateJob target = new StockBasicInfoUpdateJob();
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
                    var data = db.GetStockBasicInfo("2330").First();
                    Assert.AreEqual("2330", data.StockNo);
                    Assert.AreEqual("台積電", data.StockName);
                    Assert.AreEqual(new DateTime(1987, 2, 21), data.BuildDate);
                    Assert.AreEqual(new DateTime(1994, 9, 5), data.PublishDate);
                    Assert.AreEqual(259303804580, data.Capital);
                    Assert.AreNotEqual(0, data.MarketValue);
                    Assert.AreEqual("劉德音", data.Chairman);
                    Assert.AreEqual("總裁: 魏哲家", data.CEO);
                    Assert.AreEqual("http://www.tsmc.com", data.Url);
                    Assert.AreEqual("半導體業", data.Category);
                    Assert.AreEqual("依客戶之訂單與其提供之產品設計說明，以從事製造與銷售積體電路以及其他晶圓半導體裝置。提供前述產品之封裝與測試服務、積體電路之電腦輔助設計技術服務。提供製造光罩及其設計服務。", data.Business);
                    Assert.AreEqual("22099131", data.CompanyID);
                    Assert.AreEqual("台灣積體電路製造股份有限公司", data.CompanyName);
                    Assert.AreEqual(25930380458, data.ReleaseStockCount);
                }
            }
        }
    }
}