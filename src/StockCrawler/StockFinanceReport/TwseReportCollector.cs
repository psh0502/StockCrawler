using StockCrawler.Dao;
using System;
using System.Collections.Generic;

namespace StockCrawler.Services.StockFinanceReport
{
    internal class TwseReportCollector : IStockFinanceReportCashFlowCollector
    {
        public IList<GetStockReportCashFlowResult> GetStockFinanceReportCashFlow(string stockNo, short year, short season)
        {
            throw new NotImplementedException();
        }
    }
}
