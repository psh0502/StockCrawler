using StockCrawler.Dao;
using StockCrawler.Services.Collectors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#if (DEBUG)
namespace StockCrawler.UnitTest.Stubs
{
    internal class StockDailyInfoCollectorStub : TwseStockDailyInfoCollector
    {
        public StockDailyInfoCollectorStub() : base()
        {
            _logger = new UnitTestLogger();
        }
        public override IEnumerable<GetStockPeriodPriceResult> GetStockDailyPriceInfo()
        {
            using (var db = StockDataServiceProvider.GetServiceInstance())
                return db.GetStocks().Select(d =>
                new GetStockPeriodPriceResult()
                {
                    StockNo = d.StockNo,
                    StockName = d.StockName
                });
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
#endif