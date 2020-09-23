using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockCrawler.Services.Collectors;

namespace StockCrawler.UnitTest.Collectors
{
    [TestClass]
    public class TwseReportCollectorTests : UnitTestBase
    {
        [TestMethod]
        public void GetStockReportCashFlowTest_109Q1()
        {
            var collector = new TwseReportCollector
            {
                _logger = new UnitTestLogger()
            };
            var data = collector.GetStockReportCashFlow("2330", 109, 1);
            Assert.IsNotNull(data);
            Assert.AreEqual("2330", data.StockNo);
            Assert.AreEqual(109, data.Year);
            Assert.AreEqual(1, data.Season);
            Assert.AreEqual(67083741, data.Depreciation, "折舊");
            Assert.AreEqual(1470736, data.AmortizationFee, "攤銷");
            Assert.AreEqual(203029442, data.BusinessCashflow, "營業現金流");
            Assert.AreEqual(-188993268, data.InvestmentCashflow, "投資現金流");
            Assert.AreEqual(-40757411, data.FinancingCashflow, "融資現金流");
            //Assert.AreEqual(-192063846, data.CapitalExpenditures, "資本支出");    // 不知如何計算此數值
            Assert.AreEqual(14036174, data.FreeCashflow, "自由現金流");
            Assert.AreEqual(-26721237, data.NetCashflow, "淨現金流");
        }
        [TestMethod]
        public void GetStockReportCashFlowTest_108Q4()
        {
            TwseReportCollector collector = new TwseReportCollector
            {
                _logger = new UnitTestLogger()
            };
            var data = collector.GetStockReportCashFlow("2330", 108, 4);
            Assert.IsNotNull(data);
            Assert.AreEqual("2330", data.StockNo);
            Assert.AreEqual(108, data.Year);
            Assert.AreEqual(4, data.Season);
            Assert.AreEqual(281411832, data.Depreciation, "折舊");
            Assert.AreEqual(5472409, data.AmortizationFee, "攤銷");
            Assert.AreEqual(615138744, data.BusinessCashflow, "營業現金流");
            Assert.AreEqual(-458801647, data.InvestmentCashflow, "投資現金流");
            Assert.AreEqual(-269638166, data.FinancingCashflow, "融資現金流");
            //Assert.AreEqual(-170009539, data.CapitalExpenditures, "資本支出");    // 不知如何計算此數值
            Assert.AreEqual(156337097, data.FreeCashflow, "自由現金流");
            Assert.AreEqual(-113301069, data.NetCashflow, "淨現金流");
        }
        [TestMethod]
        public void GetStockReportIncomeTest_109Q1()
        {
            TwseReportCollector collector = new TwseReportCollector
            {
                _logger = new UnitTestLogger()
            };
            var data = collector.GetStockReportIncome("2330", 108, 4);
            Assert.IsNotNull(data);
            Assert.AreEqual("2330", data.StockNo);
            Assert.AreEqual(108, data.Year);
            Assert.AreEqual(4, data.Season);
            Assert.AreEqual(1069985448, data.Revenue);
            Assert.AreEqual(492698501, data.GrossProfit);
            Assert.AreEqual(6348626, data.SalesExpense);
            Assert.AreEqual(21737210, data.ManagementCost);
            Assert.AreEqual(91418746, data.RDExpense);
            Assert.AreEqual(119504582, data.OperatingExpenses);
            Assert.AreEqual(372701090, data.BusinessInterest);
            Assert.AreEqual(389845336, data.NetProfitTaxFree);
            Assert.AreEqual(345343809, data.NetProfitTaxed);
            Assert.AreEqual(13.32M, data.EPS, "每股盈餘(EPS)");
        }
        [TestMethod]
        public void GetStockReportBalanceTest_109Q1()
        {
            TwseReportCollector collector = new TwseReportCollector
            {
                _logger = new UnitTestLogger()
            };
            var data = collector.GetStockReportBalance("2330", 109, 1);
            Assert.IsNotNull(data);
            Assert.AreEqual("2330", data.StockNo);
            Assert.AreEqual(109, data.Year);
            Assert.AreEqual(1, data.Season);

            #region 資產
            Assert.AreEqual(430777229, data.CashAndEquivalents);    // 現金及約當現金
            Assert.AreEqual(1254253, data.ShortInvestments);    // 短期投資
            Assert.AreEqual(146420632, data.BillsReceivable);   // 應收帳款及票據
            Assert.AreEqual(78277834, data.Stock);  // 存貨
            //Assert.AreEqual(145740092, data.OtherCurrentAssets);    // 其餘流動資產 // 不知如何計算此數值
            Assert.AreEqual(802470040, data.CurrentAssets);     // 流動資產
            Assert.AreEqual(19381760, data.LongInvestment);     // 長期投資
            Assert.AreEqual(1438215285, data.FixedAssets);      // 固定資產
            Assert.AreEqual(83228611, data.OtherAssets);        // 其餘資產
            Assert.AreEqual(2343295696, data.TotalAssets);      // 總資產
            #endregion

            #region 負債
            Assert.AreEqual(139310384, data.ShortLoan); // 短期借款
            Assert.AreEqual(2992858, data.ShortBillsPayable);   // 應付短期票券
            Assert.AreEqual(39774214, data.AccountsAndBillsPayable); //應付帳款及票據
            Assert.AreEqual(0, data.AdvenceReceipt);     //預收款項
            //Assert.AreEqual(12800000, data.LongLiabilitiesWithinOneYear); // 一年內到期長期負債  // 不知如何計算此數值
            //Assert.AreEqual(394590603, data.OtherCurrentLiabilities);   // 其餘流動負債 // 不知如何計算此數值
            Assert.AreEqual(589468059, data.CurrentLiabilities); // 流動負債
            Assert.AreEqual(46475148, data.LongLiabilities);  // 長期負債
            Assert.AreEqual(30323958, data.OtherLiabilities);   // 其餘負債
            Assert.AreEqual(666267165, data.TotalLiability);  // 總負債
            Assert.AreEqual(1677028531, data.NetWorth);     // 淨值(權益總額)
            #endregion
        }
        [TestMethod]
        public void GetStockReportMonthlyNetProfitTaxed_10907()
        {
            TwseReportCollector collector = new TwseReportCollector
            {
                _logger = new UnitTestLogger()
            };
            var data = collector.GetStockReportMonthlyNetProfitTaxed("2330", 109, 7);
            Assert.IsNotNull(data);
            Assert.AreEqual("2330", data.StockNo);
            Assert.AreEqual(109, data.Year);
            Assert.AreEqual(7, data.Month);
            Assert.AreEqual(105963468, data.NetProfitTaxed);    // 本月
            Assert.AreEqual(84757724, data.LastYearNetProfitTaxed); // 去年同期
            Assert.AreEqual(21205744, data.Delta); // 增減金額
            Assert.AreEqual((decimal)(25.02 / 100), data.DeltaPercent); // 增減百分比
            Assert.AreEqual(727259018, data.ThisYearTillThisMonth); // 本年累計
            Assert.AreEqual(544460668, data.LastYearTillThisMonth); // 去年累計
            Assert.AreEqual(182798350, data.TillThisMonthDelta);    // 增減金額
            Assert.AreEqual((decimal)(33.57 / 100), data.TillThisMonthDeltaPercent);    // 增減百分比
            Assert.AreEqual(string.Empty, data.Remark);   // 備註/營收變化原因說明
        }
    }
}