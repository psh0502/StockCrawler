using StockCrawler.Services.StockDailyPrice;
using System;
using System.Collections.Generic;

namespace StockCrawler.UnitTest.Mocks
{
    internal class StockDailyInfoCollectorMock : IStockDailyInfoCollector
    {
        public StockDailyPriceInfo GetStockDailyPriceInfo(string stockNo)
        {
            throw new NotImplementedException();
        }

        public IList<StockDailyPriceInfo> GetStockDailyPriceInfo()
        {
            return new List<StockDailyPriceInfo>() { new StockDailyPriceInfo() { StockNo = "2330", StockName = "台積電" } };
        }
    }
}
