using StockCrawler.Dao;
using StockCrawler.Services.Collectors;
using System.Collections.Generic;
using System.Linq;

namespace StockCrawler.UnitTest.Stubs
{
    internal class StockReportCollectorStub : IStockReportCollector
    {
        private const string TEST_STOCK_NO_1 = "2330";
        private static readonly List<GetStockFinancialReportResult> _StockFinancial;
        static StockReportCollectorStub()
        {
            _StockFinancial = new List<GetStockFinancialReportResult>
            {
                new GetStockFinancialReportResult() { StockNo = TEST_STOCK_NO_1, Year = 109 , Season = 1 },
                new GetStockFinancialReportResult() { StockNo = TEST_STOCK_NO_1, Year = 108 , Season = 4 },
                new GetStockFinancialReportResult() { StockNo = TEST_STOCK_NO_1, Year = 108 , Season = 3 },
                new GetStockFinancialReportResult() { StockNo = TEST_STOCK_NO_1, Year = 108 , Season = 2 },
                new GetStockFinancialReportResult() { StockNo = TEST_STOCK_NO_1, Year = 108 , Season = 1 },
                new GetStockFinancialReportResult() { StockNo = TEST_STOCK_NO_1, Year = 107 , Season = 4 },
                new GetStockFinancialReportResult() { StockNo = TEST_STOCK_NO_1, Year = 107 , Season = 3 },
                new GetStockFinancialReportResult() { StockNo = TEST_STOCK_NO_1, Year = 107 , Season = 2 },
                new GetStockFinancialReportResult() { StockNo = TEST_STOCK_NO_1, Year = 107 , Season = 1 },
            };
        }

        public GetStockFinancialReportResult GetStockFinancialReport(string stockNo, short year, short season)
        {
            return _StockFinancial
                .Where(d => d.StockNo == stockNo 
                    && (d.Year == year || year == -1) 
                    && (d.Season == season || season == -1))
                .FirstOrDefault();
        }
    }
}
