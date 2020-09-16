using StockCrawler.Dao;

namespace StockCrawler.Services.Collectors
{
    public interface IStockBasicInfoCollector
    {
        GetStockBasicInfoResult GetStockBasicInfo(string stockNo);
    }
}
