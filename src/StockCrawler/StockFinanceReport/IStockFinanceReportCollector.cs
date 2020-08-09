using StockCrawler.Dao;
using System.Collections.Generic;

namespace StockCrawler.Services.StockFinanceReport
{
    public interface IStockFinanceReportCashFlowCollector
    {
        IList<GetStockReportCashFlowResult> GetStockFinanceReportCashFlow(string stockNo, short year = -1, short season = -1);
    }
}
