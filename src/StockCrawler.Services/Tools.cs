using Common.Logging;
using HtmlAgilityPack;
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
                byte[] byteArray = Encoding.UTF8.GetBytes(formdata.ToString());
                req.ContentLength = byteArray.Length;
                using (var reqStream = req.GetRequestStream())
                    reqStream.Write(byteArray, 0, byteArray.Length);
            }
            using (var res1 = req.GetResponse())
            {
                string cookies_string = res1.Headers["Set-Cookie"];
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
                var stream = res1.GetResponseStream();
                using (var sr = new StreamReader(stream, encode))
                    downloaded_data = sr.ReadToEnd();
            }
            return downloaded_data.Trim();
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
        /// 根據本日收盤資料, 計算 均線(MA 移動線)和不同周期的 K 棒
        /// </summary>
        /// <param name="list">今日收盤價</param>
        public static void CalculateMAAndPeriodK(DateTime date)
        {
            _logger.InfoFormat("Begin caculation MA and K ...{0}", date.ToString("yyyyMMdd"));
            using (var db = RepositoryProvider.GetRepositoryInstance())
            {
                var K5_list = new List<GetStockPeriodPriceResult>();
                var K20_list = new List<GetStockPeriodPriceResult>();
                var avgPriceList = new List<(string StockNo, DateTime StockDT, short Period, decimal AveragePrice)>();
                foreach (var d in db.GetStocks().ToList())
                {
                    var target_weekend_date = DateTime.MinValue;
                    var target_monthend_date = DateTime.MinValue;
                    if (target_weekend_date == DateTime.MinValue)
                    {
                        target_weekend_date = date.AddDays(5 - (int)date.DayOfWeek);
                        _logger.Debug($"[{d.StockNo}]target_weekend_date:{target_weekend_date:yyyy-MM-dd}");
                    }
                    if (date >= target_weekend_date)
                    {
                        // 週 K
                        var bgnDate = target_weekend_date.AddDays(-4);
                        var data = db.GetStockPeriodPrice(d.StockNo, 1, bgnDate, target_weekend_date).ToList();
                        if (data.Any())
                        {
                            var tmp = new GetStockPeriodPriceResult()
                            {
                                StockNo = d.StockNo,
                                StockDT = bgnDate,
                                OpenPrice = data.OrderBy(x => x.StockDT).First().OpenPrice,
                                ClosePrice = data.OrderByDescending(x => x.StockDT).First().ClosePrice,
                                HighPrice = data.Max(x => x.HighPrice),
                                LowPrice = data.Min(x => x.LowPrice),
                                Volume = data.Sum(x => x.Volume),
                                Period = 5,
                            };
                            if (tmp.Volume > 0) K5_list.Add(tmp);
                        }

                        target_weekend_date = target_weekend_date.AddDays(7);
                        _logger.Debug($"[{d.StockNo}]target_weekend_date:{target_weekend_date:yyyy-MM-dd}");
                    }

                    if (target_monthend_date == DateTime.MinValue)
                    {
                        target_monthend_date = new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
                        _logger.Debug($"[{d.StockNo}]target_monthend_date:{target_monthend_date:yyyy-MM-dd}");
                    }
                    if (date >= target_monthend_date)
                    {
                        // 月 K
                        var bgnDate = new DateTime(target_monthend_date.Year, target_monthend_date.Month, 1);
                        var data = db.GetStockPeriodPrice(d.StockNo, 1, bgnDate, target_monthend_date).ToList();
                        if (data.Any())
                        {
                            var tmp = new GetStockPeriodPriceResult()
                            {
                                StockNo = d.StockNo,
                                StockDT = bgnDate,
                                OpenPrice = data.OrderBy(x => x.StockDT).First().OpenPrice,
                                ClosePrice = data.OrderByDescending(x => x.StockDT).First().ClosePrice,
                                HighPrice = data.Max(x => x.HighPrice),
                                LowPrice = data.Min(x => x.LowPrice),
                                Volume = data.Sum(x => x.Volume),
                                Period = 20,
                            };
                            if (tmp.Volume > 0) K20_list.Add(tmp);
                        }
                        target_monthend_date = bgnDate.AddMonths(2).AddDays(-1);
                        _logger.Debug($"[{d.StockNo}]target_monthend_date:{target_monthend_date:yyyy-MM-dd}");
                    }
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
                }
                // 寫入 K 線棒
                if (K5_list.Any())
                    db.InsertOrUpdateStockPrice(K5_list.ToArray());
                if (K20_list.Any())
                    db.InsertOrUpdateStockPrice(K20_list.ToArray());
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
        /// 判斷該日期是否為周末六日假日
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>是否</returns>
        public static bool IsWeekend(this DateTime date)
        {
            return (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday);
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
