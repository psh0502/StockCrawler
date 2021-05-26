using Common.Logging;
using Quartz;
using StockCrawler.Dao;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace StockCrawler.Services
{
    public class StockAnalysisUpdateJob : JobBase, IJob
    {
        internal static ILog Logger { get; set; } = LogManager.GetLogger(typeof(StockAnalysisUpdateJob));

        #region IJob Members
        /// <summary>
        /// 計算每支股票的綜合分析結果
        /// </summary>
        public Task Execute(IJobExecutionContext context)
        {
            Logger.InfoFormat("Invoke [{0}]...", MethodBase.GetCurrentMethod().Name);
            try
            {
                foreach (var d in StockHelper.GetCompanyStockList()
                    .Where(d => !d.StockName.Contains("DR"))) // 排除憑證類
                {
                    var data = StockAnalyze(d.StockNo);
                    if (null != data)
                        using (var db = GetDB())
                            db.InsertOrUpdateStockAnalysis(data);

                    Logger.InfoFormat("[{0}] analysis done!", d.StockNo);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Job executing failed!", ex);
            }
            return null;
        }

        private GetStockAnalysisDataResult StockAnalyze(string stockNo)
        {
            var result = new GetStockAnalysisDataResult()
            {
                StockNo = stockNo,
                DiviRatio = string.Empty,
                DiviType = string.Empty,
                IsRealMode = true,
                Price = string.Empty,
                LastModifiedAt = SystemTime.Now
            };
            using (var db = GetDB())
            {
                // 近年
                var thisYearFinanaces = db.GetStockFinancialReport(4, stockNo, SystemTime.Today.AddYears(-1).GetTaiwanYear(), -1);
                // 去年
                var lastYearFinanaces = db.GetStockFinancialReport(4, stockNo, SystemTime.Today.AddYears(-2).GetTaiwanYear(), -1);
                // 前年
                var beforeYearFinanaces = db.GetStockFinancialReport(4, stockNo, SystemTime.Today.AddYears(-3).GetTaiwanYear(), -1);

                #region 獲利能力
                var thisYearEPS = thisYearFinanaces.Sum(d => d.EPS);
                var lastYearEPS = lastYearFinanaces.Sum(d => d.EPS);
                var beforeLastYearEPS = beforeYearFinanaces.Sum(d => d.EPS);
                // 賺的比去年多
                result.IsPromisingEPS = thisYearEPS > lastYearEPS;
                // 獲利持續成長
                result.IsGrowingUpEPS = thisYearEPS > lastYearEPS && lastYearEPS > beforeLastYearEPS;
                // 穩健獲利
                result.IsAlwaysIncomeEPS = thisYearEPS > 0 && lastYearEPS > 0 && beforeLastYearEPS > 0;
                #endregion

                #region 參考項目
                var thisYearRevenue = thisYearFinanaces.Sum(d => d.Revenue);
                var lastYearRevenue = lastYearFinanaces.Sum(d => d.Revenue);
                var beforeLastYearRevenue = beforeYearFinanaces.Sum(d => d.Revenue);

                // 營收正成長
                result.IsGrowingUpRevenue = thisYearRevenue > lastYearRevenue && lastYearRevenue > beforeLastYearRevenue;

                var basic = db.GetStockBasicInfo(stockNo);
                // 公司市值(>30億）
                result.IsStableTotalAmount = basic.MarketValue > 30 * 100000000M;

                var thisOtherCashflow = thisYearFinanaces.Sum(d => d.InvestmentCashflow + d.FinancingCashflow);
                var thisBusinessCashflow = thisYearFinanaces.Sum(d => d.BusinessCashflow);
                // 業外收入(<=30%)
                result.IsStableOutsideIncome = thisBusinessCashflow != 0 && (thisOtherCashflow / thisBusinessCashflow) <= 0.3M;
                #endregion

                #region 股利政策
                var thisYearDivis = db.GetStockInterestIssuedInfo(4, stockNo, SystemTime.Today.AddYears(-1).GetTaiwanYear(), -1);
                var lastYearDivis = db.GetStockInterestIssuedInfo(4, stockNo, SystemTime.Today.AddYears(-2).GetTaiwanYear(), -1);
                var beforeLastYearDivis = db.GetStockInterestIssuedInfo(4, stockNo, SystemTime.Today.AddYears(-3).GetTaiwanYear(), -1);

                var thisDivi = thisYearDivis.Sum(d => d.ProfitCashIssued + d.SsrCashIssued + d.CapitalReserveCashIssued);
                var lastDivi = lastYearDivis.Sum(d => d.ProfitCashIssued + d.SsrCashIssued + d.CapitalReserveCashIssued);
                var beforeLastDivi = beforeLastYearDivis.Sum(d => d.ProfitCashIssued + d.SsrCashIssued + d.CapitalReserveCashIssued);
                result.HasDivi = thisDivi > 0;
                result.StockCashDivi = thisDivi;
                // 連續配息
                result.IsAlwaysPayDivi = thisDivi > 0 && lastDivi > 0 && beforeLastDivi > 0;
                // 配息穩定性，配息的每年變動差異低於 10% 以內
                result.IsStableDivi = 
                    (lastDivi != 0 && (Math.Abs((thisDivi - lastDivi) / lastDivi)) < 0.1M)
                    && (beforeLastDivi != 0 && (Math.Abs((lastDivi - beforeLastDivi) / beforeLastDivi)) < 0.1M);
                // 連續三年填息(90天內)
                result.IsAlwaysRestoreDivi = false; // TODO: need help to do it
                #endregion

                if (thisDivi > 0)
                {
                    // 5% 合理價
                    result.Price5 = thisDivi / 0.05M;
                    // 6% 合理價
                    result.Price6 = thisDivi / 0.06M;
                    // 7% 合理價
                    result.Price7 = thisDivi / 0.07M;
                }
                // 現價
                var latestPrice = db.GetStockPeriodPrice(stockNo, 1, SystemTime.Today.AddDays(-14), SystemTime.Today.AddDays(1)).First();
                result.CurrPrice = latestPrice.ClosePrice;
            }
            return result;
        }
        #endregion
    }
}