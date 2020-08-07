using Common.Logging;
using Quartz;
using StockCrawler.Dao;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace StockCrawler.Services
{
    public class StockBasicInfoUpdateJob : JobBase, IJob
    {
        internal static ILog Logger { get; set; } = LogManager.GetLogger(typeof(StockPriceUpdateJob));

        public StockBasicInfoUpdateJob()
            : base()
        {
            if (null == Logger)
                Logger = LogManager.GetLogger(typeof(StockBasicInfoUpdateJob));
        }

        public StockBasicInfoUpdateJob(string collectorTypeName)
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
                using (var db = StockDataServiceProvider.GetServiceInstance())
                {
                    var collector = string.IsNullOrEmpty(CollectorTypeName) ? CollectorProviderService.GetBasicInfoCollector() : CollectorProviderService.GetBasicInfoCollector(CollectorTypeName);
                    List<GetStockBasicInfoResult> list = new List<GetStockBasicInfoResult>();
                    foreach (var d in db.GetStocks())
                    {
                        var info = collector.GetStockBasicInfo(d.StockNo);
                        if (null != info)
                            list.Add(info);
                        else
                            Logger.InfoFormat("[{0}] has no basic info", d.StockNo);
                    }

                    db.UpdateStockBasicInfo(list);
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