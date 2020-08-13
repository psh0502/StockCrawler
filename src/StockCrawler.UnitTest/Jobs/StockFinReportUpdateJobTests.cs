using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz;
using StockCrawler.Dao;
using StockCrawler.Services;
using System;
using System.Linq;

namespace StockCrawler.UnitTest.Jobs
{
    [TestClass()]
    public class StockFinReportUpdateJobTests : UnitTestBase
    {
        [TestMethod()]
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
                {
                    var data = db.GetStockReportCashFlow("2330", (short)(Services.SystemTime.Today.Year-1911), 1).ToList();
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
            }
        }
    }
}