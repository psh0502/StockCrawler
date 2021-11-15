﻿using Common.Logging;
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
                var targetDate = SystemTime.Today;
                string stockNo = null;
                if (null != args && args.Length > 1) targetDate = DateTime.Parse(args[1]);
                if (null != args && args.Length > 2) stockNo = args[2];
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
                            }).ToList();
                        // #34 Filter out non-trade day price data
                        priceInfo = priceInfo.Where(d => d.ClosePrice > 0).ToArray();
                        if (!string.IsNullOrEmpty(stockNo)) 
                            priceInfo = priceInfo.Where(d => d.StockNo == stockNo).ToArray();

                        using (var db = GetDB())
                        {
                            if (string.IsNullOrEmpty(stockNo) && stocks.Any())
                                db.InsertOrUpdateStock(stocks.ToArray());

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