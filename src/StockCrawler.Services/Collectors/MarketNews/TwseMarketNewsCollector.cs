using ServiceStack.Text;
using StockCrawler.Dao;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace StockCrawler.Services.Collectors
{
    internal class TwseMarketNewsCollector : TwseCollectorBase, IMarketNewsCollector
    {
        public GetMarketNewsResult[] GetLatestNews()
        {
            var csv_data = DownloadData();
            if (string.IsNullOrEmpty(csv_data)) return null;

            _logger.InfoFormat("csv={0}", csv_data.Substring(0, 1000));
            // Usage of CsvReader: https://blog.darkthread.net/post-2017-05-13-servicestack-text-csvserializer.aspx
            List<GetMarketNewsResult> list = new List<GetMarketNewsResult>();
            var csv_lines = CsvReader.ParseLines(csv_data);
            for (int i = 2; i < csv_lines.Count; i++)
            {
                var ln = csv_lines[i];
                string[] data = CsvReader.ParseFields(ln).ToArray();
                if (data.Length == 3)
                {
                    GerneralizeNumberFieldData(data);
                    list.Add(ParseMarketNewsData(data));
                }
            }
            return list.ToArray();
        }

        private static GetMarketNewsResult ParseMarketNewsData(string[] data)
        {
            return new GetMarketNewsResult() {
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

        protected virtual string DownloadData()
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
                    //var file = new FileInfo($"D:\\tmp\\{DateTime.Today:yyyy-MM-dd}.csv");
                    //if (file.Exists) file.Delete();
                    //using (var sw = file.CreateText())
                    //    sw.Write(csv_data);
#endif
                    return csv_data;
                }
                catch (WebException)
                {
                    _logger.Warn("Target website refuses our connection. Wait till it get peace.");
                    Thread.Sleep((int)new TimeSpan(1, 0, 0).TotalMilliseconds);
                }
        }
    }
}
