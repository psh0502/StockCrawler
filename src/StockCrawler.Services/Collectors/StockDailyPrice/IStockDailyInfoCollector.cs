using StockCrawler.Dao;
using System.Collections.Generic;

namespace StockCrawler.Services.Collectors
{
    public interface IStockDailyInfoCollector
    {
        GetStockPeriodPriceResult GetStockDailyPriceInfo(string stockNo);
        IEnumerable<GetStockPeriodPriceResult> GetStockDailyPriceInfo();
    }
}
