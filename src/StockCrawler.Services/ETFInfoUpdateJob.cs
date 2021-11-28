using Common.Logging;
using Quartz;
using StockCrawler.Dao;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace StockCrawler.Services
{
    public class ETFInfoUpdateJob : JobBase, IJob
    {
        internal static ILog Logger { get; set; } = LogManager.GetLogger(typeof(ETFInfoUpdateJob));

        #region IJob Members

        public Task Execute(IJobExecutionContext context)
        {
            Logger.InfoFormat("Invoke [{0}]...", MethodBase.GetCurrentMethod().Name);
            try
            {
                foreach (var d in StockHelper.GetETFList())
                    try
                    {
                        var collector = CollectorServiceProvider.GetETFInfoCollector(d.StockName);
                        if (collector != null)
                        {
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
                    }
                    catch (Exception e)
                    {
                        Logger.Error(string.Format("[{0}] failed to grab ETF info.", d.StockNo), e);
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