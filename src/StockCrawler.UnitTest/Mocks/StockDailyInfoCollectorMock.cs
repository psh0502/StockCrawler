using StockCrawler.Dao;
using StockCrawler.Services.Collectors;
using System.Collections.Generic;

namespace StockCrawler.UnitTest.Mocks
{
    internal class StockDailyInfoCollectorMock : TwseStockDailyInfoCollector
    {
        public override IEnumerable<GetStockPeriodPriceResult> GetStockDailyPriceInfo()
        {
            return new List<GetStockPeriodPriceResult>() { new GetStockPeriodPriceResult() { StockNo = "2330", StockName = "台積電" } };
        }
    }
}
