using StockCrawler.Dao;

namespace StockCrawler.Services.StockBasicInfo
{
    public interface IStockBasicInfoCollector
    {
        GetStockBasicInfoResult GetStockBasicInfo(string stockNo);
    }
}
