using StockCrawler.Dao;
using StockCrawler.Services.Collectors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StockCrawler.UnitTest.Mocks
{
    internal class StockDailyInfoCollectorMock : TwseStockDailyInfoCollector
    {
        public StockDailyInfoCollectorMock() : base()
        {
            _logger = new UnitTestLogger();
        }
        public override IEnumerable<GetStockPeriodPriceResult> GetStockDailyPriceInfo()
        {
            return new List<GetStockPeriodPriceResult>() {
                new GetStockPeriodPriceResult() {
                    StockNo = "2330", 
                    StockName = "台積電" } 
            };
        }
        protected override string DownloadData(DateTime day)
        {
            _logger.Info("Mock DownloadTwseStockCSV!!!");
            using (var sr = new StreamReader(@"..\..\..\StockCrawler.UnitTest\TestData\MI_INDEX_ALLBUT0999_20200406.csv", Encoding.Default))
                return sr.ReadToEnd();
        }
    }
}
