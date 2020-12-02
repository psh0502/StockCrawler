using StockCrawler.Dao;
using StockCrawler.Services.Collectors;
using System;
using System.Collections.Generic;
using System.IO;

namespace StockCrawler.UnitTest.Stubs
{
#if (DEBUG)
    internal class StockMarketNewsCollectorStub : TwseMarketNewsCollector
    {
        public override GetStockMarketNewsResult[] GetLatestNews()
        {
            var list = new List<GetStockMarketNewsResult>
            {
                new GetStockMarketNewsResult()
                {
                    StockNo = "0000",
                    Source = "twse",
                    NewsDate = new DateTime(2020, 10, 27),
                    Subject = "中央再保險股份有限公司等27家公司將於109年11月份假證交所場地舉辦法人說明會",
                    Url = "https://www.twse.com.tw/zh/news/newsDetail/ff80808174fce5b301756981369f017e"
                },
                new GetStockMarketNewsResult()
                {
                    StockNo = "0000",
                    Source = "twse",
                    NewsDate = new DateTime(2020, 10, 27),
                    Subject = "耀登科技股份有限公司申報初次上市前公開銷售之現金增資發行普通股案申報生效",
                    Url = "https://www.twse.com.tw/zh/news/newsDetail/ff80808174fce5b3017569620ee7017a"
                },
                new GetStockMarketNewsResult()
                {
                    StockNo = "0000",
                    Source = "twse",
                    NewsDate = new DateTime(2020, 10, 27),
                    Subject = "Sports Gear Co., Ltd.（志強國際企業股份有限公司，股票代號: 6768）向臺灣證券交易所申請股票第一上市",
                    Url = "https://www.twse.com.tw/zh/news/newsDetail/ff80808174fce5b3017568e27bec0176"
                }
            };

            return list.ToArray();
        }
        protected override string DownloadMopsData()
        {
            var file = new FileInfo($@"..\..\..\StockCrawler.UnitTest\TestData\TWSE\2020-12-03.html");
            if (file.Exists)
            {
                using (var sr = file.OpenText())
                    return sr.ReadToEnd();
            }
            else
                return null;
        }
    }
#endif
}
