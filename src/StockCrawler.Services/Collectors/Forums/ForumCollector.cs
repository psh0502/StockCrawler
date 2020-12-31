using Common.Logging;
using StockCrawler.Dao;
using System;

namespace StockCrawler.Services.Collectors
{
    internal class ForumCollector : IForumCollector
    {
        internal ILog _logger = LogManager.GetLogger(typeof(ForumCollector));
        GetStockForumDataResult IForumCollector.GetPttData(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
