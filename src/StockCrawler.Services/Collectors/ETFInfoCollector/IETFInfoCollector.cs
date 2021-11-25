using StockCrawler.Dao;

namespace StockCrawler.Services.Collectors
{
    internal interface IETFInfoCollector
    {
        GetETFBasicInfoResult GetBasicInfo(string etfNo);
        GetETFIngredientsResult[] GetIngredients(string etfNo);
    }
}