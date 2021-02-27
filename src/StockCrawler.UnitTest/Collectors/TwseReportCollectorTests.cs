using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using StockCrawler.Services.Collectors;
using System.Linq;
#if (DEBUG)
namespace StockCrawler.UnitTest.Collectors
{
    [TestClass]
    public class TwseReportCollectorTests : UnitTestBase
    {
        /// <summary>
        /// 本測試僅能在 110 年二月底對應線上財報公布 109 全年度財報方能成功
        /// 當時的資料下載如 StockCrawler.UnitTest\TestData\TWSE\2330_109_-1_3_2020-04-06_1.html
        /// </summary>
        [TestMethod]
        public void GetStockFinancialReportTest_109Q3()
        {
            var collector = new TwseReportCollector
            {
                _logger = new UnitTestLogger()
            };
            var data = collector.GetStockFinancialReport(TEST_STOCKNO_台積電);
            Assert.IsTrue(data.Any());
            Assert.AreEqual(3, data.Count, "data count");
            var d = data[0];
            _logger.Debug(JsonConvert.SerializeObject(d));
            Assert.AreEqual(TEST_STOCKNO_台積電, d.StockNo);
            Assert.AreEqual(109, d.Year);
            Assert.AreEqual(4, d.Season);
            Assert.AreEqual(2760711405m, d.TotalAssets, "資產總計");
            Assert.AreEqual(910089406m, d.TotalLiability, "負債總計");
            Assert.AreEqual(1850621999m, d.NetWorth, "權益總計");
            Assert.AreEqual(71.33m, d.NAV, "每股淨值");
            Assert.AreEqual(1339254811m, d.Revenue, "營業收入");
            Assert.AreEqual(566783698m, d.BusinessInterest, "營業利益");
            Assert.AreEqual(584777180m, d.NetProfitTaxFree, "稅前淨利");
            Assert.AreEqual(19.97m, d.EPS, "每股盈餘");
            Assert.AreEqual(822666212m, d.BusinessCashflow, "營業活動之淨現金流入");
            Assert.AreEqual(-505781714m, d.InvestmentCashflow, "投資活動之淨現金流入");
            Assert.AreEqual(-88615087m, d.FinancingCashflow, "籌資活動之淨現金流入");

            d = data[1];
            _logger.Debug(JsonConvert.SerializeObject(d));
            Assert.AreEqual(TEST_STOCKNO_台積電, d.StockNo);
            Assert.AreEqual(108, d.Year);
            Assert.AreEqual(4, d.Season);
            Assert.AreEqual(2264805032m, d.TotalAssets, "資產總計");
            Assert.AreEqual(642709606m, d.TotalLiability, "負債總計");
            Assert.AreEqual(1622095426m, d.NetWorth, "權益總計");
            Assert.AreEqual(62.53m, d.NAV, "每股淨值");
            Assert.AreEqual(1069985448m, d.Revenue, "營業收入");
            Assert.AreEqual(372701090m, d.BusinessInterest, "營業利益");
            Assert.AreEqual(389845336m, d.NetProfitTaxFree, "稅前淨利");
            Assert.AreEqual(13.32m, d.EPS, "每股盈餘");
            Assert.AreEqual(615138744m, d.BusinessCashflow, "營業活動之淨現金流入");
            Assert.AreEqual(-458801647m, d.InvestmentCashflow, "投資活動之淨現金流入");
            Assert.AreEqual(-269638166m, d.FinancingCashflow, "籌資活動之淨現金流入");

            d = data[2];
            _logger.Debug(JsonConvert.SerializeObject(d));
            Assert.AreEqual(TEST_STOCKNO_台積電, d.StockNo);
            Assert.AreEqual(107, d.Year);
            Assert.AreEqual(4, d.Season);
            Assert.AreEqual(2090128038m, d.TotalAssets, "資產總計");
            Assert.AreEqual(412631642m, d.TotalLiability, "負債總計");
            Assert.AreEqual(1677496396m, d.NetWorth, "權益總計");
            Assert.AreEqual(64.67m, d.NAV, "每股淨值");
            Assert.AreEqual(1031473557m, d.Revenue, "營業收入");
            Assert.AreEqual(383623524m, d.BusinessInterest, "營業利益");
            Assert.AreEqual(397510263m, d.NetProfitTaxFree, "稅前淨利");
            Assert.AreEqual(13.54m, d.EPS, "每股盈餘");
            Assert.AreEqual(573954308m, d.BusinessCashflow, "營業活動之淨現金流入");
            Assert.AreEqual(-314268908m, d.InvestmentCashflow, "投資活動之淨現金流入");
            Assert.AreEqual(-245124791m, d.FinancingCashflow, "籌資活動之淨現金流入");
        }
    }
}
#endif