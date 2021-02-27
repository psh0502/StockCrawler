using Common.Logging;
using Quartz;
using StockCrawler.Dao;
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
        private static readonly string[] _CompanyStock = GetRealCompanyStock();
        internal static ILog Logger { get; set; } = LogManager.GetLogger(typeof(StockFinReportUpdateJob));

        #region IJob Members
        /// <summary>
        /// 抓取簡易財務報表
        /// </summary>
        public Task Execute(IJobExecutionContext context)
        {
            Logger.InfoFormat("Invoke [{0}]...", MethodBase.GetCurrentMethod().Name);
            try
            {
                var collector = CollectorServiceProvider.GetStockReportCollector();
                using (var db = GetDB())
                    foreach (var stockNo in _CompanyStock)
                    {
                        try
                        {
                            var reports = collector.GetStockFinancialReport(stockNo);
                            if (reports != null && reports.Any())
                            {
                                foreach (var info in reports)
                                    db.InsertOrUpdateStockFinancialReport(info);

                                Logger.InfoFormat("[{0}] get its financial report", stockNo);
                            }
                            else
                                Logger.InfoFormat("[{0}] has no financial report", stockNo);
                        }
                        catch (ApplicationException) { }
                        Thread.Sleep(_breakInternval);
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
    }
}