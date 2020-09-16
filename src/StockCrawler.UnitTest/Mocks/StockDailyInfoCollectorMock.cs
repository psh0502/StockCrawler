using StockCrawler.Services.Collectors;
using System.Collections.Generic;

namespace StockCrawler.UnitTest.Mocks
{
    internal class StockDailyInfoCollectorMock : TwseStockDailyInfoCollector
    {
        public override IEnumerable<StockDailyPriceInfo> GetStockDailyPriceInfo()
        {
            return new List<StockDailyPriceInfo>() { new StockDailyPriceInfo() { StockNo = "2330", StockName = "台積電" } };
        }
    }
}
