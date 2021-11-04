using Common.Logging;
using Quartz;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace StockCrawler.Services
{
    public class StockPriceHistoryInitJob : JobBase, IJob
    {
        internal static ILog Logger { get; set; } = LogManager.GetLogger(typeof(StockPriceHistoryInitJob));

        #region IJob Members

        public Task Execute(IJobExecutionContext context)
        {
            Logger.InfoFormat("Invoke [{0}]...", MethodBase.GetCurrentMethod().Name);
            // init stock list
#if (DEBUG)
            var endDate = SystemTime.Today.AddMonths(-2);
#else
            var endDate = SystemTime.Today.AddYears(-5);
#endif
            var bgnDate = SystemTime.Today;

            string stockNo = null;
            if (context != null) {
                var args = (string[])context.Get("args");
                if (null != args && args.Length > 2) stockNo = args[2];
                if (null != args && args.Length > 1) endDate = DateTime.Parse(args[1]);
            }
            IJob job = new StockPriceUpdateJob();
            for (var date = bgnDate; date > endDate; date = date.AddDays(-1))
            {
                var jobContext = new ArgumentJobExecutionContext(job);
                jobContext.Put("args", new string[] { "StockPriceHistoryInitJob", date.ToShortDateString(), stockNo });
                job.Execute(jobContext);
                Logger.Info($"Finish the {date.ToShortDateString()} {stockNo ?? "stock"} history task.");
            }
            return null;
        }

        #endregion IJob Members
    }
}