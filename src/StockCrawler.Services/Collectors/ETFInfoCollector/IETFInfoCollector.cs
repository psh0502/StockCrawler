using StockCrawler.Dao;

namespace StockCrawler.Services.Collectors
{
    internal interface IETFInfoCollector
    {
        GetETFBasicInfoResult GetBasicInfo(string etfNo);
        GetStocksResult[] GetIngredients(string etfNo);
    }
}