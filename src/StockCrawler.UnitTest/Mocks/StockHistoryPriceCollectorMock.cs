using StockCrawler.Services.StockHistoryPrice;
using System;
using System.IO;

namespace StockCrawler.UnitTest.Mocks
{
    internal class StockHistoryPriceCollectorMock : YaooStockHistoryPriceCollector
    {
        protected override string DownloadYahooStockCSV(string stockNo, DateTime startDT, DateTime endDT)
        {
            using(var sr = new StreamReader(@"..\..\..\StockCrawler.UnitTest\TestData\yahoo_history_2330.csv"))
                return sr.ReadToEnd();
        }
    }
}
