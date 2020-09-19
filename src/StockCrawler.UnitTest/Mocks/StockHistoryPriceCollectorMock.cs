using StockCrawler.Services.Collectors;
using System;
using System.IO;

namespace StockCrawler.UnitTest.Mocks
{
    internal class StockHistoryPriceCollectorMock : YahooStockHistoryPriceCollector
    {
        public StockHistoryPriceCollectorMock():base() {
            _logger = new UnitTestLogger();
        }
        protected override string DownloadYahooStockCSV(string stockNo, DateTime startDT, DateTime endDT)
        {
            _logger.Info("Mock DownloadYahooStockCSV!!!");
            using(var sr = new StreamReader(@"..\..\..\StockCrawler.UnitTest\TestData\yahoo_history_2330.csv"))
                return sr.ReadToEnd();
        }
    }
}
