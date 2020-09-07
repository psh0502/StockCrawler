using Common.Logging;
using Quartz;
using StockCrawler.Dao;
using StockCrawler.Services.StockDailyPrice;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace StockCrawler.Services
{
    public class StockPriceUpdateJob : JobBase, IJob
    {
        internal static ILog Logger { get; set; } = LogManager.GetLogger(typeof(StockPriceUpdateJob));

        public StockPriceUpdateJob()
            : base()
        {
            if (null == Logger)
                Logger = LogManager.GetLogger(typeof(StockPriceUpdateJob));
        }
        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            Logger.InfoFormat("Invoke [{0}]...", MethodBase.GetCurrentMethod().Name);
            try
            {
                var list = new List<GetStockHistoryResult>();
                using (var db = StockDataServiceProvider.GetServiceInstance())
                {
                    var collector = CollectorProviderService.GetStockDailyPriceCollector();
                    foreach (var d in db.GetStocks())
                    {
                        Logger.DebugFormat("Retrieving daily price of [{0}] stock.", d.StockNo);
                        StockDailyPriceInfo info = collector.GetStockDailyPriceInfo(d.StockNo);
                        if (null != info)
                        {
                            Logger.Debug(info);
                            db.UpdateStockName(info.StockNo, info.StockName);
                            if (info.Volume > 0)
                            {
                                GetStockHistoryResult dr = new GetStockHistoryResult
                                {
                                    StockNo = info.StockNo,
                                    StockDT = info.LastTradeDT.Date,
                                    OpenPrice = info.OpenPrice,
                                    HighPrice = info.HighPrice,
                                    LowPrice = info.LowPrice,
                                    ClosePrice = info.ClosePrice,
                                    Volume = info.Volume,
                                    AdjClosePrice = info.ClosePrice
                                };

                                list.Add(dr);

                                Logger.InfoFormat("Finish the {0} stock daily price retrieving task.", d.StockNo);
                            }
                            else
                            {
                                Logger.WarnFormat("The {0} stock has no volumn today, skip it.", d.StockNo);
                            }
                        }
                    }
                    db.UpdateStockPriceHistoryDataTable(list);
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