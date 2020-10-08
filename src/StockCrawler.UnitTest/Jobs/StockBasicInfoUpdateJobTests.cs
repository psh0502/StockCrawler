using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz;
using StockCrawler.Dao;
using StockCrawler.Services;
using System;
using System.Linq;
#if (DEBUG)
namespace StockCrawler.UnitTest.Jobs
{
    [TestClass]
    public class StockBasicInfoUpdateJobTests : UnitTestBase
    {
        [TestMethod]
        public void ExecuteTest()
        {
            StockBasicInfoUpdateJob.Logger = new UnitTestLogger();
            var target = new StockBasicInfoUpdateJob();
            IJobExecutionContext context = null;
            target.Execute(context);
            using (var db = new StockDataContext(ConnectionStringHelper.StockConnectionString))
            {
                {
                    var data = db.GetStocks().ToList();
                    Assert.AreEqual(45, data.Count);
                    Assert.AreEqual(TEST_STOCK_NO_1, data.Last().StockNo);
                }
                {
                    var data = db.GetStockBasicInfo(TEST_STOCK_NO_1).First();
                    Assert.AreEqual(TEST_STOCK_NO_1, data.StockNo);
                    Assert.AreEqual("台積電", data.StockName);
                    Assert.AreEqual(new DateTime(1987, 2, 21), data.BuildDate);
                    Assert.AreEqual(new DateTime(1994, 9, 5), data.PublishDate);
                    Assert.AreEqual(259303804580M, data.Capital);
                    //Assert.AreEqual(25930380458 * 275.5M, data.MarketValue);
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
#endif