using HtmlAgilityPack;
using StockCrawler.Dao;
using System;
using System.Text;
using System.Web;

namespace StockCrawler.Services.StockBasicInfo
{
    internal class GoodInfoStockBasicInfoCollector : IStockBasicInfoCollector
    {
        private static readonly string UTF8SpacingChar = Encoding.UTF8.GetString(new byte[] { 0xC2, 0xA0 });
        public GetStockBasicInfoResult GetStockBasicInfo(string stockNo)
        {
            var url = string.Format("https://goodinfo.tw/StockInfo/BasicInfo.asp?STOCK_ID={0}", stockNo);
            var html = Tools.DownloadStringData(url, Encoding.UTF8).Trim();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            var node = doc.DocumentNode.ChildNodes[2].SelectSingleNode("/html/body/table[2]/tr/td[3]/table[2]");
            return new GetStockBasicInfoResult()
            {
                StockNo = stockNo,
                StockName = node.SelectSingleNode("tr[2]/td[4]").InnerText,
                Category = node.SelectSingleNode("tr[3]/td[2]").InnerText,
                CompanyName = node.SelectSingleNode("tr[4]/td[2]").InnerText,
                Capital = ParseCapital(node.SelectSingleNode("tr[8]/td[2]").InnerText),
                BuildDate = DateTime.Parse(node.SelectSingleNode("tr[6]/td[2]").InnerText.Substring(0, 10)),
                PublishDate = DateTime.Parse(node.SelectSingleNode("tr[7]/td[2]").InnerText.Substring(0, 10)),
                MarketValue = ParseCapital(node.SelectSingleNode("tr[9]/td[2]").InnerText),
                ReleaseStockCount = ParseStockCount(node.SelectSingleNode("tr[10]/td[2]").InnerText),
                Chairman = node.SelectSingleNode("tr[12]/td[2]").InnerText,
                CEO = node.SelectSingleNode("tr[13]/td[2]").InnerText,
                CompanyID = node.SelectSingleNode("tr[16]/td[2]").InnerText,
                Url = node.SelectSingleNode("tr[20]/td[2]").InnerText,
                Businiess = node.SelectSingleNode("tr[23]/td[2]").InnerText
            };
        }

        private static long ParseStockCount(string innerText)
        {
            innerText = HttpUtility.HtmlDecode(innerText);
            var position = innerText.IndexOf(UTF8SpacingChar);
            return long.Parse(innerText.Substring(0, position).Replace(",", ""));
        }

        private static decimal ParseCapital(string innerText)
        {
            innerText = HttpUtility.HtmlDecode(innerText);
            var position = innerText.IndexOf(UTF8SpacingChar);
            var number = decimal.Parse(innerText.Substring(0, position));
            var unit = innerText.Substring(position + 1, 1);

            switch(unit)
            {
                case "兆":
                    number *= 1000000000000M;
                    break;
                case "億":
                    number *= 100000000M;
                    break;
                case "萬":
                    number *= 10000M;
                    break;
            }
            return number;
        }
    }
}
