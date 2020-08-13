using Common.Logging;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace StockCrawler.Services
{
    public class Tools
    {
        internal static ILog _logger = LogManager.GetLogger(typeof(Tools));
        public static string DownloadStringData(Uri url, Encoding encode, out IList<Cookie> respCookies, string contentType = null, IList<Cookie> cookies = null, string method = "GET", NameValueCollection formdata = null, string refer = null)
        {
            _logger.DebugFormat("url=[{0}]", url.OriginalString);
             
            respCookies = new List<Cookie>();
            string downloaded_data = null;
            // https://blog.darkthread.net/blog/disable-tls-1-0-issues
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var req = WebRequest.CreateHttp(url);
            req.Method = method;
            if(!string.IsNullOrEmpty(contentType)) req.ContentType = contentType;
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
            req.Referer = refer;
            if (null != cookies)
            {
                req.CookieContainer = new CookieContainer();
                foreach (var c in cookies)
                    req.CookieContainer.Add(c);
            }
            if (null != formdata)
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(formdata.ToString());
                using (var reqStream = req.GetRequestStream())
                    reqStream.Write(byteArray, 0, byteArray.Length);
            }
            _logger.Debug($"req.ContentLength={req.ContentLength}");
            using (var res1 = req.GetResponse())
            {
                string cookies_string = res1.Headers["Set-Cookie"];
                if (!string.IsNullOrEmpty(cookies_string))
                {
                    var cookie_str = cookies_string.Split(';');
                    foreach (var c in cookie_str)
                    {
                        _logger.Debug($"c={c}");
                        var ck = c.Split('=');
                        if (ck.Length > 1)
                            respCookies.Add(new Cookie
                            {
                                Name = ck[0].Trim(),
                                Value = ck[1].Trim(),
                                Domain = url.Host,
                                Path ="/",
                            });
                    }
                }
                var stream = res1.GetResponseStream();
                using (var sr = new StreamReader(stream, encode))
                    downloaded_data = sr.ReadToEnd();
            }
            return downloaded_data.Trim();
        }

        public static string GetMyIpAddress()
        {
            var html = DownloadStringData(new Uri("https://www.whatismyip.com.tw/"), Encoding.UTF8, out _);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            var text = doc.DocumentNode.SelectSingleNode("/html/body/b/span").InnerText.Trim();
            return text;
        }
    }
}
