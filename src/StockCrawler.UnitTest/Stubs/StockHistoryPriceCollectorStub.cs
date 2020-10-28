using StockCrawler.Services.Collectors;
using System;
using System.IO;

#if (DEBUG)
namespace StockCrawler.UnitTest.Stubs
{
    internal class StockHistoryPriceCollectorStub : YahooStockHistoryPriceCollector
    {
        public StockHistoryPriceCollectorStub():base() {
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
#endif