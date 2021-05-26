using Common.Logging;
using Quartz;
using StockCrawler.Dao;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace StockCrawler.Services
{
    public class StockFinReportUpdateJob : JobBase, IJob
    {
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
                    foreach (var stock in StockHelper.GetCompanyStockList())
                    {
                        try
                        {
                            var reports = collector.GetStockFinancialReport(stock.StockNo);
                            if (reports != null && reports.Any())
                            {
                                foreach (var info in reports)
                                    db.InsertOrUpdateStockFinancialReport(info);

                                Logger.InfoFormat("[{0}] get its financial report", stock.StockNo);
                            }
                            else
                                Logger.InfoFormat("[{0}] has no financial report", stock.StockNo);
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
    }
}