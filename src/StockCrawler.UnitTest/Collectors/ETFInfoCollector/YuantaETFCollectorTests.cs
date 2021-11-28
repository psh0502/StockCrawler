using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockCrawler.Dao;
using StockCrawler.Services;
using StockCrawler.Services.Collectors;
using System;
using System.Linq;

namespace StockCrawler.UnitTest.Collectors
{
#if (DEBUG)
    [TestClass]
    public class YuantaETFCollectorTests : UnitTestBase
    {
        [TestInitialize]
        public override void InitBeforeTest()
        {
            base.InitBeforeTest();
            SqlTool.ExecuteSqlFile(@"..\..\..\..\database\MSSQL\20_initial_data\Stock.data.sql");
        }
        [TestMethod]
        public void GetBasicInfoTest_0050()
        {
            var collector = CollectorServiceProvider.GetETFInfoCollector(StockHelper.GetETFList().First().StockName);
            Assert.AreEqual(typeof(YuantaETFCollector), collector.GetType());
            var basicInfo = collector.GetBasicInfo("0050");

            #region assert basic info
            Assert.AreEqual("0050", basicInfo.StockNo);
            Assert.AreEqual("元大台灣卓越50證券投資信託基金", basicInfo.CompanyName);
            Assert.AreEqual(new DateTime(2003, 6, 25), basicInfo.BuildDate);
            Assert.AreEqual(36.98M, basicInfo.BuildPrice);
            Assert.AreEqual(new DateTime(2003, 6, 30), basicInfo.PublishDate);
            Assert.AreEqual(36.96M, basicInfo.PublishPrice);
            Assert.AreEqual("指數股票型", basicInfo.Category);
            Assert.AreEqual("以完全複製的指數化操作策略，追蹤臺灣50指數之績效表現，臺灣50指數由臺灣證券交易所與英國富時指數編制公司合作編制，成分股是由上市股票中評選出50檔市值最大、符合篩選條件的上市股票；讓您一次買進臺灣股市市值最大的50家上市公司，用小錢投資50檔績優藍籌股，有效分散個股投資風險。指數化產品為最簡單易懂的投資工具，追求長期資本利得之外，還能享受配息，持股內容每季調整，充分掌握產業脈動。", basicInfo.Business);
            Assert.AreEqual("中國信託商業銀行", basicInfo.KeepingBank);
            Assert.AreEqual("許雅惠", basicInfo.CEO);
            Assert.AreEqual(0.0032M, basicInfo.ManagementFee);
            Assert.AreEqual(0.00035M, basicInfo.KeepFee);
            Assert.IsTrue(basicInfo.Distribution);
            Assert.AreEqual(172183831394M, basicInfo.TotalAssetNAV);
            Assert.AreEqual(138.41M, basicInfo.NAV);
            Assert.AreEqual(1244000000, basicInfo.TotalPublish);

            #endregion

        }
        [TestMethod]
        public void GetIngredientsTest_0050()
        {
            var collector = CollectorServiceProvider.GetETFInfoCollector(StockHelper.GetETFList().First().StockName);
            Assert.AreEqual(typeof(YuantaETFCollector), collector.GetType());
            ((YuantaETFCollector)collector)._logger = new UnitTestLogger();
            var ingredients = collector.GetIngredients("0050");

            #region assert ingredients
            Assert.AreEqual(50, ingredients.Length);
            var d1 = ingredients[0];
            Assert.AreEqual("0050", d1.ETFNo);
            Assert.AreEqual("2330", d1.StockNo);
            Assert.AreEqual(138433314, d1.Quantity);
            Assert.AreEqual(0.4792M, d1.Weight);
            #endregion
        }
        [TestMethod]
        public void GetBasicInfoTest_0051()
        {
            var collector = CollectorServiceProvider.GetETFInfoCollector(StockHelper.GetETFList().First().StockName);
            Assert.AreEqual(typeof(YuantaETFCollector), collector.GetType());
            var basicInfo = collector.GetBasicInfo("0051");

            #region assert basic info
            Assert.AreEqual("0051", basicInfo.StockNo);
            Assert.AreEqual("元大台灣中型100證券投資信託基金", basicInfo.CompanyName);
            Assert.AreEqual(new DateTime(2006, 8, 24), basicInfo.BuildDate);
            Assert.AreEqual(26M, basicInfo.BuildPrice);
            Assert.AreEqual(new DateTime(2006, 8, 31), basicInfo.PublishDate);
            Assert.AreEqual(26.49M, basicInfo.PublishPrice);
            Assert.AreEqual("指數股票型", basicInfo.Category);
            Assert.AreEqual("以完全複製的指數化操作策略，追蹤臺灣中型100指數之績效表現，臺灣中型100指數由臺灣證券交易所與英國富時指數編制公司合作編制，成份股是以扣除台灣50指數成份股後，市值前100大的上市公司股票，中型指數成長動能強勁，更勝大型股；一次買進台灣最具成長動能的100家上市公司，用小錢投資100檔高成長績優股票組合，有效分散個股投資風險，持股內容每季調整，充分掌握產業脈動，為個人資產配置的絕佳工具。", basicInfo.Business);
            Assert.AreEqual("台新國際商業銀行", basicInfo.KeepingBank);
            Assert.AreEqual("邱鉦淵", basicInfo.CEO);
            Assert.AreEqual(0M, basicInfo.ManagementFee);
            Assert.AreEqual(0.00035M, basicInfo.KeepFee);
            Assert.IsTrue(basicInfo.Distribution);
            Assert.AreEqual(675082459.00M, basicInfo.TotalAssetNAV);
            Assert.AreEqual(58.7M, basicInfo.NAV);
            Assert.AreEqual(11500000, basicInfo.TotalPublish);

            #endregion
        }
        [TestMethod]
        public void GetIngredientsTest_0051()
        {
            var collector = CollectorServiceProvider.GetETFInfoCollector(StockHelper.GetETFList().First().StockName);
            Assert.AreEqual(typeof(YuantaETFCollector), collector.GetType());
            ((YuantaETFCollector)collector)._logger = new UnitTestLogger();
            var ingredients = collector.GetIngredients("0051");

            #region assert ingredients
            Assert.AreEqual(100, ingredients.Length);
            var d1 = ingredients[0];
            Assert.AreEqual("0051", d1.ETFNo);
            Assert.AreEqual("3037", d1.StockNo);
            Assert.AreEqual(134041, d1.Quantity);
            Assert.AreEqual(0.0412M, d1.Weight);
            #endregion
        }
        [TestMethod]
        public void GetBasicInfoTest_0053()
        {
            var collector = CollectorServiceProvider.GetETFInfoCollector(StockHelper.GetETFList().First().StockName);
            Assert.AreEqual(typeof(YuantaETFCollector), collector.GetType());
            var basicInfo = collector.GetBasicInfo("0053");

            #region assert basic info
            Assert.AreEqual("0053", basicInfo.StockNo);
            Assert.AreEqual("元大台灣ETF傘型證券投資信託基金之電子科技證券投資信託基金", basicInfo.CompanyName);
            Assert.AreEqual(new DateTime(2007, 7, 4), basicInfo.BuildDate);
            Assert.AreEqual(30M, basicInfo.BuildPrice);
            Assert.AreEqual(new DateTime(2007, 7, 16), basicInfo.PublishDate);
            Assert.AreEqual(30.44M, basicInfo.PublishPrice);
            Assert.AreEqual("指數股票型", basicInfo.Category);
            Assert.AreEqual("以最適化複製指數操作策略，追蹤臺灣電子類加權股價指數，電子類加權股價指數，由台灣證券交易所編制，成分股是以所有台灣證券交易所產業分類為「電子類」之上市公司股票，讓您可以一網打盡所有台灣上市電子精英，用小錢投資一籃子股票，有效分散個股投資風險，持股內容即時調整，隨時掌握產業脈動，是參與類股輪動漲勢最簡單的投資工具。", basicInfo.Business);
            Assert.AreEqual("台新國際商業銀行", basicInfo.KeepingBank);
            Assert.AreEqual("許雅惠", basicInfo.CEO);
            Assert.AreEqual(0M, basicInfo.ManagementFee);
            Assert.AreEqual(0.00035M, basicInfo.KeepFee);
            Assert.IsTrue(basicInfo.Distribution);
            Assert.AreEqual(332587270.00M, basicInfo.TotalAssetNAV);
            Assert.AreEqual(66.68M, basicInfo.NAV);
            Assert.AreEqual(4988000, basicInfo.TotalPublish);

            #endregion
        }
        [TestMethod]
        public void GetIngredientsTest_0053()
        {
            var collector = CollectorServiceProvider.GetETFInfoCollector(StockHelper.GetETFList().First().StockName);
            Assert.AreEqual(typeof(YuantaETFCollector), collector.GetType());
            ((YuantaETFCollector)collector)._logger = new UnitTestLogger();
            var ingredients = collector.GetIngredients("0053");

            #region assert ingredients
            Assert.AreEqual(165, ingredients.Length);
            var d1 = ingredients[0];
            Assert.AreEqual("0053", d1.ETFNo);
            Assert.AreEqual("2330", d1.StockNo);
            Assert.AreEqual(238754, d1.Quantity);
            Assert.AreEqual(0.4278M, d1.Weight);
            #endregion
        }
    }
#endif
}