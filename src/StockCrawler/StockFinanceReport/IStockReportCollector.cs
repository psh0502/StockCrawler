using StockCrawler.Dao;

namespace StockCrawler.Services.StockFinanceReport
{
    public interface IStockReportCashFlowCollector
    {
        GetStockReportCashFlowResult GetStockReportCashFlow(string stockNo, short year, short season);
        GetStockReportIncomeResult GetStockReportIncome(string stockNo, short year, short season);
    }
}
