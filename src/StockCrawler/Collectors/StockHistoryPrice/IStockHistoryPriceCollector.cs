using StockCrawler.Dao;
using System;
using System.Collections.Generic;

namespace StockCrawler.Services.Collectors
{
    public interface IStockHistoryPriceCollector
    {
        IEnumerable<GetStockPeriodPriceResult> GetStockDailyPriceInfo(string stockNo, DateTime bgnDate, DateTime endDate);
    }
}
