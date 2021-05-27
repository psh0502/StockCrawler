using Common.Logging;
using Quartz;
using StockCrawler.Dao;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace StockCrawler.Services
{
    public class StockPriceUpdateJob : JobBase, IJob
    {
        internal static ILog Logger { get; set; } = LogManager.GetLogger(typeof(StockPriceUpdateJob));

        #region IJob Members

        public Task Execute(IJobExecutionContext context)
        {
            Logger.InfoFormat("Invoke [{0}]...", MethodBase.GetCurrentMethod().Name);
            try
            {
                var args = (string[])context.Get("args");
                if (args != null && args.Length > 1)
                    SystemTime.SetFakeTime(DateTime.Parse(args[1]));

                var collector = CollectorServiceProvider.GetStockDailyPriceCollector();

                var priceInfo = collector.GetStockDailyPriceInfo();
                var stocks = priceInfo
                    .Select(d => new GetStocksResult()
                    {
                        StockNo = d.StockNo,
                        StockName = d.StockName,
                        Enable = true
                    }).ToList();
                // #34 Filter out non-trade day price data
                priceInfo = priceInfo.Where(d => d.ClosePrice > 0).ToArray();

                using (var db = GetDB())
                {
                    if (stocks.Any())
                        db.InsertOrUpdateStock(stocks.ToArray());

                    if (priceInfo.Any())
                        // 寫入日價
                        db.InsertOrUpdateStockPrice(priceInfo.ToArray());
                }
                Tools.CalculateMAAndPeriodK(SystemTime.Today);
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