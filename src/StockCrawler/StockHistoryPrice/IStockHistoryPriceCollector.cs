using StockCrawler.Dao;
using System;
using System.Collections.Generic;

namespace StockCrawler.Services.StockHistoryPrice
{
    public interface IStockHistoryPriceCollector
    {
        IEnumerable<GetStockPriceHistoryResult> GetStockDailyPriceInfo(string stockNo, DateTime bgnDate, DateTime endDate);
    }
}
