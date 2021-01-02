using Common.Logging;
using Quartz;
using StockCrawler.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace StockCrawler.Services
{
    public class StockPriceUpdateJob : JobBase, IJob
    {
        internal static ILog Logger { get; set; } = LogManager.GetLogger(typeof(StockPriceUpdateJob));

        #region IJob Members

        public Task Execute(IJobExecutionContext context)
        {
            Logger.InfoFormat("Invoke [{0}]...", MethodBase.GetCurrentMethod().Name);
            try
            {
                var collector = CollectorServiceProvider.GetStockDailyPriceCollector();

                var list = new List<GetStockPeriodPriceResult>();
                using (var db = GetDB())
                {
                    var stocks = collector.GetStockDailyPriceInfo()
                        .Select(d => new GetStocksResult()
                        {
                            StockNo = d.StockNo,
                            StockName = d.StockName,
                            Enable = true
                        }).ToList();

                    if (stocks.Any())
                        db.InsertOrUpdateStock(stocks.ToArray());

                    foreach (var d in collector.GetStockDailyPriceInfo())
                    {
                        Logger.DebugFormat("Retrieving daily price of [{0}] stock.", d.StockNo);
                        var info = collector.GetStockDailyPriceInfo(d.StockNo);
                        if (null != info)
                        {
                            db.InsertOrUpdateStock(info.StockNo, info.StockName, null);
                            list.Add(info);
                            Logger.InfoFormat("Finish the {0} stock daily price retrieving task.", d.StockNo);
                        }
                    }
                    if (list.Any())
                        // 寫入日價
                        db.InsertOrUpdateStockPrice(list.ToArray());
                }
                Tools.CalculateMAAndPeriodK(SystemTime.Today);
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