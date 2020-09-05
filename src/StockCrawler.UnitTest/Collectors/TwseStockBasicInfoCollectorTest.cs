using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockCrawler.Services.StockBasicInfo;
using System;

namespace StockCrawler.UnitTest.Collectors
{
    [TestClass]
    public class TwseStockBasicInfoCollectorTest : UnitTestBase
    {
        [TestMethod]
        public void CollectorTestMethod_2330()
        {
            var collector = new TwseStockBasicInfoCollector();
            TwseStockBasicInfoCollector._logger = new UnitTestLogger();
            var r = collector.GetStockBasicInfo("2330");

            Assert.AreEqual("2330", r.StockNo);
            Assert.AreEqual(new DateTime(1987, 2, 21), r.BuildDate);
            Assert.AreEqual(new DateTime(1994, 9, 5), r.PublishDate);
            Assert.AreEqual(259303804580, r.Capital);
            Assert.AreEqual("劉德音", r.Chairman);
            Assert.AreEqual("總裁: 魏哲家", r.CEO);
            Assert.AreEqual("半導體業", r.Category);
            Assert.AreEqual("依客戶之訂單與其提供之產品設計說明，以從事製造與銷售積體電路以及其他晶圓半導體裝置。提供前述產品之封裝與測試服務、積體電路之電腦輔助設計技術服務。提供製造光罩及其設計服務。", r.Businiess);
            Assert.AreEqual("22099131", r.CompanyID);
            Assert.AreEqual("台灣積體電路製造股份有限公司", r.CompanyName);
            Assert.AreEqual(25930380458, r.ReleaseStockCount);
        }
        [TestMethod]
        public void CollectorTestMethod_2888()
        {
            var collector = new TwseStockBasicInfoCollector();
            TwseStockBasicInfoCollector._logger = new UnitTestLogger();
            var r = collector.GetStockBasicInfo("2888");

            Assert.AreEqual("2888", r.StockNo);
            Assert.AreEqual(new DateTime(2002, 2, 19), r.BuildDate);
            Assert.AreEqual(new DateTime(2002, 2, 19), r.PublishDate);
            Assert.AreEqual(130953940630, r.Capital);
            Assert.AreEqual("吳東進", r.Chairman);
            Assert.AreEqual("吳欣儒", r.CEO);
            Assert.AreEqual("金融保險業", r.Category);
            Assert.AreEqual("H801011金融控股公司業", r.Businiess);
            Assert.AreEqual("80328219", r.CompanyID);
            Assert.AreEqual("新光金融控股股份有限公司", r.CompanyName);
            Assert.AreEqual(13020394063, r.ReleaseStockCount);
        }
    }
}
