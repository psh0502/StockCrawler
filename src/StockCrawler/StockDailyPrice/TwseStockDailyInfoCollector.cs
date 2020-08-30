using Common.Logging;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace StockCrawler.Services.StockDailyPrice
{
    internal class TwseStockDailyInfoCollector : IStockDailyInfoCollector
    {
        internal static ILog _logger = LogManager.GetLogger(typeof(TwseStockDailyInfoCollector));
        private Dictionary<string, StockDailyPriceInfo> _stockInfoDictCache = null;
        public virtual StockDailyPriceInfo GetStockDailyPriceInfo(string stockNo)
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
                        _stockInfoDictCache = new Dictionary<string, StockDailyPriceInfo>();
                        foreach (var info in GetAllStockDailyPriceInfo(SystemTime.Today))
                        {
                            _stockInfoDictCache[info.StockNo] = info;
                            _logger.DebugFormat("[{0}] {1}", info.StockNo, info.ClosePrice);
                        }
                    }
        }

        private static StockDailyPriceInfo[] GetAllStockDailyPriceInfo(DateTime day)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var csv_data = Tools.DownloadStringData(new Uri($"https://www.twse.com.tw/exchangeReport/MI_INDEX?response=csv&date={day:yyyyMMdd}&type=ALLBUT0999"), Encoding.Default, out IList<Cookie> _);
            if (string.IsNullOrEmpty(csv_data)) {
                _logger.WarnFormat("Download has no data by date[{0}]", day.ToString("yyyyMMdd"));
                return null; 
            }
            _logger.Info(csv_data.Substring(0, 1000));
            // Usage of CsvReader: https://blog.darkthread.net/post-2017-05-13-servicestack-text-csvserializer.aspx
            var csv_lines = CsvReader.ParseLines(csv_data);
            var daily_info = new List<StockDailyPriceInfo>();
            bool found_stock_list = false;
            foreach (var ln in csv_lines)
            {
                //證券代號	證券名稱	成交股數	成交筆數	成交金額	開盤價	最高價	最低價	收盤價	漲跌(+/-)	漲跌價差	最後揭示買價	最後揭示買量	最後揭示賣價	最後揭示賣量	本益比
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
                        if (!string.IsNullOrEmpty(data[i]))
                            data[i] = data[i].Replace("--", "0").Replace(",", string.Empty);

                    daily_info.Add(new StockDailyPriceInfo()
                    {
                        StockNo = data[0].Replace("=\"", string.Empty).Replace("\"", string.Empty),
                        StockName = data[1],
                        Volume = long.Parse(data[2]) / 1000,
                        LastTradeDT = day,
                        OpenPrice = decimal.Parse(data[5]),
                        HighPrice = decimal.Parse(data[6]),
                        LowPrice = decimal.Parse(data[7]),
                        ClosePrice = decimal.Parse(data[8])
                    });
                }
                else
                {
                    if ("證券代號" == data[0])
                        found_stock_list = true;
                }
            }
            return daily_info.ToArray();
        }

        public virtual IList<StockDailyPriceInfo> GetStockDailyPriceInfo()
        {
            InitStockDailyPriceCache();
            return _stockInfoDictCache.Select(d => d.Value).ToList();
        }
    }
}
