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
                var collector = CollectorServiceProvider.GetStockReportCollector();
                using (var db = GetDB())
                    GetFinancialReportIntoDatabase(db, collector);
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
        /// <summary>
        /// 抓取簡易財務報表
        /// </summary>
        /// <param name="db">DAO 物件</param>
        /// <param name="collector">對應資料源的收集器</param>
        /// <exception cref="ApplicationException">該公司股票不繼續公開發行</exception>
        private static void GetFinancialReportIntoDatabase(IRepository db, IStockReportCollector collector)
        {
            try
            {
                foreach (var stockNo in GetRealCompanyStock())
                {
                    var last = db.GetStockFinancialReport(1, stockNo, -1, -1).FirstOrDefault();
                    DateTime bgnDate;
                    if (null == last)
                        bgnDate = new DateTime(beginYear + 1911, 1, 1);
                    else
                        bgnDate = new DateTime(last.Year + 1911, 1, 1).AddSeason(last.Season);

                    var deadline = SystemTime.Today.AddSeason(0);
                    for (; bgnDate < deadline; bgnDate = bgnDate.AddSeason(1))
                    {
                        var info = collector.GetStockFinancialReport(stockNo, bgnDate.GetTaiwanYear(), bgnDate.GetSeason());
                        if (null != info)
                        {
                            db.InsertOrUpdateStockFinancialReport(info);
                            Logger.InfoFormat("[{0}] get its financial report(year={1}/season={2})", stockNo, bgnDate.GetTaiwanYear(), bgnDate.GetSeason());
                        }
                        else
                            Logger.InfoFormat("[{0}] has no financial report(year={1}/season={2})", stockNo, bgnDate.GetTaiwanYear(), bgnDate.GetSeason());

                        Thread.Sleep(_breakInternval);
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