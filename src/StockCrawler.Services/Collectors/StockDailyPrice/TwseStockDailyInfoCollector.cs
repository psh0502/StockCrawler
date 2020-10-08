using ServiceStack.Text;
using StockCrawler.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace StockCrawler.Services.Collectors
{
    internal class TwseStockDailyInfoCollector : TwseCollectorBase, IStockDailyInfoCollector
    {
        private Dictionary<string, GetStockPeriodPriceResult> _stockInfoDictCache = null;
        public virtual GetStockPeriodPriceResult GetStockDailyPriceInfo(string stockNo)
        {
            InitStockDailyPriceCache();
            return (_stockInfoDictCache.ContainsKey(stockNo)) ? _stockInfoDictCache[stockNo] : null;
        }
        private void InitStockDailyPriceCache()
        {
            if (null == _stockInfoDictCache)
                lock (this)
                    if (null == _stockInfoDictCache)
                    {
                        _logger.Info("Initialize all stock information cache.");
                        _stockInfoDictCache = new Dictionary<string, GetStockPeriodPriceResult>();
                        if (!Tools.IsWeekend(SystemTime.Today))
                        {
                            var data = GetAllStockDailyPriceInfo(SystemTime.Today, out long totalVolume);
                            if (null != data)
                                foreach (var info in data)
                                {
                                    if (info.StockNo == "0000") info.Volume = totalVolume;
                                    _stockInfoDictCache[info.StockNo] = info;
                                    _logger.DebugFormat("[{0}] {1}", info.StockNo, info.ClosePrice);
                                }

                        }
                    }
        }
        protected virtual GetStockPeriodPriceResult[] GetAllStockDailyPriceInfo(DateTime day, out long totalVolume)
        {
            totalVolume = 0;
            if (Tools.IsWeekend(day)) return null;

            var csv_data = DownloadData(day);
            if (string.IsNullOrEmpty(csv_data)) return null;

            _logger.InfoFormat("Day={1}, csv={0}", csv_data.Substring(0, 1000), day.ToShortDateString());
            // Usage of CsvReader: https://blog.darkthread.net/post-2017-05-13-servicestack-text-csvserializer.aspx
            var csv_lines = CsvReader.ParseLines(csv_data);
            var daily_info = new List<GetStockPeriodPriceResult>();
            bool found_stock_list = false;
            for (int i = 1; i < csv_lines.Count; i++)
            {
                var ln = csv_lines[i];

                string[] data = CsvReader.ParseFields(ln).ToArray();
                GerneralizeNumberFieldData(data);

                if (found_stock_list)
                {
                    if ("備註:" == data[0].Trim())
                    {
                        break;
                    }var d = GetParsedStockDailyInfo(day, data);
                    daily_info.Add(d);
                    totalVolume += d.Volume;
                }
                else
                {
                    found_stock_list = ("證券代號" == data[0]);
                    if (!found_stock_list && data.Length == 7 && i < 100)
                        foreach (var s in GetCategoryStockList())
                            if (data[0].Contains(s.StockName))
                                daily_info.Add(GetParsedCategoryMarketIndexData(day, data, s));
                }
            }
            return daily_info.ToArray();
        }
        private static void GerneralizeNumberFieldData(string[] data)
        {
            // Generalize number fields data
            for (int i = 0; i < data.Length; i++)
                if (!string.IsNullOrEmpty(data[i]))
                    data[i] = data[i]
                        .Replace("--", "0")
                        .Replace(",", string.Empty)
                        .Replace("X", string.Empty)
                        .Trim();
        }
        private GetStockPeriodPriceResult GetParsedStockDailyInfo(DateTime day, string[] data)
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
            var tmp = new GetStockPeriodPriceResult()
            {
                StockNo = data[0].Replace("=\"", string.Empty).Replace("\"", string.Empty),
                StockName = data[1],
                Period = 1,
                Volume = long.Parse(data[2]),
                StockDT = day,
                OpenPrice = decimal.Parse(data[5]),
                HighPrice = decimal.Parse(data[6]),
                LowPrice = decimal.Parse(data[7]),
                ClosePrice = decimal.Parse(data[8]),
                DeltaPrice = decimal.Parse(data[9] + data[10]),
                PE = decimal.Parse(data[15]),
            };
            // 取小數點下四位就好
            tmp.DeltaPercent = (tmp.OpenPrice == 0) ? 0 : decimal.Parse((tmp.DeltaPrice / tmp.OpenPrice).ToString("0.####"));
            return tmp;
        }
        /// <summary>
        /// 解析每日大盤與類股指數資訊
        /// </summary>
        /// <param name="day">日期</param>
        /// <param name="data"></param>
        /// <param name="s"></param>
        /// <returns>類股指數資訊</returns>
        private static GetStockPeriodPriceResult GetParsedCategoryMarketIndexData(DateTime day, string[] data, GetStocksResult s)
        {
            var d = new GetStockPeriodPriceResult()
            {
                StockNo = s.StockNo,
                StockName = s.StockName,
                Period = 1,
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
        /// <summary>
        /// 取出大盤和類股指數清單
        /// </summary>
        /// <returns>大盤和類股指數清單</returns>
        protected virtual IList<GetStocksResult> GetCategoryStockList()
        {
            using(var db = StockDataServiceProvider.GetServiceInstance())
                return db.GetStocks()
                    .Where(d => int.TryParse(d.StockNo, out int _) && int.Parse(d.StockNo) < 50)
                    .ToList();
        }
        protected virtual string DownloadData(DateTime day)
        {
            while (true) // retry till it get
                try
                {
                    var csv_data = Tools.DownloadStringData(new Uri($"https://www.twse.com.tw/exchangeReport/MI_INDEX?response=csv&date={day:yyyyMMdd}&type=ALLBUT0999"), Encoding.Default, out IList<Cookie> _);
                    if (string.IsNullOrEmpty(csv_data))
                    {
                        _logger.WarnFormat("Download has no data by date[{0}]", day.ToString("yyyyMMdd"));
                        return null;
                    }
#if (DEBUG)
            //var file = new FileInfo($"D:\\tmp\\MI_INDEX_ALLBUT0999_{day:yyyyMMdd}.csv");
            //if (file.Exists) file.Delete();
            //using (var sw = file.CreateText())
            //    sw.Write(csv_data);
#endif
                    return csv_data;
                }
                catch (WebException)
                {
                    _logger.WarnFormat("Target website refuses our connection. Wait till it get peace. day={0}", day.ToString("yyyy-MM-dd"));
                    Thread.Sleep((int)new TimeSpan(1, 0, 0).TotalMilliseconds);
                }
        }
        public virtual IEnumerable<GetStockPeriodPriceResult> GetStockDailyPriceInfo()
        {
            InitStockDailyPriceCache();
            return _stockInfoDictCache.Select(d => d.Value).ToList();
        }
    }
}
