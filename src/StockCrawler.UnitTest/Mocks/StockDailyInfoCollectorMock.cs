using StockCrawler.Services.StockDailyPrice;
using System.Collections.Generic;

namespace StockCrawler.UnitTest.Mocks
{
    internal class StockDailyInfoCollectorMock : TwseStockDailyInfoCollector
    {
        public override IList<StockDailyPriceInfo> GetStockDailyPriceInfo()
        {
            return new List<StockDailyPriceInfo>() { new StockDailyPriceInfo() { StockNo = "2330", StockName = "台積電" } };
        }
    }
}
