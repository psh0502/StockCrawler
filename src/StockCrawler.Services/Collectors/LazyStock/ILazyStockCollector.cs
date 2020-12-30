namespace StockCrawler.Services.Collectors
{
    public interface ILazyStockCollector
    {
        LazyStockData GetData(string stockNo);
    }
}
