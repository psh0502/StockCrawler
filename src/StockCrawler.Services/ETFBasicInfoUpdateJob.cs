﻿using Common.Logging;
using Quartz;
using StockCrawler.Dao;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace StockCrawler.Services
{
    public class ETFBasicInfoUpdateJob : JobBase, IJob
    {
        internal static ILog Logger { get; set; } = LogManager.GetLogger(typeof(ETFBasicInfoUpdateJob));

        #region IJob Members

        public Task Execute(IJobExecutionContext context)
        {
            Logger.InfoFormat("Invoke [{0}]...", MethodBase.GetCurrentMethod().Name);
            try
            {
                string stockNo = null;
                var args = (string[])context.Get("args");
                if (null != args && args.Length > 1) stockNo = args[1];

                var collector = CollectorServiceProvider.GetETFInfoCollector();
                foreach (var d in StockHelper.GetETFList()
                    .Where(d => string.IsNullOrEmpty(stockNo) || int.Parse(d.StockNo) >= int.Parse(stockNo)))
                {
                    try
                    {
                        var info = collector.GetBasicInfo(d.StockNo);
                        var ingredients = collector.GetIngredients(d.StockNo);
                        if (null != info)
                            using (var db = GetDB())
                            {
                                db.InsertOrUpdateETFBasicInfo(info);
                                db.InsertOrUpdateETFIngredients(ingredients);
                            }
                        else
                            Logger.InfoFormat("[{0}] has no basic info", d.StockNo);
                    }
                    catch (Exception e)
                    {
                        Logger.Warn(string.Format("[{0}] has no basic info", d.StockNo), e);
                    }
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