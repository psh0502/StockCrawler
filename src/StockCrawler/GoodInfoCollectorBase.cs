using Common.Logging;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;

namespace StockCrawler.Services
{
    internal abstract class GoodInfoCollectorBase
    {
        internal static ILog _logger = LogManager.GetLogger(typeof(GoodInfoCollectorBase));
        protected static readonly string UTF8SpacingChar = Encoding.UTF8.GetString(new byte[] { 0xC2, 0xA0 });
        protected readonly DateTime now = SystemTime.Now;

        protected static T GetNodeTextTo<T>(HtmlNode node)
        {
            if (null == node) return default;
            var innerText = HttpUtility.HtmlDecode(node.InnerText.Trim().Replace(",", string.Empty));
            innerText = innerText.Replace(UTF8SpacingChar, string.Empty);
            if (typeof(T) == typeof(int))
                return (T)((object)int.Parse(innerText));
            else if (typeof(T) == typeof(decimal))
                return (T)((object)decimal.Parse(innerText));
            else if (typeof(T) == typeof(double))
                return (T)((object)double.Parse(innerText));
            else if (typeof(T) == typeof(float))
                return (T)((object)float.Parse(innerText));
            else if (typeof(T) == typeof(string))
                return (T)((object)innerText.Trim());
            else if (typeof(T) == typeof(bool))
                return (T)((object)bool.Parse(innerText));
            else
                throw new InvalidCastException(typeof(T).Name + " is not defined in GetNodeTextTo.");
        }
        protected virtual string GetGoodInfoData(string url, string stockNo)
        {
            url = string.Format(url, stockNo);
            string html;
            var ipAddress = Tools.GetMyIpAddress();
            do
            {
                IList<Cookie> cookies = new List<Cookie>
                {
                    new Cookie("CLIENT_ID", string.Format("{0}_{1}", now.ToString("yyyyMMddHHmmssfff"), ipAddress), "/", "goodinfo.tw"),
                    new Cookie("SCREEN_SIZE", "WIDTH=1920&HEIGHT=1080", "/", "goodinfo.tw"),
                    new Cookie("GOOD_INFO_STOCK_BROWSE_LIST", $"3|{stockNo}", "/", "goodinfo.tw")
                };
                html = Tools.DownloadStringData(new Uri(url), Encoding.UTF8, out IList<Cookie> _, null, cookies);
                if (string.IsNullOrEmpty(html)) return null;
                if (html.Contains("您的瀏覽量異常"))
                {
                    _logger.InfoFormat("The target[{0}] is pissed off....wait a second...", stockNo);
                    Thread.Sleep(10 * 1000);
                }
                else
                    break;
            } while (true);

            return html;
        }
    }
}
