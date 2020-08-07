using Common.Logging;
using System.IO;
using System.Net;
using System.Text;

namespace StockCrawler.Services
{
    public class Tools
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Tools));
        public static string DownloadStringData(string url, Encoding encode, Cookie[] cookies = null)
        {
            _logger.DebugFormat("url=[{0}]", url);
            string downloaded_data = null;
            // https://blog.darkthread.net/blog/disable-tls-1-0-issues
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var req = WebRequest.CreateHttp(url);
            req.Method = "GET";
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            if (null != cookies)
            {
                req.CookieContainer = new CookieContainer();
                foreach (var c in cookies)
                    req.CookieContainer.Add(c);
            }

            using (var res1 = req.GetResponse())
            {
                var stream = res1.GetResponseStream();
                using (var sr = new StreamReader(stream, encode))
                    downloaded_data = sr.ReadToEnd();
            }
            return downloaded_data.Trim();
        }
    }
}
