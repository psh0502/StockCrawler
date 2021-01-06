using StockCrawler.Dao;
using System;
using System.Collections.Generic;

namespace StockCrawler.Services.Collectors
{
    public interface IStockForumCollector
    {
        IList<(GetStockForumDataResult Article, IList<GetStocksResult> relateToStockNo)> GetPttData(DateTime date);
    }
}
