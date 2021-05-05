using HtmlAgilityPack;
using StockCrawler.Services.Collectors;
using System.IO;

namespace StockCrawler.UnitTest.Stubs
{
#if (DEBUG)
    internal class StockInterestIssuedCollectorStub : TwseInterestIssuedCollector
    {
        protected override HtmlNode GetTwseDataBack(string url, string stockNo, short year = -1, short season = -1, short month = -1, short step = 1, string xpath = "/html/body/center/table[2]")
        {
            _logger = new UnitTestLogger();
            string html = null;
            _logger.Info($"Mock DownloadData!!!");
            FileInfo file = null;
            switch (stockNo) {
                case "2330":
                    file = new FileInfo($@"..\..\..\StockCrawler.UnitTest\TestData\TWSE\Issued\2330_110_-1_-1_2021-03-16_1.html");
                    break;
                case "1477":
                    file = new FileInfo($@"..\..\..\StockCrawler.UnitTest\TestData\TWSE\Issued\1477_110_-1_-1_2021-03-16_1.html");
                    break;
            }
            if (file.Exists)
            {
                using (var sr = file.OpenText())
                    html = sr.ReadToEnd();

                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                return doc.DocumentNode.SelectSingleNode(xpath);
            }
            else
                return null;
        }
    }
#endif
}
