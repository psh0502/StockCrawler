using StockCrawler.Dao;

namespace StockCrawler.Services.Collectors
{
    public interface IStockMarketNewsCollector
    {
        GetStockMarketNewsResult[] GetLatestNews();
        GetStockMarketNewsResult[] GetLatestStockNews();
    }
}
