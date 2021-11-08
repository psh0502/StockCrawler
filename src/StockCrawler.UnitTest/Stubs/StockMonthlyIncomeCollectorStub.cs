﻿using HtmlAgilityPack;
using StockCrawler.Services.Collectors;
using System.IO;

namespace StockCrawler.UnitTest.Stubs
{
#if (DEBUG)
    internal class StockMonthlyIncomeCollectorStub : TwseMonthlyIncomeCollector
    {
        protected override HtmlNode GetTwseDataBack(string url, string stockNo, short year = -1, short season = -1, short month = -1, short step = 1, string xpath = "/html/body/div/table[2]")
        {
            _logger = new UnitTestLogger();
            string html = null;
            _logger.Info($"Mock DownloadData!!!");

            var file = new FileInfo($@"..\..\..\StockCrawler.UnitTest\TestData\TWSE\{typeof(TwseMonthlyIncomeCollector).Name}\{stockNo}_2.html");
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
