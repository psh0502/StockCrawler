using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz;
using StockCrawler.Dao;
using StockCrawler.Services;
using System;
using System.Linq;

namespace StockCrawler.UnitTest.Jobs
{
    [TestClass]
    public class StockFinReportUpdateJobTests : UnitTestBase
    {
        [TestMethod]
        public void ExecuteTest()
        {
            Services.SystemTime.SetFakeTime(new DateTime(2020, 4, 6));
            StockFinReportUpdateJob.Logger = new UnitTestLogger();
            StockFinReportUpdateJob target = new StockFinReportUpdateJob();
            IJobExecutionContext context = null;
            target.Execute(context);
            using (var db = new StockDataContext(ConnectionStringHelper.StockConnectionString))
            {
                {
                    var data = db.GetStocks().ToList();
                    Assert.AreEqual(1, data.Count);
                    Assert.AreEqual("2330", data.First().StockNo);
                }
                #region cashflow
                {
                    var data = db.GetStockReportCashFlow("2330", (short)(Services.SystemTime.Today.Year - 1911), 1).ToList();
                    Assert.AreEqual(1, data.Count);
                    var d1 = data.First();
                    Assert.AreEqual("2330", d1.StockNo);
                    Assert.AreEqual(109, d1.Year);
                    Assert.AreEqual(1, d1.Season);
                    Assert.AreEqual(67083741, d1.Depreciation);
                    Assert.AreEqual(1470736, d1.AmortizationFee);
                    Assert.AreEqual(203029442, d1.BusinessCashflow);
                    Assert.AreEqual(-188993268, d1.InvestmentCashflow);
                    Assert.AreEqual(-40757411, d1.FinancingCashflow);
                    //Assert.AreEqual(-192063846, d1.CapitalExpenditures);
                    Assert.AreEqual(14036174, d1.FreeCashflow);
                    Assert.AreEqual(-26721237, d1.NetCashflow);
                }
                #endregion

                #region Income
                {
                    var data = db.GetStockReportIncome("2330", (short)(Services.SystemTime.Today.Year - 1911), 1).ToList();
                    Assert.AreEqual(1, data.Count);
                    var d1 = data.First();
                    Assert.AreEqual("2330", d1.StockNo);
                    Assert.AreEqual(109, d1.Year);
                    Assert.AreEqual(1, d1.Season);
                    Assert.AreEqual(310597183, d1.Revenue);
                    Assert.AreEqual(160784181, d1.GrossProfit);
                    Assert.AreEqual(1451102, d1.SalesExpense);
                    Assert.AreEqual(5903061, d1.ManagementCost);
                    Assert.AreEqual(24968883, d1.RDExpense);
                    Assert.AreEqual(32323046, d1.OperatingExpenses);
                    Assert.AreEqual(128521637, d1.BusinessInterest);
                    Assert.AreEqual(132147178, d1.NetProfitTaxFree);
                    Assert.AreEqual(117062893, d1.NetProfitTaxed);
                }
                #endregion

                #region balance
                {
                    var data = db.GetStockReportBalance("2330", (short)(Services.SystemTime.Today.Year - 1911), 1).ToList();
                    Assert.AreEqual(1, data.Count);
                    var d1 = data.First();
                    Assert.AreEqual("2330", d1.StockNo);
                    Assert.AreEqual(109, d1.Year);
                    Assert.AreEqual(1, d1.Season);

                    #region 資產
                    Assert.AreEqual(430777229, d1.CashAndEquivalents);    // 現金及約當現金
                    Assert.AreEqual(1254253, d1.ShortInvestments);    // 短期投資
                    Assert.AreEqual(146420632, d1.BillsReceivable);   // 應收帳款及票據
                    Assert.AreEqual(78277834, d1.Stock);  // 存貨
                    //Assert.AreEqual(145740092, data.OtherCurrentAssets);    // 其餘流動資產
                    Assert.AreEqual(802470040, d1.CurrentAssets);     // 流動資產
                    Assert.AreEqual(19381760, d1.LongInvestment);     // 長期投資
                    Assert.AreEqual(1438215285, d1.FixedAssets);      // 固定資產
                    Assert.AreEqual(83228611, d1.OtherAssets);        // 其餘資產
                    Assert.AreEqual(2343295696, d1.TotalAssets);      // 總資產
                    #endregion

                    #region 負債
                    Assert.AreEqual(139310384, d1.ShortLoan); // 短期借款
                    Assert.AreEqual(2992858, d1.ShortBillsPayable);   // 應付短期票券
                    Assert.AreEqual(39774214, d1.AccountsAndBillsPayable); //應付帳款及票據
                    Assert.AreEqual(0, d1.AdvenceReceipt);     //預收款項
                    //Assert.AreEqual(12800000, data.LongLiabilitiesWithinOneYear); // 一年內到期長期負債
                    //Assert.AreEqual(394590603, data.OtherCurrentLiabilities);   // 其餘流動負債
                    Assert.AreEqual(589468059, d1.CurrentLiabilities); // 流動負債
                    Assert.AreEqual(46475148, d1.LongLiabilities);  // 長期負債
                    Assert.AreEqual(30323958, d1.OtherLiabilities);   // 其餘負債
                    Assert.AreEqual(666267165, d1.TotalLiability);  // 總負債
                    Assert.AreEqual(1677028531, d1.NetWorth);     // 淨值(權益總額)
                    #endregion

                }
                #endregion

                #region Monthly net profit taxed
                {
                    var data = db.GetStockReportMonthlyNetProfitTaxed("2330", (short)(Services.SystemTime.Today.Year - 1911), 3).ToList();
                    Assert.AreEqual(1, data.Count);
                    var d1 = data.First();
                    Assert.AreEqual("2330", d1.StockNo);
                    Assert.AreEqual(109, d1.Year);
                    Assert.AreEqual(3, d1.Month);
                    Assert.AreEqual(113519599, d1.NetProfitTaxed);
                    Assert.AreEqual(79721587, d1.LastYearNetProfitTaxed);
                    Assert.AreEqual(33798012, d1.Delta);
                    Assert.AreEqual((decimal)(42.40 / 100), d1.DeltaPercent);
                    Assert.AreEqual(310597183, d1.ThisYearTillThisMonth);
                    Assert.AreEqual(218704469, d1.LastYearTillThisMonth);
                    Assert.AreEqual(91892714, d1.TillThisMonthDelta);
                    Assert.AreEqual((decimal)(42.02 / 100), d1.TillThisMonthDeltaPercent);
                    Assert.AreEqual(string.Empty, d1.Remark);
                }
                #endregion

            }
        }
    }
}