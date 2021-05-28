using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockCrawler.Dao;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StockCrawler.UnitTest.Others
{
#if(DEBUG)
    /// <summary>
    /// These classes in [Others] folder are not real unit testing.
    /// They are some tools for fixing some issues. 
    /// I put them here for easy to launch.
    /// </summary>
    [TestClass]
    public class UnitTestTools
    {
        [TestMethod]
        public void ReGeneratePeriodPriceHistoryForPatchingDeltaDataMissing()
        {
            var _logger = new UnitTestLogger();
            var bgnDate = new DateTime(2015, 9, 30);
            _logger.InfoFormat("bgnDate = {0:yyyy-MM-dd}", bgnDate);
            do
            {
                CalculateMAAndPeriodK(bgnDate);
                bgnDate = bgnDate.AddDays(1);
            } while (bgnDate < DateTime.Today);
        }
        /// <summary>
        /// 根據本日收盤資料, 計算 均線(MA 移動線)和不同周期的 K 棒
        /// </summary>
        /// <param name="list">今日收盤價</param>
        public static void CalculateMAAndPeriodK(DateTime date)
        {
            var _logger = new UnitTestLogger();
            _logger.InfoFormat("Begin caculation MA and K ...{0}", date.ToString("yyyyMMdd"));
            var K5_list = new List<GetStockPeriodPriceResult>();
            var K20_list = new List<GetStockPeriodPriceResult>();
            using (var db = RepositoryProvider.GetRepositoryInstance())
            {
                foreach (var d in StockHelper.GetAllStockList())
                {
                    var target_weekend_date = date.AddDays(5 - (int)date.DayOfWeek);
                    _logger.Debug($"[{d.StockNo}]target_weekend_date:{target_weekend_date:yyyy-MM-dd}");

                    if (date >= target_weekend_date)
                    {
                        // 週 K
                        var bgnDate = target_weekend_date.AddDays(-4);
                        var data = db.GetStockPeriodPrice(d.StockNo, 1, bgnDate, target_weekend_date).ToList();
                        if (data.Any())
                        {
                            var first = data.OrderBy(x => x.StockDT).First();
                            var last = data.OrderByDescending(x => x.StockDT).First();
                            var lastPeriodClosePrice = (first.ClosePrice - first.DeltaPrice);
                            var deltaPrice = (last.ClosePrice - lastPeriodClosePrice);
                            var tmp = new GetStockPeriodPriceResult()
                            {
                                StockNo = d.StockNo,
                                StockDT = bgnDate,
                                OpenPrice = first.OpenPrice,
                                ClosePrice = last.ClosePrice,
                                HighPrice = data.Max(x => x.HighPrice),
                                LowPrice = data.Min(x => x.LowPrice),
                                Volume = data.Sum(x => x.Volume),
                                Period = 5,
                                DeltaPrice = deltaPrice,
                                DeltaPercent = deltaPrice / lastPeriodClosePrice
                            };
                            if (tmp.Volume > 0) K5_list.Add(tmp);
                        }

                        target_weekend_date = target_weekend_date.AddDays(7);
                        _logger.Debug($"[{d.StockNo}]target_weekend_date:{target_weekend_date:yyyy-MM-dd}");
                    }

                    var target_monthend_date = new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
                    _logger.Debug($"[{d.StockNo}]target_monthend_date:{target_monthend_date:yyyy-MM-dd}");

                    if (date >= target_monthend_date)
                    {
                        // 月 K
                        var bgnDate = new DateTime(target_monthend_date.Year, target_monthend_date.Month, 1);
                        var data = db.GetStockPeriodPrice(d.StockNo, 1, bgnDate, target_monthend_date).ToList();
                        if (data.Any())
                        {
                            var first = data.OrderBy(x => x.StockDT).First();
                            var last = data.OrderByDescending(x => x.StockDT).First();
                            var lastPeriodClosePrice = (first.ClosePrice - first.DeltaPrice);
                            var deltaPrice = (last.ClosePrice - lastPeriodClosePrice);
                            var tmp = new GetStockPeriodPriceResult()
                            {
                                StockNo = d.StockNo,
                                StockDT = bgnDate,
                                OpenPrice = first.OpenPrice,
                                ClosePrice = last.ClosePrice,
                                HighPrice = data.Max(x => x.HighPrice),
                                LowPrice = data.Min(x => x.LowPrice),
                                Volume = data.Sum(x => x.Volume),
                                Period = 20,
                                DeltaPrice = deltaPrice,
                                DeltaPercent = deltaPrice / lastPeriodClosePrice
                            };
                            if (tmp.Volume > 0) K20_list.Add(tmp);
                        }
                        target_monthend_date = bgnDate.AddMonths(2).AddDays(-1);
                        _logger.Debug($"[{d.StockNo}]target_monthend_date:{target_monthend_date:yyyy-MM-dd}");
                    }
                }
                // 寫入 K 線棒
                if (K5_list.Any())
                    db.InsertOrUpdateStockPrice(K5_list.ToArray());
                if (K20_list.Any())
                    db.InsertOrUpdateStockPrice(K20_list.ToArray());
            }
        }
    }
#endif
}
