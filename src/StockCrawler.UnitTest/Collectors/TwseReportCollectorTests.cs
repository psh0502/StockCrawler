﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockCrawler.Services.StockFinanceReport;

namespace StockCrawler.UnitTest.Collectors
{
    [TestClass]
    public class TwseReportCollectorTests : UnitTestBase
    {
        [TestMethod]
        public void GetStockReportCashFlowTest_109Q1()
        {
            IStockReportCollector collector = new TwseReportCollector();
            TwseReportCollector._logger = new UnitTestLogger();
            var data = collector.GetStockReportCashFlow("2330", 109, 1);
            Assert.IsNotNull(data);
            Assert.AreEqual("2330", data.StockNo);
            Assert.AreEqual(109, data.Year);
            Assert.AreEqual(1, data.Season);
            Assert.AreEqual(67083741, data.Depreciation);
            Assert.AreEqual(1470736, data.AmortizationFee);
            Assert.AreEqual(203029442, data.BusinessCashflow);
            Assert.AreEqual(-188993268, data.InvestmentCashflow);
            Assert.AreEqual(-40757411, data.FinancingCashflow);
            //Assert.AreEqual(-192063846, data.CapitalExpenditures);
            Assert.AreEqual(14036174, data.FreeCashflow);
            Assert.AreEqual(-26721237, data.NetCashflow);
        }
        [TestMethod]
        public void GetStockReportCashFlowTest_108Q4()
        {
            IStockReportCollector collector = new TwseReportCollector();
            TwseReportCollector._logger = new UnitTestLogger();
            var data = collector.GetStockReportCashFlow("2330", 108, 4);
            Assert.IsNotNull(data);
            Assert.AreEqual("2330", data.StockNo);
            Assert.AreEqual(108, data.Year);
            Assert.AreEqual(4, data.Season);
            Assert.AreEqual(66137308, data.Depreciation);
            Assert.AreEqual(1394477, data.AmortizationFee);
            Assert.AreEqual(202954417, data.BusinessCashflow);
            Assert.AreEqual(-171605106, data.InvestmentCashflow);
            Assert.AreEqual(-17182776, data.FinancingCashflow);
            //Assert.AreEqual(-170009539, data.CapitalExpenditures);
            Assert.AreEqual(31349311, data.FreeCashflow);
            Assert.AreEqual(14166535, data.NetCashflow);
        }
        [TestMethod]
        public void GetStockReportIncomeTest_109Q1()
        {
            IStockReportCollector collector = new TwseReportCollector();
            TwseReportCollector._logger = new UnitTestLogger();
            var data = collector.GetStockReportIncome("2330", 109, 1);
            Assert.IsNotNull(data);
            Assert.AreEqual("2330", data.StockNo);
            Assert.AreEqual(109, data.Year);
            Assert.AreEqual(1, data.Season);
            Assert.AreEqual(310597183, data.Revenue);
            Assert.AreEqual(160784181, data.GrossProfit);
            Assert.AreEqual(1451102, data.SalesExpense);
            Assert.AreEqual(5903061, data.ManagementCost);
            Assert.AreEqual(24968883, data.RDExpense);
            Assert.AreEqual(32323046, data.OperatingExpenses);
            Assert.AreEqual(128521637, data.BusinessInterest);
            Assert.AreEqual(132147178, data.NetProfitTaxFree);
            Assert.AreEqual(117062893, data.NetProfitTaxed);
        }
        [TestMethod]
        public void GetStockReportBalanceTest_109Q1()
        {
            IStockReportCollector collector = new TwseReportCollector();
            TwseReportCollector._logger = new UnitTestLogger();
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
            //Assert.AreEqual(145740092, data.OtherCurrentAssets);    // 其餘流動資產
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
            //Assert.AreEqual(12800000, data.LongLiabilitiesWithinOneYear); // 一年內到期長期負債
            //Assert.AreEqual(394590603, data.OtherCurrentLiabilities);   // 其餘流動負債
            Assert.AreEqual(589468059, data.CurrentLiabilities); // 流動負債
            Assert.AreEqual(46475148, data.LongLiabilities);  // 長期負債
            Assert.AreEqual(30323958, data.OtherLiabilities);   // 其餘負債
            Assert.AreEqual(666267165, data.TotalLiability);  // 總負債
            Assert.AreEqual(1677028531, data.NetWorth);     // 淨值(權益總額)
            #endregion
        }
    }
}