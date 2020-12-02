using HtmlAgilityPack;
using ServiceStack.Text;
using StockCrawler.Dao;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace StockCrawler.Services.Collectors
{
    internal class TwseMarketNewsCollector : TwseCollectorBase, IStockMarketNewsCollector
    {
        public virtual GetStockMarketNewsResult[] GetLatestNews()
        {
            var csv_data = DownloadTwseData();
            if (string.IsNullOrEmpty(csv_data)) return null;

            _logger.DebugFormat("csv={0}", csv_data.Substring(0, 1000));
            // Usage of CsvReader: https://blog.darkthread.net/post-2017-05-13-servicestack-text-csvserializer.aspx
            var list = new List<GetStockMarketNewsResult>();
            var csv_lines = CsvReader.ParseLines(csv_data);
            for (int i = 2; i < csv_lines.Count; i++)
            {
                var ln = csv_lines[i];
                string[] data = CsvReader.ParseFields(ln).ToArray();
                if (data.Length == 3)
                {
                    GerneralizeNumberFieldData(data);
                    list.Add(ParseStockMarketNewsData("0000", "twse", data));
                }
            }
            return list.ToArray();
        }
        private static GetStockMarketNewsResult ParseStockMarketNewsData(string stockNo, string source, string[] data)
        {
            return new GetStockMarketNewsResult() {
                StockNo = stockNo,
                Source = source,
                Subject = data[0],
                Url = data[1],
                NewsDate = ParseTaiwanDate(data[2])
            };
        }
        private static DateTime ParseTaiwanDate(string v)
        {
            //e.g 109年01月20日
            int year = int.Parse(v.Substring(0, 3));
            year += 1911;
            int month = int.Parse(v.Substring(4, 2));
            int day = int.Parse(v.Substring(7, 2));
            return new DateTime(year, month, day);
        }
        protected virtual string DownloadTwseData()
        {
            while (true) // retry till it get
                try
                {
                    var csv_data = Tools.DownloadStringData(new Uri($"https://www.twse.com.tw/news/newsList?response=csv&keyword=&startYear=&endYear=&lang=zh"), Encoding.Default, out IList<Cookie> _);
                    if (string.IsNullOrEmpty(csv_data))
                    {
                        _logger.WarnFormat("Download has no market news.");
                        return null;
                    }
#if (DEBUG)
                    var file = new FileInfo($"D:\\tmp\\{DateTime.Today:yyyy-MM-dd}.csv");
                    if (file.Exists) file.Delete();
                    using (var sw = file.CreateText())
                        sw.Write(csv_data);
#endif
                    return csv_data;
                }
                catch (WebException)
                {
                    _logger.Warn("Target website refuses our connection. Wait till it get peace.");
                    Thread.Sleep((int)new TimeSpan(1, 0, 0).TotalMilliseconds);
                }
        }
        protected virtual string DownloadMopsData()
        {
            while (true) // retry till it get
                try
                {
                    var html = Tools.DownloadStringData(new Uri("https://mops.twse.com.tw/mops/web/ajax_t05sr01_1"), Encoding.UTF8, out IList<Cookie> _);
                    if (string.IsNullOrEmpty(html))
                    {
                        _logger.WarnFormat("Download has no stock news.");
                        return null;
                    }
#if (DEBUG)
                    var file = new FileInfo($"D:\\tmp\\{DateTime.Today:yyyy-MM-dd}.html");
                    if (file.Exists) file.Delete();
                    if (!file.Directory.Exists) file.Directory.Create();
                    using (var sw = file.CreateText())
                        sw.Write(html);
#endif
                    return html;
                }
                catch (WebException)
                {
                    _logger.Warn("Target website refuses our connection. Wait till it get peace.");
                    Thread.Sleep((int)new TimeSpan(1, 0, 0).TotalMilliseconds);
                }
        }
        public virtual GetStockMarketNewsResult[] GetLatestStockNews()
        {
            var html = DownloadMopsData();
            if (string.IsNullOrEmpty(html)) return null;

            _logger.DebugFormat("html={0}", html.Substring(0, 1000));
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var data_nodes = doc.DocumentNode.SelectNodes("/html/body/form/table/tr");
            var list = new List<GetStockMarketNewsResult>();
            for (int i = 1; i < data_nodes.Count; i++)
            {
                var data = data_nodes[i].SelectNodes("td");
                if (null != data && data.Count == 6)
                    list.Add(new GetStockMarketNewsResult()
                    {
                        StockNo = CleanData(data[2].InnerText),
                        Source = "mops",
                        Subject = CleanData(data[4].InnerText),
                        NewsDate = DateTime.Parse(ParseTaiwanDate(CleanData(data[0].InnerText)).ToShortDateString() + " " + CleanData(data[1].InnerText)),
                        Url = ""
                    });
            }
            return list.ToArray();
        }
        private static string CleanData(string text)
        {
            return text
                .Replace("&nbsp;", string.Empty)
                .Replace(" ", string.Empty)
                .Replace(Environment.NewLine, string.Empty)
                .Trim();
        }
    }
}
