using Common.Logging;
using StockCrawler.Dao;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace StockCrawler.Services
{
    /// <summary>
    /// 瑞士小刀馬蓋仙
    /// </summary>
    public static class Tools
    {
        private static readonly string UTF8SpacingChar = Encoding.UTF8.GetString(new byte[] { 0xC2, 0xA0 });
        internal static ILog _logger = LogManager.GetLogger(typeof(Tools));
        static Tools()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        /// <summary>
        /// 網路資料下載萬用工法
        /// </summary>
        /// <param name="url">網址, 若要帶入 query 參數請直接串好送入</param>
        /// <param name="respCookies">網站回應輸出的 cookies</param>
        /// <param name="encode">資料採用的編碼頁, 若不指定, 預設為 UTF8</param>
        /// <param name="contentType">要求的內容類型</param>
        /// <param name="cookies">要送出去的 cookies</param>
        /// <param name="method">使用哪種呼叫方法 GET POST DELETE UPDATE</param>
        /// <param name="formdata">若是要採用 form post 方式, 請提供</param>
        /// <param name="refer">呼叫來源</param>
        /// <returns>下載到的字串資料</returns>
        public static string DownloadStringData(
            Uri url, 
            out IList<Cookie> respCookies,
            Encoding encode = null,
            string contentType = null, 
            IList<Cookie> cookies = null, 
            string method = "GET", 
            NameValueCollection formdata = null, 
            string refer = null)
        {
            _logger.DebugFormat("url=[{0}]", url.OriginalString);
            if (null == encode) encode = Encoding.UTF8;
            respCookies = new List<Cookie>();
            string downloaded_data = null;
            // https://blog.darkthread.net/blog/disable-tls-1-0-issues
            var req = WebRequest.CreateHttp(url);
            req.Method = method;
            if (!string.IsNullOrEmpty(contentType)) req.ContentType = contentType;
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
            req.Referer = refer;
            req.ContentLength = 0;
            if (null != cookies)
            {
                req.CookieContainer = new CookieContainer();
                foreach (var c in cookies)
                    req.CookieContainer.Add(c);
            }
            if (null != formdata)
            {
                var byteArray = Encoding.UTF8.GetBytes(formdata.ToString());
                req.ContentLength = byteArray.Length;
                using (var reqStream = req.GetRequestStream())
                    reqStream.Write(byteArray, 0, byteArray.Length);
            }
            using (var res = req.GetResponse())
            {
                var target = res.Headers["Target"];
                var redirect = !string.IsNullOrEmpty(target);
                if (redirect)
                {
                    return DownloadStringData(
                        new Uri(target), 
                        out respCookies, 
                        encode, 
                        contentType, 
                        cookies,
                        method,
                        formdata,
                        refer);
                }
                else
                {
                    var cookies_string = res.Headers["Set-Cookie"];
                    if (!string.IsNullOrEmpty(cookies_string))
                    {
                        var cookie_str = cookies_string.Split(';');
                        foreach (var c in cookie_str)
                        {
                            var ck = c.Split('=');
                            if (ck.Length > 1)
                                try
                                {
                                    respCookies.Add(new Cookie
                                    {
                                        Name = ck[0].Trim(),
                                        Value = ck[1].Trim(),
                                        Domain = url.Host,
                                        Path = "/",
                                    });
                                }
                                catch (Exception e)
                                {
                                    _logger.Warn(e.Message, e);
                                }
                        }
                    }
                    var stream = res.GetResponseStream();
                    var respEncode = (res.ContentType.Contains("big5") ? Encoding.Default : encode);
                    using (var sr = new StreamReader(stream, respEncode))
                        downloaded_data = sr.ReadToEnd();
                    return downloaded_data.Trim();
                }
            }
        }
        /// <summary>
        /// 服務提供 by 湯湯數據庫
        /// </summary>
        /// <returns></returns>
        public static string GetMyIpAddress()
        {
            return DownloadStringData(new Uri("http://www.comeondata.com/App/api/IpLocApi/GetMyIpInfo"), out _)
                .Replace("\"", string.Empty);
        }
        /// <summary>
        /// 根據本日收盤資料, 計算 均線(MA 移動線)
        /// </summary>
        /// <param name="list">今日收盤價</param>
        public static void CalculateMA(DateTime date)
        {
            _logger.InfoFormat("Begin caculation MA...{0}", date.ToString("yyyyMMdd"));
            var avgPriceList = new List<(string StockNo, DateTime StockDT, short Period, decimal AveragePrice)>();
            using (var db = RepositoryProvider.GetRepositoryInstance())
            {
                foreach (var d in StockHelper.GetAllStockList())
                {
                    // 週線
                    var period_price = db.CaculateStockClosingAveragePrice(d.StockNo, date, 5);
                    if (period_price > 0) avgPriceList.Add((d.StockNo, date, 5, period_price));
                    // 雙週線
                    period_price = db.CaculateStockClosingAveragePrice(d.StockNo, date, 10);
                    if (period_price > 0) avgPriceList.Add((d.StockNo, date, 10, period_price));
                    // 月線
                    period_price = db.CaculateStockClosingAveragePrice(d.StockNo, date, 20);
                    if (period_price > 0) avgPriceList.Add((d.StockNo, date, 20, period_price));
                    // 季線
                    period_price = db.CaculateStockClosingAveragePrice(d.StockNo, date, 60);
                    if (period_price > 0) avgPriceList.Add((d.StockNo, date, 60, period_price));
                }
                // 寫入均價
                if (avgPriceList.Any())
                    db.InsertOrUpdateStockAveragePrice(avgPriceList.ToArray());
            }
        }
        /// <summary>
        /// 取得今日的台灣民國年
        /// </summary>
        /// <returns>回傳今日的台灣民國年</returns>
        public static short GetTaiwanYear()
        {
            return GetTaiwanYear(SystemTime.Today.Year);
        }
        public static short GetTaiwanYear(int westernYear)
        {
            return (short)(westernYear - 1911);
        }
        /// <summary>
        /// 取得日期對應的四季(Q?)
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>季</returns>
        public static short GetSeason(this DateTime date)
        {
            return GetSeason(date.Month);
        }
        public static short GetSeason(int month)
        {
            return (short)(month / 3 + (month % 3 == 0 ? 0 : 1));
        }
        /// <summary>
        /// 取得台灣的中華民國年
        /// </summary>
        /// <param name="date">西園日期物件</param>
        /// <returns>中華民國年</returns>
        public static short GetTaiwanYear(this DateTime date)
        {
            return GetTaiwanYear(date.Year);
        }
        public static DateTime AddSeason(this DateTime date, short season)
        {
            var current_season = GetSeason(date);
            return new DateTime(date.Year, current_season * 3, 1).AddMonths(3 * season);
        }
        /// <summary>
        /// 清理字串內容不必要的垃圾字元
        /// </summary>
        /// <param name="text">參雜垃圾字元的字串</param>
        /// <returns>乾淨的字元</returns>
        public static string CleanString(string text)
        {
            return HttpUtility.HtmlDecode(text)
                .Replace("&nbsp;", string.Empty)
                .Replace(" ", string.Empty)
                .Replace(Environment.NewLine, string.Empty)
                .Replace(UTF8SpacingChar, string.Empty)
                .Trim();
        }
        public static string GenerateMD5Hash(string text, string salt = null)
        {
            HashAlgorithm md5;
            if (string.IsNullOrEmpty(salt))
                md5 = MD5.Create();
            else
                md5 = new HMACMD5(Encoding.UTF8.GetBytes(salt));

            using (md5)
                return Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(text)));
        }
    }
}
