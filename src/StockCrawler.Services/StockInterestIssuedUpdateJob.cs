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
    public class StockInterestIssuedUpdateJob : JobBase, IJob
    {
        internal static ILog Logger { get; set; } = LogManager.GetLogger(typeof(StockInterestIssuedUpdateJob));

        #region IJob Members
        /// <summary>
        /// 抓取股票除權息資訊
        /// </summary>
        public Task Execute(IJobExecutionContext context)
        {
            Logger.InfoFormat("Invoke [{0}]...", MethodBase.GetCurrentMethod().Name);
            try
            {
                var collector = CollectorServiceProvider.GetStockInterestIssuedCollector();
                using (var db = GetDB())
                    foreach (var stock in StockHelper.GetCompanyStockList())
                    {
                        try
                        {
                            var data = collector.GetStockInterestIssuedInfo(stock.StockNo);
                            if (data != null && data.Any())
                            {
                                foreach (var info in data)
                                    db.InsertOrUpdateStockInterestIssuedInfo(info);

                                Logger.InfoFormat("[{0}] get its interest issued info", stock.StockNo);
                            }
                            else
                                Logger.InfoFormat("[{0}] has no interest issued info", stock.StockNo);
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