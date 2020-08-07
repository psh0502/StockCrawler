using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockCrawler.Services.StockBasicInfo;
using System;

namespace StockCrawler.UnitTest.JobUnitTest
{
    [TestClass]
    public class GoodInfoDownloaderUnitTest
    {
        [TestMethod]
        public void CollectorTestMethod_2330()
        {
            var collector = new GoodInfoStockBasicInfoCollector();
            var r = collector.GetStockBasicInfo("2330");

            Assert.AreEqual("2330", r.StockNo);
            Assert.AreEqual("台積電", r.StockName);
            Assert.AreEqual(new DateTime(1987, 2, 21), r.BuildDate);
            Assert.AreEqual(new DateTime(1994, 9, 5), r.PublishDate);
            Assert.AreEqual(11300000000000, r.MarketValue);
            Assert.AreEqual(259300000000, r.Capital);
            Assert.AreEqual("總裁: 魏哲家", r.CEO);
            Assert.AreEqual("http://www.tsmc.com", r.Url);
            Assert.AreEqual("半導體業", r.Category);
            Assert.AreEqual("依客戶之訂單與其提供之產品設計說明，以從事製造與銷售積體電路以及其他晶圓半導體裝置。提供前述產品之封裝與測試服務、積體電路之電腦輔助設計技術服務。提供製造光罩及其設計服務。", r.Businiess);
            Assert.AreEqual("22099131", r.CompanyID);
            Assert.AreEqual("台灣積體電路製造股份有限公司", r.CompanyName);
            Assert.AreEqual(25930380458, r.ReleaseStockCount);
        }
        [TestMethod]
        public void CollectorTestMethod_2888()
        {
            var collector = new GoodInfoStockBasicInfoCollector();
            var r = collector.GetStockBasicInfo("2888");

            Assert.AreEqual("2888", r.StockNo);
            Assert.AreEqual("新光金", r.StockName);
            Assert.AreEqual(new DateTime(2002, 2, 19), r.BuildDate);
            Assert.AreEqual(new DateTime(1994, 9, 5), r.PublishDate);
            Assert.AreEqual(11300000000000, r.MarketValue);
            Assert.AreEqual(259300000000, r.Capital);
            Assert.AreEqual("總裁: 魏哲家", r.CEO);
            Assert.AreEqual("http://www.tsmc.com", r.Url);
            Assert.AreEqual("金控業", r.Category);
            Assert.AreEqual("依客戶之訂單與其提供之產品設計說明，以從事製造與銷售積體電路以及其他晶圓半導體裝置。提供前述產品之封裝與測試服務、積體電路之電腦輔助設計技術服務。提供製造光罩及其設計服務。", r.Businiess);
            Assert.AreEqual("22099131", r.CompanyID);
            Assert.AreEqual("台灣積體電路製造股份有限公司", r.CompanyName);
            Assert.AreEqual(25930380458, r.ReleaseStockCount);
        }
    }
}
