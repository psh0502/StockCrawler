using Common.Logging;
using Quartz;
using StockCrawler.Dao;
using System;
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

                foreach (var d in StockHelper.GetETFList())
                    try
                    {
                        var collector = CollectorServiceProvider.GetETFInfoCollector(d.StockName);
                        var info = collector.GetBasicInfo(d.StockNo);
                        var ingredients = collector.GetIngredients(d.StockNo);
                        if (null != info)
                            using (var db = GetDB())
                            {
                                db.InsertOrUpdateETFBasicInfo(info);
                                db.ClearETFIngredients(d.StockNo);
                                db.InsertETFIngredients(ingredients);
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
            catch (Exception ex)
            {
                Logger.Error("Job executing failed!", ex);
            }
            return null;
        }
        #endregion
    }
}