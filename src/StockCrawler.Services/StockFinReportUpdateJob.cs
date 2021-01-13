using Common.Logging;
using Quartz;
using StockCrawler.Dao;
using StockCrawler.Services.Collectors;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace StockCrawler.Services
{
    public class StockFinReportUpdateJob : JobBase, IJob
    {
#if (DEBUG)
        private const int beginYear = 107;
#else
        private const int beginYear = 104;
#endif
        internal static ILog Logger { get; set; } = LogManager.GetLogger(typeof(StockFinReportUpdateJob));

        #region IJob Members
        public Task Execute(IJobExecutionContext context)
        {
            Logger.InfoFormat("Invoke [{0}]...", MethodBase.GetCurrentMethod().Name);
            try
            {
                using (var db = GetDB())
                {
                    var collector = CollectorServiceProvider.GetStockReportCollector();
                    GetIncomeIntoDatabase(db, collector);
                    GetCashflowIntoDatabase(db, collector);
                    GetBalanceIntoDatabase(db, collector);
                    GetMonthlyNetProfitTaxedIntoDatabase(db, collector);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Job executing failed!", ex);
            }
            return null;
        }
        #endregion

        /// <summary>
        /// 取得股票清單，排除非公司的基金型股票
        /// </summary>
        /// <returns>股票代碼清單</returns>
        private static string[] GetRealCompanyStock()
        {
            return StockHelper.GetCompanyStockList().Select(d => d.StockNo).ToArray();
        }
        private static void GetMonthlyNetProfitTaxedIntoDatabase(IRepository db, IStockReportCollector collector)
        {
            try
            {
                foreach (var stockNo in GetRealCompanyStock())
                {
                    var last = db.GetStockReportMonthlyNetProfitTaxed(1, stockNo, -1, -1).FirstOrDefault();
                    DateTime bgnDate;
                    if (null == last)
                        bgnDate = new DateTime(beginYear + 1911, 1, 1);
                    else
                        bgnDate = new DateTime(last.Year + 1911, last.Month, 1);

                    var deadline = SystemTime.Today.AddDays(-SystemTime.Today.Day);
                    for (; bgnDate < deadline; bgnDate = bgnDate.AddMonths(1))
                    {
                        var info = collector.GetStockReportMonthlyNetProfitTaxed(stockNo, bgnDate.GetTaiwanYear(), (short)bgnDate.Month);
                        if (null != info)
                        {
                            db.InsertOrUpdateStockMonthlyNetProfitTaxedReport(info);
                            Logger.InfoFormat("[{0}] get its monthly net profit report(year={1}/month={2})", stockNo, bgnDate.GetTaiwanYear(), bgnDate.Month);
                        }
                        else
                            Logger.InfoFormat("[{0}] has no monthly net profit report(year={1}/month={2})", stockNo, bgnDate.GetTaiwanYear(), bgnDate.Month);

                        Thread.Sleep(_breakInternval);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Warn(string.Format("[{0}] has error: {1}", MethodBase.GetCurrentMethod().Name, ex.Message), ex);
            }
        }
        private static void GetIncomeIntoDatabase(IRepository db, IStockReportCollector collector)
        {
            try
            {
                foreach (var stockNo in GetRealCompanyStock())
                {
                    var last = db.GetStockReportIncome(1, stockNo, -1, -1).FirstOrDefault();
                    DateTime bgnDate;
                    if (null == last)
                        bgnDate = new DateTime(beginYear + 1911, 1, 1);
                    else
                        bgnDate = new DateTime(last.Year + 1911, 1, 1).AddSeason(last.Season);

                    var deadline = SystemTime.Today.AddSeason(0);
                    for (; bgnDate < deadline; bgnDate = bgnDate.AddSeason(1))
                    {
                        var info = collector.GetStockReportIncome(stockNo, bgnDate.GetTaiwanYear(), bgnDate.GetSeason());
                        if (null != info)
                        {
                            db.InsertOrUpdateStockIncomeReport(info);
                            Logger.InfoFormat("[{0}] get its income report(year={1}/season={2})", stockNo, bgnDate.GetTaiwanYear(), bgnDate.GetSeason());
                        }
                        else
                            Logger.InfoFormat("[{0}] has no income report(year={1}/season={2})", stockNo, bgnDate.GetTaiwanYear(), bgnDate.GetSeason());

                        Thread.Sleep(_breakInternval);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Warn(string.Format("[{0}] has error: {1}", MethodBase.GetCurrentMethod().Name, ex.Message), ex);
            }
        }
        /// <summary>
        /// 抓取現金流量表
        /// </summary>
        /// <param name="db">DAO 物件</param>
        /// <param name="collector">對應資料源的收集器</param>
        /// <param name="stockNo">股票代碼</param>
        /// <param name="year">民國年</param>
        /// <param name="season">第幾季</param>
        /// <exception cref="ApplicationException">該公司股票不繼續公開發行</exception>
        /// <returns>成功失敗</returns>
        private static void GetCashflowIntoDatabase(IRepository db, IStockReportCollector collector)
        {
            try
            {
                foreach (var stockNo in GetRealCompanyStock())
                {
                    var last = db.GetStockReportCashFlow(1, stockNo, -1, -1).FirstOrDefault();
                    DateTime bgnDate;
                    if (null == last)
                        bgnDate = new DateTime(beginYear + 1911, 1, 1);
                    else
                        bgnDate = new DateTime(last.Year + 1911, 1, 1).AddSeason(last.Season);

                    var deadline = SystemTime.Today.AddSeason(0);
                    for (; bgnDate < deadline; bgnDate = bgnDate.AddSeason(1))
                    {
                        var info = collector.GetStockReportCashFlow(stockNo, bgnDate.GetTaiwanYear(), bgnDate.GetSeason());
                        if (null != info)
                        {
                            db.InsertOrUpdateStockCashflowReport(info);
                            Logger.InfoFormat("[{0}] get its cashflow report(year={1}/season={2})", stockNo, bgnDate.GetTaiwanYear(), bgnDate.GetSeason());
                        }
                        else
                            Logger.InfoFormat("[{0}] has no cashflow report(year={1}/season={2})", stockNo, bgnDate.GetTaiwanYear(), bgnDate.GetSeason());

                        Thread.Sleep(_breakInternval);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Warn(string.Format("[{0}] has error: {1}", MethodBase.GetCurrentMethod().Name, ex.Message), ex);
            }
        }
        private static void GetBalanceIntoDatabase(IRepository db, IStockReportCollector collector)
        {
            try
            {
                foreach (var stockNo in GetRealCompanyStock())
                {
                    var last = db.GetStockReportBalance(1, stockNo, -1, -1).FirstOrDefault();
                    DateTime bgnDate;
                    if (null == last)
                        bgnDate = new DateTime(beginYear + 1911, 1, 1);
                    else
                        bgnDate = new DateTime(last.Year + 1911, 1, 1).AddSeason(last.Season);

                    var deadline = SystemTime.Today.AddSeason(0);
                    for (; bgnDate < deadline; bgnDate = bgnDate.AddSeason(1))
                    {
                        var info = collector.GetStockReportBalance(stockNo, bgnDate.GetTaiwanYear(), bgnDate.GetSeason());
                        if (null != info)
                        {
                            var basicInfo = db.GetStockBasicInfo(stockNo);
                            if (null != basicInfo)
                                info.NAV = info.NetWorth * 1000 / basicInfo.ReleaseStockCount;

                            db.InsertOrUpdateStockBalanceReport(info);
                            Logger.InfoFormat("[{0}] get its balance report(year={1}/season={2})", stockNo, bgnDate.GetTaiwanYear(), bgnDate.GetSeason());

                            Thread.Sleep(_breakInternval);
                        }
                        else
                            Logger.InfoFormat("[{0}] has no balance report(year={1}/season={2})", stockNo, bgnDate.GetTaiwanYear(), bgnDate.GetSeason());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Warn(string.Format("[{0}] has error: {1}", MethodBase.GetCurrentMethod().Name, ex.Message), ex);
            }
        }
    }
}