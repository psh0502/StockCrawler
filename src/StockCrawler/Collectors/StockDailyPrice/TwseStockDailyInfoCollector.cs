using ServiceStack.Text;
using StockCrawler.Dao;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

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
                        foreach (var info in GetAllStockDailyPriceInfo(SystemTime.Today))
                        {
                            _stockInfoDictCache[info.StockNo] = info;
                            _logger.DebugFormat("[{0}] {1}", info.StockNo, info.ClosePrice);
                        }
                    }
        }

        protected virtual GetStockPeriodPriceResult[] GetAllStockDailyPriceInfo(DateTime day)
        {
            if (day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday) return null;

            var csv_data = DownloadData(day);
            if (string.IsNullOrEmpty(csv_data)) return null;

            _logger.InfoFormat("Day={1}, csv={0}", csv_data.Substring(0, 1000), day.ToShortDateString());
            // Usage of CsvReader: https://blog.darkthread.net/post-2017-05-13-servicestack-text-csvserializer.aspx
            var csv_lines = CsvReader.ParseLines(csv_data);
            var daily_info = new List<GetStockPeriodPriceResult>();
            bool found_stock_list = false;
            foreach (var ln in csv_lines)
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

                string[] data = CsvReader.ParseFields(ln).ToArray();
                if (found_stock_list)
                {
                    if ("備註:" == data[0].Trim())
                    {
                        found_stock_list = false;
                        break;
                    }
                    // Generalize number fields data
                    for (int i = 2; i < data.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(data[i]))
                            data[i] = data[i]
                                .Replace("--", "0")
                                .Replace(",", string.Empty)
                                .Replace("X", string.Empty)
                                .Trim();
                    }
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
                    //_logger.DebugFormat("StockNo={0}\r\nStockDT={1}\r\nOpenPrice={2}\r\nHighPrice={3}\r\nLowPrice={4}\r\nClosePrice={5}\r\nVolume={6}\r\nDeltaPrice={7}\r\nPE={8}",
                    // tmp.StockNo, tmp.StockDT.ToShortDateString(), tmp.OpenPrice, tmp.HighPrice, tmp.LowPrice, tmp.ClosePrice, tmp.Volume, tmp.DeltaPrice, tmp.PE);
                    // 取小數點下四位就好
                    tmp.DeltaPercent = decimal.Parse((tmp.DeltaPrice / (tmp.OpenPrice == 0 ? 1 : tmp.OpenPrice)).ToString("0.####"));
                    daily_info.Add(tmp);
                }
                else
                {
                    if ("證券代號" == data[0])
                        found_stock_list = true;
                }
            }
            return daily_info.ToArray();
        }

        protected virtual string DownloadData(DateTime day)
        {
            var csv_data = Tools.DownloadStringData(new Uri($"https://www.twse.com.tw/exchangeReport/MI_INDEX?response=csv&date={day:yyyyMMdd}&type=ALLBUT0999"), Encoding.Default, out IList<Cookie> _);
            if (string.IsNullOrEmpty(csv_data))
            {
                _logger.WarnFormat("Download has no data by date[{0}]", day.ToString("yyyyMMdd"));
                return null;
            }
#if (DEBUG)
            var file = new FileInfo($"D:\\tmp\\MI_INDEX_ALLBUT0999_{day:yyyyMMdd}.csv");
            if (file.Exists) file.Delete();
            using (var sw = file.CreateText())
                sw.Write(csv_data);
#endif
            return csv_data;
        }

        public virtual IEnumerable<GetStockPeriodPriceResult> GetStockDailyPriceInfo()
        {
            InitStockDailyPriceCache();
            return _stockInfoDictCache.Select(d => d.Value).ToList();
        }
    }
}
