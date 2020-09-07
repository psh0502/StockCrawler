using Common.Logging;
using Quartz;
using StockCrawler.Dao;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace StockCrawler.Services
{
    public class StockBasicInfoUpdateJob : JobBase, IJob
    {
        internal static ILog Logger { get; set; } = LogManager.GetLogger(typeof(StockBasicInfoUpdateJob));

        public StockBasicInfoUpdateJob()
            : base()
        {
            if (null == Logger)
                Logger = LogManager.GetLogger(typeof(StockBasicInfoUpdateJob));
        }
        public string BeginStockNo { get; set; }

        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            Logger.InfoFormat("Invoke [{0}]...", MethodBase.GetCurrentMethod().Name);
            try
            {
                using (var db = StockDataServiceProvider.GetServiceInstance())
                {
                    var collector = CollectorProviderService.GetBasicInfoCollector();
                    foreach (var d in db.GetStocks().Where(d => !d.StockNo.StartsWith("0") && (string.IsNullOrEmpty(BeginStockNo) || int.Parse(d.StockNo.Substring(0, 4)) >= int.Parse(BeginStockNo)))) // 排除非公司的基金型股票
                    {
                        try
                        {
                            var info = collector.GetStockBasicInfo(d.StockNo);
                            if (null != info)
                                db.UpdateStockBasicInfo(info);
                            else
                                Logger.InfoFormat("[{0}] has no basic info", d.StockNo);
                        }catch(Exception e)
                        {
                            Logger.WarnFormat("[{0}] has no basic info", d.StockNo, e);
                        }
                        Thread.Sleep(1 * 1000);
                    }
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