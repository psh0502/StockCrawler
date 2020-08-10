using Common.Logging;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace StockCrawler.Services
{
    public class Tools
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Tools));
        public static string DownloadStringData(string url, Encoding encode, out Cookie[] respCookies, Cookie[] cookies = null, string method = "GET", Dictionary<string,string> formdata = null, string refer = null)
        {
            _logger.DebugFormat("url=[{0}]", url);
            respCookies = new Cookie[0];
            string downloaded_data = null;
            // https://blog.darkthread.net/blog/disable-tls-1-0-issues
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var req = WebRequest.CreateHttp(url);
            req.Method = method;
            req.ContentType = "application/x-www-form-urlencoded";
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
            req.Accept = "*.*";
            req.Referer = refer;
            req.Headers.Add("Accept-Language", "zh-TW,zh;q=0.9,en-US;q=0.8,en;q=0.7");
            req.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            if (null != cookies)
            {
                req.CookieContainer = new CookieContainer();
                foreach (var c in cookies)
                    req.CookieContainer.Add(c);
            }
            if (null != formdata)
            {
                NameValueCollection postParams = new NameValueCollection();
                foreach (var data in formdata)
                    postParams.Add(data.Key, data.Value);

                byte[] byteArray = Encoding.UTF8.GetBytes(postParams.ToString());
                using (var reqStream = req.GetRequestStream())
                    reqStream.Write(byteArray, 0, byteArray.Length);
            }
            using (var res1 = req.GetResponse())
            {
                string cookies_string = res1.Headers["Set-Cookie"];
                if (!string.IsNullOrEmpty(cookies_string))
                {
                    var cookie_str = cookies_string.Split(';')[0];
                    respCookies = new Cookie[] {
                        new Cookie() {
                            Name = cookie_str.Split('=')[0],
                            Value = cookie_str.Split('=')[1] }
                    };                    
                }
                var stream = res1.GetResponseStream();
                using (var sr = new StreamReader(stream, encode))
                    downloaded_data = sr.ReadToEnd();
            }
            return downloaded_data.Trim();
        }

        public static string GetMyIpAddress()
        {
            var html = DownloadStringData("https://www.whatismyip.com.tw/", Encoding.UTF8, out _);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            var text = doc.DocumentNode.SelectSingleNode("/html/body/b/span").InnerText.Trim();
            return text;
        }
    }
}
