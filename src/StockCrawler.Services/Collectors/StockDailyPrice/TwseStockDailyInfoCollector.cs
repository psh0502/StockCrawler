using ServiceStack;
using ServiceStack.Text;
using StockCrawler.Dao;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace StockCrawler.Services.Collectors
{
    internal class TwseStockDailyInfoCollector : TwseCollectorBase, IStockDailyInfoCollector
    {
        private Dictionary<string, GetStockPriceHistoryResult> _stockInfoDictCache = null;
        private Dictionary<string, string> _stockCategoryNo = null;
        private Dictionary<string, long> _categoriedVolume = null;
        public virtual GetStockPriceHistoryResult GetStockDailyPriceInfo(string stockNo, DateTime date)
        {
            InitStockDailyPriceCache(date);
            if (_stockInfoDictCache == null) return null;
            return (_stockInfoDictCache.ContainsKey(stockNo)) ? 
                _stockInfoDictCache[stockNo] : null;
        }
        public virtual IEnumerable<GetStockPriceHistoryResult> GetStockDailyPriceInfo(DateTime date)
        {
            InitStockDailyPriceCache(date);
            if (_stockInfoDictCache == null) return null;
            return _stockInfoDictCache.Select(d => d.Value).ToList();
        }
        private void InitStockDailyPriceCache(DateTime date)
        {
            if (null == _categoriedVolume)
            {
                _categoriedVolume = new Dictionary<string, long>();
                _stockCategoryNo = new Dictionary<string, string>();
                foreach (var d in StockHelper.GetIndexStockList())
                    _categoriedVolume.Add(d.StockNo, 0);

                foreach (var d in StockHelper.GetCompanyStockList()
                    .Where(s => !string.IsNullOrEmpty(s.CategoryNo)))
                    _stockCategoryNo.Add(d.StockNo, d.CategoryNo);
            }

            _logger.Info("Initialize all stock information cache.");
            var data = GetAllStockDailyPriceInfo(date);
            if (null != data) _stockInfoDictCache = data.ToDictionary(d => d.StockNo);
        }
        protected virtual GetStockPriceHistoryResult[] GetAllStockDailyPriceInfo(DateTime day)
        {
            if (day.IsWeekend()) return null;

            var csv_data = DownloadData(day);
            if (string.IsNullOrEmpty(csv_data)) return null;

            // Usage of CsvReader: https://blog.darkthread.net/post-2017-05-13-servicestack-text-csvserializer.aspx
            var csv_lines = CsvReader.ParseLines(csv_data);
            var daily_info = new Dictionary<string, GetStockPriceHistoryResult>();
            bool found_stock_list = false;
            for (int i = 1; i < csv_lines.Count; i++)
            {
                var ln = csv_lines[i];

                string[] data = CsvReader.ParseFields(ln).ToArray();
                GerneralizeNumberFieldData(data);

                if (found_stock_list)
                {
                    if ("備註:" == data[0].Trim()) break;

                    var d = GetParsedStockDailyInfo(day, data);
                    daily_info.Add(d.StockNo, d);
                    if (_stockCategoryNo.ContainsKey(d.StockNo))
                        _categoriedVolume[_stockCategoryNo[d.StockNo]] += d.Volume;
                }
                else
                {
                    if (data[0] == "1.一般股票")
                        _categoriedVolume["0000"] = long.Parse(data[1]);

                    found_stock_list = ("證券代號" == data[0]);
                    if (!found_stock_list && data.Length == 7 && i < 100)
                        foreach (var s in StockHelper.GetIndexStockList())
                            if (data[0].Contains(s.StockName))
                            {
                                var marketIndexStock = GetParsedCategoryMarketIndexData(day, data, s);
                                    daily_info[marketIndexStock.StockNo] = marketIndexStock;
                            }
                }
            }
            if (daily_info.Any())
            {
                foreach (var d in _categoriedVolume)
                    if (!daily_info.ContainsKey(d.Key))
                        daily_info.Add(d.Key, new GetStockPriceHistoryResult() { StockNo = d.Key, Volume = d.Value });
                    else
                        daily_info[d.Key].Volume = d.Value;
            }
            return daily_info.Values.ToArray();
        }
        private GetStockPriceHistoryResult GetParsedStockDailyInfo(DateTime day, string[] data)
        {
            #region csv index comment
            /*
            0. 證券代號
            1. 證券名稱
            2. 成交股數
            3. 成交筆數
            4. 成交金額
            5. 開盤價
            6. 最高價
            7. 最低價
            8. 收盤價
            9. 漲跌(+/-)
            10. 漲跌價差
            11. 最後揭示買價
            12. 最後揭示買量
            13. 最後揭示賣價
            14. 最後揭示賣量
            15. 本益比
            */
            #endregion
            var tmp = new GetStockPriceHistoryResult()
            {
                StockNo = data[0].Replace("=\"", string.Empty).Replace("\"", string.Empty),
                StockName = data[1],
                Volume = long.Parse(data[2]),
                StockDT = day,
                OpenPrice = decimal.Parse(data[5]),
                HighPrice = decimal.Parse(data[6]),
                LowPrice = decimal.Parse(data[7]),
                ClosePrice = decimal.Parse(data[8]),
                DeltaPrice = decimal.Parse(data[9] + data[10]),
                PE = decimal.Parse(data[15]),
            };
            var previousClosePrice = tmp.ClosePrice - tmp.DeltaPrice;
            // 取小數點下四位就好
            tmp.DeltaPercent = (previousClosePrice == 0) ? 0 : decimal.Parse((tmp.DeltaPrice / previousClosePrice).ToString("0.####"));
            return tmp;
        }
        /// <summary>
        /// 解析每日大盤與類股指數資訊
        /// </summary>
        /// <param name="day">日期</param>
        /// <param name="data"></param>
        /// <param name="s"></param>
        /// <returns>類股指數資訊</returns>
        private static GetStockPriceHistoryResult GetParsedCategoryMarketIndexData(DateTime day, string[] data, GetStocksResult s)
        {
            var d = new GetStockPriceHistoryResult()
            {
                StockNo = s.StockNo,
                StockName = s.StockName,
                Volume = 0,
                StockDT = day,
                OpenPrice = decimal.Parse(data[1]),
                HighPrice = decimal.Parse(data[1]),
                LowPrice = decimal.Parse(data[1]),
                ClosePrice = decimal.Parse(data[1]),
                DeltaPrice = decimal.Parse(data[2] + data[3]),
                DeltaPercent = decimal.Parse(data[4]) / 100
            };
            d.OpenPrice = d.ClosePrice - d.DeltaPrice;
            d.HighPrice = Math.Max(d.ClosePrice, d.OpenPrice);
            d.LowPrice = Math.Min(d.ClosePrice, d.OpenPrice);

            return d;
        }
        protected virtual string DownloadData(DateTime day)
        {
            while (true) // retry till it get
                try
                {
                    var csv_data = Tools.DownloadStringData(
                        new Uri($"https://www.twse.com.tw/exchangeReport/MI_INDEX?response=csv&date={day:yyyyMMdd}&type=ALLBUT0999"),                         
                        out IList<Cookie> _,
                        Encoding.Default);

                    if (string.IsNullOrEmpty(csv_data))
                    {
                        _logger.WarnFormat("Download has no data by date[{0}]", day.ToShortDateString());
                        return null;
                    }
#if (DEBUG)
                    var file = new FileInfo($@"..\..\..\StockCrawler.UnitTest\TestData\TWSE\{GetType().Name}\MI_INDEX_ALLBUT0999_{day:yyyyMMdd}.csv");
                    if (!file.Directory.Exists) file.Directory.Create();
                    if (file.Exists) file.Delete();
                    using (var sw = file.CreateText())
                        sw.Write(csv_data);
#endif
                    return csv_data;
                }
                catch (WebException)
                {
                    _logger.WarnFormat("Target website refuses our connection. Wait till it get peace. day={0}", day.ToString("yyyy-MM-dd"));
                    Thread.Sleep((int)new TimeSpan(1, 0, 0).TotalMilliseconds);
                }
        }
    }
}
