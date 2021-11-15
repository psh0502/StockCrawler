using Common.Logging;
using Quartz;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace StockCrawler.Services
{
    public class StockCaculationJob : JobBase, IJob
    {
        internal static ILog Logger { get; set; } = LogManager.GetLogger(typeof(StockCaculationJob));

        #region IJob Members

        public Task Execute(IJobExecutionContext context)
        {
            Logger.InfoFormat("Invoke [{0}]...", MethodBase.GetCurrentMethod().Name);
            try
            {
                var args = (string[])context.Get("args");
                var targetDate = SystemTime.Today;
                if (null != args && args.Length > 1) targetDate = DateTime.Parse(args[1]);
                do
                {
                    Tools.CalculateMA(targetDate);
                    Tools.CalculateTechnicalIndicators(targetDate);
                    targetDate = targetDate.AddDays(1);
                } while (targetDate <= SystemTime.Today);
            }
            catch (Exception ex)
            {
                Logger.Error("Job executing failed!", ex);
                throw;
            }
            return null;
        }
        #endregion
    }
}