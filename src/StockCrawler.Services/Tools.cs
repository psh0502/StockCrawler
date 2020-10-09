﻿using Common.Logging;
using HtmlAgilityPack;
using StockCrawler.Dao;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace StockCrawler.Services
{
    public static class Tools
    {
        internal static ILog _logger = LogManager.GetLogger(typeof(Tools));
        static Tools()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
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
            var html = DownloadStringData(new Uri("http://myip.com.tw/"), Encoding.UTF8, out _);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            var text = doc.DocumentNode.SelectSingleNode("/html/body/h1/font").InnerText.Trim();
            return text;
        }

        /// <summary>
        /// 根據本日收盤資料, 計算 均線(MA 移動線)和不同周期的 K 棒
        /// </summary>
        /// <param name="list">今日收盤價</param>
        public static void CalculateMAAndPeriodK(DateTime date)
        {
            _logger.InfoFormat("Begin caculation MA and K ...{0}", date.ToString("yyyyMMdd"));
            using (var db = StockDataServiceProvider.GetServiceInstance())
            {
                var K5_list = new List<GetStockPeriodPriceResult>();
                var K20_list = new List<GetStockPeriodPriceResult>();
                var avgPriceList = new List<(string StockNo, DateTime StockDT, short Period, decimal AveragePrice)>();
                foreach (var d in db.GetStocks().ToList())
                {
                    DateTime target_weekend_date = DateTime.MinValue;
                    DateTime target_monthend_date = DateTime.MinValue;
                    if (target_weekend_date == DateTime.MinValue)
                    {
                        target_weekend_date = date.AddDays(5 - (int)date.DayOfWeek);
                        _logger.Debug($"target_weekend_date:{target_weekend_date:yyyy-MM-dd}");
                    }
                    if (date >= target_weekend_date)
                    {
                        // 週 K
                        DateTime bgnDate = target_weekend_date.AddDays(-4);
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
                        _logger.Debug($"target_weekend_date:{target_weekend_date:yyyy-MM-dd}");
                    }

                    if (target_monthend_date == DateTime.MinValue)
                    {
                        target_monthend_date = new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
                        _logger.Debug($"target_monthend_date:{target_monthend_date:yyyy-MM-dd}");
                    }
                    if (date >= target_monthend_date)
                    {
                        // 月 K
                        DateTime bgnDate = new DateTime(target_monthend_date.Year, target_monthend_date.Month, 1);
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
                        _logger.Debug($"target_monthend_date:{target_monthend_date:yyyy-MM-dd}");
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
                    db.InsertOrUpdateStockPrice(K5_list);
                if (K20_list.Any())
                    db.InsertOrUpdateStockPrice(K20_list);
                // 寫入均價
                if (avgPriceList.Any())
                    db.InsertOrUpdateStockAveragePrice(avgPriceList);
            }
        }
        public static short GetTaiwanYear()
        {
            return GetTaiwanYear(SystemTime.Today.Year);
        }
        public static short GetTaiwanYear(int westernYear)
        {
            return (short)(westernYear - 1911);
        }
        public static short GetSeason()
        {
            return GetSeason(SystemTime.Today.Month);
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
        public static bool IsWeekend(DateTime date)
        {
            return (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday);
        }
    }
}
