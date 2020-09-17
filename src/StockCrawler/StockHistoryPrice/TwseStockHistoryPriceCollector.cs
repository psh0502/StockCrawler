using ServiceStack.Text;
using StockCrawler.Dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace StockCrawler.Services.Collectors
{
    internal class TwseStockHistoryPriceCollector : TwseCollectorBase, IStockHistoryPriceCollector
    {
        public virtual IEnumerable<GetStockPriceHistoryResult> GetStockDailyPriceInfo(string stockNo, DateTime bgnDate, DateTime endDate)
        {
            List<GetStockPriceHistoryResult> result = new List<GetStockPriceHistoryResult>();
            for (int year = bgnDate.Year; year <= endDate.Year; year++)
                for (int month = 1; month <= 12; month++)
                {
                    if (new DateTime(year, month, 1) > endDate) break;
                    var r = GetStockDailyPriceInfo(stockNo, year, month);
                    if (null != r)
                        result.AddRange(r);
                    else
                        return result;

                    Thread.Sleep(_breakInternval);
                }

            return result;
        }
        private static GetStockPriceHistoryResult[] GetStockDailyPriceInfo(string stockNo, int year, int month)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var csv_data = Tools.DownloadStringData(new Uri($"https://www.twse.com.tw/exchangeReport/STOCK_DAY?response=csv&date={year}{month:00}01&stockNo={stockNo}"), Encoding.Default, out IList<Cookie> _);
            if (string.IsNullOrEmpty(csv_data))
            {
                _logger.WarnFormat("Download stock[{0}] has no data by date[{1}]", stockNo, new DateTime(year, month, 1).ToString("yyyy-MM"));
                return null;
            }
            // Usage of CsvReader: https://blog.darkthread.net/post-2017-05-13-servicestack-text-csvserializer.aspx
            var csv_lines = CsvReader.ParseLines(csv_data);
            var daily_info = new List<GetStockPriceHistoryResult>();
            bool found_stock_list = false;
            foreach (var ln in csv_lines)
            {
                // 日期	成交股數	成交金額	開盤價	最高價	最低價	收盤價	漲跌價差	成交筆數
                string[] data = CsvReader.ParseFields(ln).ToArray();
                if (found_stock_list)
                {
                    if ("說明:" == data[0].Trim())
                    {
                        found_stock_list = false;
                        break;
                    }
                    // Generalize number fields data
                    for (int i = 0; i < data.Length; i++)
                        if (!string.IsNullOrEmpty(data[i]))
                            data[i] = data[i].Replace("--", "0").Replace(",", string.Empty);

                    var tmp = data[0].Split('/').Select(int.Parse).ToList();
                    _logger.Debug("ln: " + ln);
                    daily_info.Add(new GetStockPriceHistoryResult()
                    {
                        StockNo = stockNo,
                        Volume = long.Parse(data[1]) / 1000,
                        StockDT = new DateTime(tmp[0] + 1911, tmp[1], tmp[2]),
                        OpenPrice = decimal.Parse(data[3]),
                        HighPrice = decimal.Parse(data[4]),
                        LowPrice = decimal.Parse(data[5]),
                        ClosePrice = decimal.Parse(data[6])
                    });
                }
                else
                {
                    if ("日期" == data[0])
                        found_stock_list = true;
                }
            }
            return daily_info.ToArray();
        }
    }
}
