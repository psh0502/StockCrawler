using Common.Logging;
using Quartz;
using StockCrawler.Dao;
using StockCrawler.Dao.Schema;
using StockCrawler.Services.StockDailyPrice;
using System;
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

        public StockPriceUpdateJob(string collectorTypeName)
            : this()
        {
            CollectorTypeName = collectorTypeName;
        }

        public string CollectorTypeName { get; private set; }

        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            Logger.InfoFormat("Invoke [{0}]...", MethodBase.GetCurrentMethod().Name);
            try
            {
                StockDataSet.StockPriceHistoryDataTable dt = new StockDataSet.StockPriceHistoryDataTable();
                using (var db = StockDataServiceProvider.GetServiceInstance())
                {
                    var collector = string.IsNullOrEmpty(CollectorTypeName) ? CollectorProviderService.GetDailyPriceCollector() : CollectorProviderService.GetDailyPriceCollector(CollectorTypeName);
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
                                StockDataSet.StockPriceHistoryRow dr = dt.NewStockPriceHistoryRow();
                                dr.StockNo = info.StockNo;
                                dr.StockDT = info.LastTradeDT.Date;
                                dr.OpenPrice = info.OpenPrice;
                                dr.HighPrice = info.HighPrice;
                                dr.LowPrice = info.LowPrice;
                                dr.ClosePrice = info.ClosePrice;
                                dr.Volume = info.Volume;
                                dr.AdjClosePrice = info.ClosePrice;

                                dt.AddStockPriceHistoryRow(dr);

                                Logger.InfoFormat("Finish the {0} stock daily price retrieving task.", d.StockNo);
                            }
                            else
                            {
                                Logger.WarnFormat("The {0} stock has no volumn today, skip it.", d.StockNo);
                            }
                        }
                    }
                    db.UpdateStockPriceHistoryDataTable(dt);
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