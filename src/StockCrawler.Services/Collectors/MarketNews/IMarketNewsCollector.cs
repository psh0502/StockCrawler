using StockCrawler.Dao;

namespace StockCrawler.Services.Collectors
{
    public interface IMarketNewsCollector
    {
        GetMarketNewsResult[] GetLatestNews();
        GetStockMarketNewsResult[] GetLatestStockNews(string stockNo);
    }
}
