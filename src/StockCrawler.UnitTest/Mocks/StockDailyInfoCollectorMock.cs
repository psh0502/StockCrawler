using StockCrawler.Dao;
using StockCrawler.Services.Collectors;
using System;
using System.Collections.Generic;
using System.IO;

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
            return new List<GetStockPeriodPriceResult>() 
            {
                new GetStockPeriodPriceResult() 
                {
                    StockNo = "2330",
                    StockName = "台積電" 
                }
            };
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
