using StockCrawler.Dao;

namespace StockCrawler.Services.StockFinanceReport
{
    public interface IStockFinanceReportCollector
    {
        GetStockBasicInfoResult GetStockBasicInfo(string stockNo);
    }
}
