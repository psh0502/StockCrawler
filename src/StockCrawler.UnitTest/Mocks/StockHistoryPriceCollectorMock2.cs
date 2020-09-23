using StockCrawler.Services.Collectors;
using System;
using System.IO;
using System.Text;

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
            if (day == new DateTime(2020, 4, 6))
            {
                _logger.Info("Mock DownloadTwseStockCSV!!!");
                using (var sr = new StreamReader(@"..\..\..\StockCrawler.UnitTest\TestData\MI_INDEX_ALLBUT0999_20200406.csv", Encoding.Default))
                    return sr.ReadToEnd();
            }
            else
                return null;
        }
    }
}
