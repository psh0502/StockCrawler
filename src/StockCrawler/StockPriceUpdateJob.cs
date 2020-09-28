using Common.Logging;
using Quartz;
using StockCrawler.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StockCrawler.Services
{
    public class StockPriceUpdateJob : JobBase, IJob
    {
        internal static ILog Logger { get; set; } = LogManager.GetLogger(typeof(StockPriceUpdateJob));

        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            Logger.InfoFormat("Invoke [{0}]...", MethodBase.GetCurrentMethod().Name);
            try
            {
                var list = new List<GetStockPeriodPriceResult>();
                using (var db = StockDataServiceProvider.GetServiceInstance())
                {
                    var collector = CollectorProviderService.GetStockDailyPriceCollector();
                    foreach (var d in db.GetStocks())
                    {
                        Logger.DebugFormat("Retrieving daily price of [{0}] stock.", d.StockNo);
                        var info = collector.GetStockDailyPriceInfo(d.StockNo);
                        if (null != info)
                        {
                            db.UpdateStockName(info.StockNo, info.StockName);
                            if (info.Volume > 0)
                            {
                                list.Add(info);
                                Logger.InfoFormat("Finish the {0} stock daily price retrieving task.", d.StockNo);
                            }
                            else
                            {
                                Logger.WarnFormat("The {0} stock has no volumn today, skip it.", d.StockNo);
                            }
                        }
                    }
                    if (list.Any())
                        // 寫入日價
                        db.InsertOrUpdateStockPrice(list);

                    Tools.CalculateMAAndPeriodK(SystemTime.Today);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Job executing failed!", ex);
                throw;
            }
        }
        #endregion
    }
}