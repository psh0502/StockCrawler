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
                var targetDate = SystemTime.Today;
                string stockNo = null;
                if (context != null)
                {
                    var args = ((string[])context.Get("args")) ?? new string[] { };
                    if (DateTime.TryParse(args[0], out targetDate))
                        stockNo = null;
                    else
                        stockNo = args[0];
                    if (args.Length > 1) stockNo = args[1];
                    if(targetDate == DateTime.MinValue) targetDate = SystemTime.Today;
                }
                if (!targetDate.IsWeekend())
                {
                    var collector = CollectorServiceProvider.GetStockDailyPriceCollector();
                    var priceInfo = collector.GetStockDailyPriceInfo(targetDate);
                    if (null != priceInfo)
                    {
                        var stocks = priceInfo
                            .Select(d => new GetStocksResult()
                            {
                                StockNo = d.StockNo,
                                StockName = d.StockName,
                                Enable = true
                            }).ToArray();
                        // #34 Filter out non-trade day price data
                        priceInfo = priceInfo.Where(d => d.ClosePrice > 0);
                        if (!string.IsNullOrEmpty(stockNo))
                        {
                            priceInfo = priceInfo.Where(d => d.StockNo == stockNo).ToArray();
                            stocks = stocks.Where(d => d.StockNo == stockNo).ToArray();
                        }

                        using (var db = GetDB())
                        {
                            if (string.IsNullOrEmpty(stockNo) && stocks.Any())
                                db.InsertOrUpdateStock(stocks);

                            if (priceInfo.Any())
                                // 寫入日價
                                db.InsertOrUpdateStockPrice(priceInfo.ToArray());
                        }
                    }
                }
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