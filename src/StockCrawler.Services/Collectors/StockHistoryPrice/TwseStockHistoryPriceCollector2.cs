using StockCrawler.Dao;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace StockCrawler.Services.Collectors
{
    /// <summary>
    /// 透過每日收盤的數據回朔過去歷史股價
    /// </summary>
    internal class TwseStockHistoryPriceCollector2 : TwseStockDailyInfoCollector, IStockHistoryPriceCollector
    {
        private readonly Dictionary<string, List<GetStockPeriodPriceResult>> _dataset = new Dictionary<string, List<GetStockPeriodPriceResult>>();
        public virtual IEnumerable<GetStockPeriodPriceResult> GetStockHistoryPriceInfo(string stockNo, DateTime bgnDate, DateTime endDate)
        {
            if (_dataset.ContainsKey(stockNo)) return _dataset[stockNo];

            for (DateTime processing_date = bgnDate;
                processing_date <= endDate;
                processing_date = processing_date.AddDays(1))
            {
                while (true) // retry till it get
                    try
                    {
                        var r = GetAllStockDailyPriceInfo(processing_date);
                        if (null != r)
                            foreach (var d in r)
                            {
                                if (!_dataset.ContainsKey(d.StockNo))
                                    _dataset[d.StockNo] = new List<GetStockPeriodPriceResult>();

                                _dataset[d.StockNo].Add(d);
                            }
                        break;
                    }
                    catch (WebException)
                    {
                        _logger.WarnFormat("Target website refuses our connection. Wait till it get peace. stockNo={0}, processing_date={1}", stockNo, processing_date.ToString("yyyy-MM-dd"));
                        Thread.Sleep((int)new TimeSpan(1, 0, 0).TotalMilliseconds);
                    }

                Thread.Sleep(_breakInternval);
            }

            return _dataset[stockNo];
        }
    }
}
