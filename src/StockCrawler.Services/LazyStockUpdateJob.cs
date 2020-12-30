using Common.Logging;
using Quartz;
using StockCrawler.Dao;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace StockCrawler.Services
{
    public class LazyStockUpdateJob : JobBase, IJob
    {
        internal static ILog Logger { get; set; } = LogManager.GetLogger(typeof(LazyStockUpdateJob));

        public Task Execute(IJobExecutionContext context)
        {
            Logger.InfoFormat("Invoke [{0}]...", MethodBase.GetCurrentMethod().Name);
            try
            {
                var collector = CollectorProviderService.GetLazyStockCollector();
                using (var db = StockDataServiceProvider.GetServiceInstance())
                {
                    foreach (var d in db.GetStocks())
                    {
                        var data = collector.GetData(d.StockNo);
                        if (null != data) db.InsertOrUpdateLazyStock(data.ToDbObject());
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
    }
}