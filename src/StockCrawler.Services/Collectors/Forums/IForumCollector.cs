using StockCrawler.Dao;
using System;

namespace StockCrawler.Services.Collectors
{
    public interface IForumCollector
    {
        GetStockForumDataResult GetPttData(DateTime date);
    }
}
