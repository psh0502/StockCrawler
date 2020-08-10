using Common.Logging;
using HtmlAgilityPack;
using StockCrawler.Dao;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;

namespace StockCrawler.Services.StockBasicInfo
{
    internal class GoodInfoStockBasicInfoCollector : IStockBasicInfoCollector
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(GoodInfoStockBasicInfoCollector));
        private static readonly string UTF8SpacingChar = Encoding.UTF8.GetString(new byte[] { 0xC2, 0xA0 });
        private readonly DateTime now = SystemTime.Now;
        public GetStockBasicInfoResult GetStockBasicInfo(string stockNo)
        {
            var url = string.Format("https://goodinfo.tw/StockInfo/BasicInfo.asp?STOCK_ID={0}", stockNo);
            string html;
            var ipAddress = Tools.GetMyIpAddress();
            do
            {
                List<Cookie> cookies = new List<Cookie>
                {
                    new Cookie("CLIENT_ID", string.Format("{0}_{1}", now.ToString("yyyyMMddHHmmssfff"), ipAddress), "/", "goodinfo.tw"),
                    new Cookie("SCREEN_SIZE", "WIDTH=1920&HEIGHT=1080", "/", "goodinfo.tw")
                };
                html = Tools.DownloadStringData(new Uri(url), Encoding.UTF8, out List<Cookie> _, null, cookies);
                if (string.IsNullOrEmpty(html)) return null;
                if (html.Contains("您的瀏覽量異常"))
                {
                    _logger.InfoFormat("The target[{0}] is pissed off....wait a second...", stockNo);
                    Thread.Sleep(10 * 1000);
                }
                else
                    break;
            } while (true);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            var node = doc.DocumentNode.ChildNodes[2].SelectSingleNode("/html/body/table[2]/tr/td[3]/table[2]");
            if (null == node)
            {
                if (html.Contains("查無基本資料")) return null;
                _logger.InfoFormat("[{0}] can't find the table node! html={1}", stockNo, html);
                return null;
            }
            try
            {
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
            catch (Exception ex)
            {
                _logger.Info(ex.Message, ex);
                return null;
            }
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
