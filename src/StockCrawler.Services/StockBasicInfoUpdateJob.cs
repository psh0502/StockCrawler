using Common.Logging;
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
    public class StockBasicInfoUpdateJob : JobBase, IJob
    {
        internal static ILog Logger { get; set; } = LogManager.GetLogger(typeof(StockBasicInfoUpdateJob));

        public string BeginStockNo { get; set; }

        #region IJob Members

        public Task Execute(IJobExecutionContext context)
        {
            Logger.InfoFormat("Invoke [{0}]...", MethodBase.GetCurrentMethod().Name);
            try
            {
                var collector = CollectorProviderService.GetStockBasicInfoCollector();
                foreach (var d in StockHelper.GetCompanyStockList()
                    .Where(d => string.IsNullOrEmpty(BeginStockNo) || int.Parse(d.StockNo) >= int.Parse(BeginStockNo)))
                {
                    try
                    {
                        var info = collector.GetStockBasicInfo(d.StockNo);
                        if (null != info)
                            using (var db = GetDB())
                                db.InsertOrUpdateStockBasicInfo(info);
                        else
                            Logger.InfoFormat("[{0}] has no basic info", d.StockNo);
                    }
                    catch (Exception e)
                    {
                        Logger.WarnFormat("[{0}] has no basic info", d.StockNo, e);
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