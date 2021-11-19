using Common.Logging;
using Quartz;
using StockCrawler.Dao;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace StockCrawler.Services
{
    public class StockMonthlyIncomeUpdateJob : JobBase, IJob
    {
        internal static ILog Logger { get; set; } = LogManager.GetLogger(typeof(StockMonthlyIncomeUpdateJob));

        #region IJob Members

        public Task Execute(IJobExecutionContext context)
        {
            Logger.InfoFormat("Invoke [{0}]...", MethodBase.GetCurrentMethod().Name);
            try
            {
                var targetDate = SystemTime.Today;
                string stockNo = null;
                if (context != null)
                {
                    var args = ((string[])context.Get("args")) ?? new string[] { };
                    if (args.Length > 0) targetDate = DateTime.Parse(args[0]);
                    if (args.Length > 1) stockNo = args[1];
                }
                if (!targetDate.IsWeekend())
                {
                    var collector = CollectorServiceProvider.GetStockMonthlyIncomeCollector();
                    foreach (var stock in StockHelper.GetCompanyStockList())
                    {
                        var priceInfo = collector.GetStockMonthlyIncome(stock.StockNo);
                        if (null != priceInfo)
                            using (var db = GetDB())
                            {
                                db.InsertOrUpdateStockMonthlyIncome(priceInfo.ToArray());
                                Logger.Info($"[{stock.StockNo}] has been updated.");
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Job executing failed!", ex);
                throw;
            }
            return null;
        }
        #endregion
    }
}