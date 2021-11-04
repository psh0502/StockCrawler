using StockCrawler.Services.Collectors;
using System;
using System.IO;

#if (DEBUG)
namespace StockCrawler.UnitTest.Stubs
{
    internal class StockDailyInfoCollectorStub : TwseStockDailyInfoCollector
    {
        public StockDailyInfoCollectorStub() : base()
        {
            _logger = new UnitTestLogger();
        }
        protected override string DownloadData(DateTime day)
        {
            _logger.Info($"Mock DownloadData!!!day={day:yyyyMMdd}");
            var file = new FileInfo($@"..\..\..\StockCrawler.UnitTest\TestData\TWSE\{typeof(TwseStockDailyInfoCollector).Name}\MI_INDEX_ALLBUT0999_{day:yyyyMMdd}.csv");
            if (!file.Directory.Exists) file.Directory.Create();
            if (file.Exists)
            {
                using (var sr = file.OpenText())
                    return sr.ReadToEnd();
            }
            else
            {
                return base.DownloadData(new DateTime(2021, 11, 03));
            }
        }
    }
}
#endif