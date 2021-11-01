using StockCrawler.Dao;
using System;
using System.Collections.Generic;

namespace StockCrawler.Services.Collectors
{
    public interface IStockDailyInfoCollector
    {
        GetStockPriceHistoryResult GetStockDailyPriceInfo(string stockNo, DateTime date);
        IEnumerable<GetStockPriceHistoryResult> GetStockDailyPriceInfo(DateTime date);
    }
}
