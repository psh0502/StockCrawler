using StockCrawler.Dao;
using System;
using System.Collections.Generic;

namespace StockCrawler.Services.Collectors
{
    public interface IStockDailyInfoCollector
    {
        GetStockPeriodPriceResult GetStockDailyPriceInfo(string stockNo, DateTime date);
        IEnumerable<GetStockPeriodPriceResult> GetStockDailyPriceInfo(DateTime date);
    }
}
