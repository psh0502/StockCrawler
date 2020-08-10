using StockCrawler.Dao;

namespace StockCrawler.Services.StockFinanceReport
{
    public interface IStockFinanceReportCashFlowCollector
    {
        GetStockReportCashFlowResult GetStockFinanceReportCashFlow(string stockNo, short year, short season);
    }
}
