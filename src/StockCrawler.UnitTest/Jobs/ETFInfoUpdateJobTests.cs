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
    public class ETFInfoUpdateJobTests : UnitTestBase
    {
        [TestInitialize]
        public override void InitBeforeTest()
        {
            base.InitBeforeTest();
            SqlTool.ExecuteSqlFile(@"..\..\..\..\database\MSSQL\20_initial_data\Stock.data.sql");
        }
        [TestMethod]
        public void ExecuteTest()
        {
            ETFInfoUpdateJob.Logger = new UnitTestLogger();
            var target = new ETFInfoUpdateJob();
            IJobExecutionContext context = new ArgumentJobExecutionContext(target);
            target.Execute(context);
            using (var db = new StockDataContext(ConnectionStringHelper.StockConnectionString))
            {
                var basicInfo = db.GetETFBasicInfo("0050").First();

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

                var ingredients = db.GetETFIngredients("0050").ToList();
                #region assert ingredients
                Assert.AreEqual(50, ingredients.Count);
                var d1 = (from dd in ingredients 
                         where dd.StockNo == "2330" 
                         select dd).First();
                Assert.AreEqual("0050", d1.ETFNo);
                Assert.AreEqual("2330", d1.StockNo);
                Assert.AreEqual(138433314, d1.Quantity);
                Assert.AreEqual(0.4792M, d1.Weight);
                #endregion
            }
        }
    }
}
#endif