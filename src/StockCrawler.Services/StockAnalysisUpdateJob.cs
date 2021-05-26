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
                throw;
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
                // 今年
                var thisYearFinanaces = db.GetStockFinancialReport(1, stockNo, SystemTime.Today.GetTaiwanYear(), -1).FirstOrDefault();
                var thisYearDivis = db.GetStockInterestIssuedInfo(4, stockNo, SystemTime.Today.GetTaiwanYear(), -1);
                // 去年
                var lastYearFinanaces = db.GetStockFinancialReport(1, stockNo, SystemTime.Today.AddYears(-1).GetTaiwanYear(), 4).FirstOrDefault() ?? new GetStockFinancialReportResult();
                var lastYearDivis = db.GetStockInterestIssuedInfo(4, stockNo, SystemTime.Today.AddYears(-1).GetTaiwanYear(), -1);
                // 前年
                var beforeYearFinanaces = db.GetStockFinancialReport(1, stockNo, SystemTime.Today.AddYears(-2).GetTaiwanYear(), 4).FirstOrDefault() ?? new GetStockFinancialReportResult();
                var beforeLastYearDivis = db.GetStockInterestIssuedInfo(4, stockNo, SystemTime.Today.AddYears(-2).GetTaiwanYear(), -1);
                // 大前年
                var mostBeforeYearFinanaces = db.GetStockFinancialReport(1, stockNo, SystemTime.Today.AddYears(-3).GetTaiwanYear(), 4).FirstOrDefault() ?? new GetStockFinancialReportResult();
                var mostBeforeLastYearDivis = db.GetStockInterestIssuedInfo(4, stockNo, SystemTime.Today.AddYears(-3).GetTaiwanYear(), -1);

                if (null == thisYearFinanaces)   // 若今年 Q1 還未公布，以去年財報比較為準
                {
                    thisYearFinanaces = db.GetStockFinancialReport(1, stockNo, SystemTime.Today.AddYears(-1).GetTaiwanYear(), 4).FirstOrDefault() ?? new GetStockFinancialReportResult();
                    thisYearDivis = db.GetStockInterestIssuedInfo(4, stockNo, SystemTime.Today.AddYears(-1).GetTaiwanYear(), -1);
                    lastYearFinanaces = db.GetStockFinancialReport(1, stockNo, SystemTime.Today.AddYears(-2).GetTaiwanYear(), 4).FirstOrDefault() ?? new GetStockFinancialReportResult();
                    lastYearDivis = db.GetStockInterestIssuedInfo(4, stockNo, SystemTime.Today.AddYears(-2).GetTaiwanYear(), -1);
                    beforeYearFinanaces = db.GetStockFinancialReport(1, stockNo, SystemTime.Today.AddYears(-3).GetTaiwanYear(), 4).FirstOrDefault() ?? new GetStockFinancialReportResult();
                    beforeLastYearDivis = db.GetStockInterestIssuedInfo(4, stockNo, SystemTime.Today.AddYears(-3).GetTaiwanYear(), -1);
                    mostBeforeYearFinanaces = db.GetStockFinancialReport(1, stockNo, SystemTime.Today.AddYears(-4).GetTaiwanYear(), 4).FirstOrDefault() ?? new GetStockFinancialReportResult();
                    mostBeforeLastYearDivis = db.GetStockInterestIssuedInfo(4, stockNo, SystemTime.Today.AddYears(-4).GetTaiwanYear(), -1);
                }
                #region 獲利能力
                var thisYearEPS = thisYearFinanaces.EPS;
                var lastYearEPS = lastYearFinanaces.EPS;
                var beforeLastYearEPS = beforeYearFinanaces.EPS;
                // 賺的比去年多
                result.IsPromisingEPS = thisYearEPS > lastYearEPS;
                // 獲利持續成長
                result.IsGrowingUpEPS = thisYearEPS > lastYearEPS && lastYearEPS > beforeLastYearEPS;
                // 穩健獲利
                result.IsAlwaysIncomeEPS = thisYearEPS > 0 && lastYearEPS > 0 && beforeLastYearEPS > 0;
                #endregion

                #region 參考項目
                var thisYearRevenue = thisYearFinanaces.Revenue;
                var lastYearRevenue = lastYearFinanaces.Revenue;
                var beforeLastYearRevenue = beforeYearFinanaces.Revenue;
                var mostBeforeLastYearRevenue = mostBeforeYearFinanaces.Revenue;

                // 營收正成長
                result.IsGrowingUpRevenue = lastYearRevenue > beforeLastYearRevenue;

                var basic = db.GetStockBasicInfo(stockNo);
                if (basic != null)
                    // 公司市值(>30億）
                    result.IsStableTotalAmount = basic.MarketValue > 30 * 100000000M;

                var thisOtherCashflow = thisYearFinanaces.InvestmentCashflow + thisYearFinanaces.FinancingCashflow;
                var thisBusinessCashflow = thisYearFinanaces.BusinessCashflow;
                // 業外收入(<=30%)
                result.IsStableOutsideIncome = thisBusinessCashflow != 0 && (thisOtherCashflow / thisBusinessCashflow) <= 0.3M;
                #endregion

                #region 股利政策
                var thisDivi = thisYearDivis.Sum(d => d.ProfitCashIssued + d.ProfitStockIssued);
                var lastDivi = lastYearDivis.Sum(d => d.ProfitCashIssued + d.ProfitStockIssued);
                var beforeLastDivi = beforeLastYearDivis.Sum(d => d.ProfitCashIssued + d.ProfitStockIssued);
                var mostBeforeLastYearDivi = mostBeforeLastYearDivis.Sum(d => d.ProfitCashIssued + d.ProfitStockIssued);
                result.HasDivi = thisDivi > 0;
                result.StockCashDivi = thisDivi;
                // 連續配息
                result.IsAlwaysPayDivi = lastDivi > 0 && beforeLastDivi > 0 && mostBeforeLastYearDivi > 0;
                // 配息穩定性，配息的每年變動差異低於 10% 以內
                result.IsStableDivi =
                    (beforeLastDivi != 0 && (Math.Abs((lastDivi - beforeLastDivi) / beforeLastDivi)) < 0.1M)
                    && (mostBeforeLastYearDivi != 0 && (Math.Abs((beforeLastDivi - mostBeforeLastYearDivi) / mostBeforeLastYearDivi)) < 0.1M);
                // 連續三年填息(90天內)
                result.IsAlwaysRestoreDivi = false; // TODO: need help to do it
                #endregion

                // 平均配息 = 最今三年加總(配息 / EPS) / 3 年
                var averageDivi = 
                    (
                    (thisYearEPS == 0 ? 0 : (thisDivi / thisYearEPS))
                    + (lastYearEPS == 0 ? 0 : (lastDivi / lastYearEPS))
                    + (beforeLastYearEPS == 0 ? 0 : (beforeLastDivi / beforeLastYearEPS))
                    ) / 3;
                Logger.DebugFormat("thisYearEPS: {0}, lastYearEPS: {1}, beforeLastYearEPS: {2}", thisYearEPS, lastYearEPS, beforeLastYearEPS);
                // 預測配息 = 平均配息 * 目前已實現的 EPS
                var possibleDivi = averageDivi * thisYearEPS;
                Logger.DebugFormat("averageDivi({0}) * thisYearEPS({1}) = possibleDivi({2})", averageDivi, thisYearEPS, possibleDivi);
                if (possibleDivi > 0)
                {
                    // 5% 合理價
                    result.Price5 = possibleDivi / 0.05M;
                    // 6% 合理價
                    result.Price6 = possibleDivi / 0.06M;
                    // 7% 合理價
                    result.Price7 = possibleDivi / 0.07M;
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