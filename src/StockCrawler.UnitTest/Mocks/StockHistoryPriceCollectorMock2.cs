using StockCrawler.Services.Collectors;
using System;
using System.IO;

namespace StockCrawler.UnitTest.Mocks
{
    internal class StockHistoryPriceCollectorMock2 : TwseStockHistoryPriceCollector2, IStockHistoryPriceCollector
    {
        public StockHistoryPriceCollectorMock2() : base()
        {
            _logger = new UnitTestLogger();
        }
        protected override string DownloadData(DateTime day)
        {
            _logger.Info("Mock DownloadTwseStockCSV!!!");
            using (var sr = new StreamReader(@"..\..\..\StockCrawler.UnitTest\TestData\MI_INDEX_ALLBUT0999_20200406.csv"))
                return sr.ReadToEnd();
        }
    }
}
