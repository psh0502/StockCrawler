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
            _logger.Info($"Mock DownloadTwseStockCSV!!!day={day:yyyyMMdd}");
            var file = new FileInfo($@"..\..\..\StockCrawler.UnitTest\TestData\TWSE\MI_INDEX_ALLBUT0999_{day:yyyyMMdd}.csv");
            if (file.Exists)
            {
                using (var sr = file.OpenText())
                    return sr.ReadToEnd();
            }
            else
                return null;
        }
    }
}
