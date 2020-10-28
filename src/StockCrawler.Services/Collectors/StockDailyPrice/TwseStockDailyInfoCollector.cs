using ServiceStack;
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
        private Dictionary<string, string> _stockCategoryNo = null;
        private Dictionary<string, long> _categoriedVolume = null;
        public TwseStockDailyInfoCollector()
        {
            InitStockDailyPriceCache();
        }
        public virtual GetStockPeriodPriceResult GetStockDailyPriceInfo(string stockNo)
        {
            return (_stockInfoDictCache.ContainsKey(stockNo)) ? _stockInfoDictCache[stockNo] : null;
        }
        private void InitStockDailyPriceCache()
        {
            if (!Tools.IsWeekend(SystemTime.Today))
                if (null == _stockInfoDictCache)
                    lock (this)
                    {
                        if (null == _categoriedVolume)
                        {
                            _categoriedVolume = new Dictionary<string, long>();
                            _stockCategoryNo = new Dictionary<string, string>();
                            using (var db = StockDataServiceProvider.GetServiceInstance())
                            {
                                var stock_data = db.GetStocks();
                                foreach (var d in stock_data
                                    .Where(s =>
                                        s.StockNo.StartsWith("00")
                                        && int.TryParse(s.StockNo, out int no)
                                        && no < 50))
                                    _categoriedVolume.Add(d.StockNo, 0);

                                foreach (var d in stock_data
                                    .Where(s => 
                                        !s.StockNo.StartsWith("00")
                                        && !string.IsNullOrEmpty(s.CategoryNo)))
                                    _stockCategoryNo.Add(d.StockNo, d.CategoryNo);
                            }
                        }

                        if (null == _stockInfoDictCache)
                        {
                            _logger.Info("Initialize all stock information cache.");
                            _stockInfoDictCache = new Dictionary<string, GetStockPeriodPriceResult>();

                            var data = GetAllStockDailyPriceInfo(SystemTime.Today);
                            if (null != data)
                                foreach (var info in data)
                                {
                                    _stockInfoDictCache[info.StockNo] = info;
                                    _logger.DebugFormat("[{0}] {1}", info.StockNo, info.ClosePrice);
                                }
                        }
                    }
        }
        protected virtual GetStockPeriodPriceResult[] GetAllStockDailyPriceInfo(DateTime day)
        {
            if (Tools.IsWeekend(day)) return null;

            var csv_data = DownloadData(day);
            if (string.IsNullOrEmpty(csv_data)) return null;

            _logger.InfoFormat("Day={1}, csv={0}", csv_data.Substring(0, 1000), day.ToShortDateString());
            // Usage of CsvReader: https://blog.darkthread.net/post-2017-05-13-servicestack-text-csvserializer.aspx
            var csv_lines = CsvReader.ParseLines(csv_data);
            var daily_info = new Dictionary<string, GetStockPeriodPriceResult>();
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
                    _categoriedVolume["0000"] += d.Volume;
                    if (_stockCategoryNo.ContainsKey(d.StockNo))
                        _categoriedVolume[_stockCategoryNo[d.StockNo]] += d.Volume;
                }
                else
                {
                    found_stock_list = ("證券代號" == data[0]);
                    if (!found_stock_list && data.Length == 7 && i < 100)
                        foreach (var s in GetCategoryStockList())
                            if (data[0].Contains(s.StockName))
                            {
                                var marketIndexStock = GetParsedCategoryMarketIndexData(day, data, s);
                                    daily_info[marketIndexStock.StockNo] = marketIndexStock;
                            }
                }
            }
            if (daily_info.Any())
            {
                foreach(var d in _categoriedVolume)
                    daily_info[d.Key].Volume = d.Value;
                // 未含金融指數(0009) = 大盤指數(0000) - 金融保險類指數(0040) - 
                daily_info["0009"].Volume = daily_info["0000"].Volume - daily_info["0040"].Volume;
                // 未含電子指數(0010) = 
                daily_info["0010"].Volume = 
                    daily_info["0000"].Volume // 大盤指數(0000) 
                    - daily_info["0029"].Volume // 半導體業 - 通信網路業(0032) - 電子零組件業(0033) - 電子通路業(0034) - 其他電子業(0036)
                    - daily_info["0030"].Volume // 電腦及週邊設備業
                    - daily_info["0031"].Volume // 光電業
                    - daily_info["0032"].Volume // 通信網路業
                    - daily_info["0033"].Volume // 電子零組件業
                    - daily_info["0034"].Volume // 電子通路業
                    - daily_info["0035"].Volume // 資訊服務業
                    - daily_info["0036"].Volume;    // 其他電子業
                // 未含電子與金融的指數
                daily_info["0011"].Volume = daily_info["0010"].Volume - daily_info["0040"].Volume;
            }
            return daily_info.Values.ToArray();
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
