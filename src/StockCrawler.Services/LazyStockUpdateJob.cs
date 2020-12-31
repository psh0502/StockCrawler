﻿using Common.Logging;
using Quartz;
using StockCrawler.Dao;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
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
                foreach (var d in StockHelper.GetCompanyStockList()
                    .Where(d => !d.StockName.Contains("DR"))) // 排除憑證類
                {
                    var data = collector.GetData(d.StockNo);
                    if (null != data)
                        using (var db = GetDB())
                            db.InsertOrUpdateLazyStock(data.ToDbObject());

                    Logger.InfoFormat("[{0}] get!", d.StockNo);
                    Thread.Sleep(_breakInternval * 2);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Job executing failed!", ex);
            }
            return null;
        }
    }
}