using Common.Logging;
using Newtonsoft.Json;
using System;
using System.Text;

namespace StockCrawler.Services.Collectors
{
    internal class LazyStockCollector : ILazyStockCollector
    {
        internal ILog _logger = LogManager.GetLogger(typeof(LazyStockCollector));
        public LazyStockData GetData(string stockNo)
        {
            LazyStockData result = null;
            try
            {
                var json = Tools.DownloadStringData(new Uri($"http://www.lazystock.tw/Data/GetStockInfo?StockNum={stockNo}&isFullYear=false"), Encoding.UTF8, out _);
                result = JsonConvert.DeserializeObject<LazyStockData>(json);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;
        }
    }
}
