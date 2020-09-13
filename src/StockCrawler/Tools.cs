using Common.Logging;
using StockCrawler.Dao;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
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
            return DownloadStringData(new Uri("http://www.comeondata.com/App/api/IpLocApi/GetMyIpInfo"), Encoding.UTF8, out _).Replace("\"", string.Empty);

            //var html = DownloadStringData(new Uri("https://www.whatismyip.com.tw/"), Encoding.UTF8, out _);
            //HtmlDocument doc = new HtmlDocument();
            //doc.LoadHtml(html);
            //var text = doc.DocumentNode.SelectSingleNode("/html/body/b/span").InnerText.Trim();
            //return text;
        }

        /// <summary>
        /// 根據本日收盤資料, 計算 均線(MA 移動線)和不同周期的 K 棒
        /// </summary>
        /// <param name="list">今日收盤價</param>
        public static void CalculateMAAndPeriodK(IEnumerable<GetStockPriceHistoryResult> list)
        {
            using (var db = StockDataServiceProvider.GetServiceInstance())
            {
                // 寫入日價
                db.InsertOrUpdateStockPriceHistory(list);
                var K5_list = new List<GetStockPriceHistoryResult>();
                var K20_list = new List<GetStockPriceHistoryResult>();
                var avgPriceList = new List<(string StockNo, DateTime StockDT, short Period, decimal AveragePrice)>();
                DateTime target_weekend_date = DateTime.MinValue;
                DateTime target_monthend_date = DateTime.MinValue;
                foreach (var d in list)
                {
                    if (target_weekend_date == DateTime.MinValue) 
                        target_weekend_date = d.StockDT.AddDays(5 - (int)d.StockDT.DayOfWeek);

                    if (d.StockDT >= target_weekend_date)
                    {
                        // 週 K
                        DateTime bgnDate = target_weekend_date.AddDays(-4);
                        var data = db.GetStockPeriodPrice(d.StockNo, bgnDate, target_weekend_date).ToList();
                        if (data.Any())
                            K5_list.Add(new GetStockPriceHistoryResult()
                            {
                                StockNo = d.StockNo,
                                StockDT = bgnDate,
                                OpenPrice = data.OrderBy(x => x.StockDT).First().OpenPrice,
                                ClosePrice = data.OrderByDescending(x => x.StockDT).First().ClosePrice,
                                HighPrice = data.Max(x => x.HighPrice),
                                LowPrice = data.Min(x => x.LowPrice),
                                Volume = data.Sum(x => x.Volume),
                                Period = 5,
                                AdjClosePrice = 0
                            });

                        target_weekend_date = target_weekend_date.AddDays(7);
                    }

                    if (target_monthend_date == DateTime.MinValue)
                        target_monthend_date = new DateTime(d.StockDT.Year, d.StockDT.Month, 1).AddMonths(1).AddDays(-1);

                    if (d.StockDT >= target_monthend_date)
                    {
                        // 月 K
                        DateTime bgnDate = new DateTime(target_monthend_date.Year, target_monthend_date.Month, 1);
                        var data = db.GetStockPeriodPrice(d.StockNo, bgnDate, target_monthend_date).ToList();
                        if (data.Any())
                            K20_list.Add(new GetStockPriceHistoryResult()
                            {
                                StockNo = d.StockNo,
                                StockDT = bgnDate,
                                OpenPrice = data.OrderBy(x => x.StockDT).First().OpenPrice,
                                ClosePrice = data.OrderByDescending(x => x.StockDT).First().ClosePrice,
                                HighPrice = data.Max(x => x.HighPrice),
                                LowPrice = data.Min(x => x.LowPrice),
                                Volume = data.Sum(x => x.Volume),
                                Period = 20,
                                AdjClosePrice = 0
                            });
                        target_monthend_date = bgnDate.AddMonths(1).AddDays(-1);
                    }
                    {
                        // 週線
                        var data = db.GetStockPriceAVG(d.StockNo, d.StockDT, 5);
                        avgPriceList.Add((d.StockNo, d.StockDT, 5, data));
                        // 雙週線
                        data = db.GetStockPriceAVG(d.StockNo, d.StockDT, 10);
                        avgPriceList.Add((d.StockNo, d.StockDT, 10, data));
                        // 月線
                        data = db.GetStockPriceAVG(d.StockNo, d.StockDT, 20);
                        avgPriceList.Add((d.StockNo, d.StockDT, 20, data));
                        // 季線
                        data = db.GetStockPriceAVG(d.StockNo, d.StockDT, 60);
                        avgPriceList.Add((d.StockNo, d.StockDT, 60, data));
                    }
                }
                // 寫入 K 線棒
                if (K5_list.Any())
                    db.InsertOrUpdateStockPriceHistory(K5_list);
                if (K20_list.Any())
                    db.InsertOrUpdateStockPriceHistory(K20_list);
                // 寫入均價
                if (avgPriceList.Any())
                    db.InsertOrUpdateStockAveragePrice(avgPriceList);
            }
        }
    }
}
