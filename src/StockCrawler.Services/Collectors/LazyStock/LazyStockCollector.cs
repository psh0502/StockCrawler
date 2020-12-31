using Common.Logging;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;

namespace StockCrawler.Services.Collectors
{
    internal class LazyStockCollector : ILazyStockCollector
    {
        internal ILog _logger = LogManager.GetLogger(typeof(LazyStockCollector));
        public LazyStockData GetData(string stockNo)
        {
            try
            {
                do
                {
                    var json = Tools.DownloadStringData(new Uri($"http://www.lazystock.tw/Data/GetStockInfo?StockNum={stockNo}&isFullYear=false"), Encoding.UTF8, out _, method: "POST");
                    var result = JsonConvert.DeserializeObject<LazyStockData>(json);
                    if (result.Code == 0)
                        return result;
                    else
                    {
                        if (result.Msg == "查詢過於繁複，已達本日上限")
                        {
                            var retryInterval = new TimeSpan(0, 30, 0);
                            _logger.WarnFormat("[{0}][{1}]{2}...retry after {3} mins", stockNo, result.Code, result.Msg, retryInterval.TotalMinutes);
                            Thread.Sleep(retryInterval);
                        }
                        else
                            throw new ApplicationException(string.Format("[{0}][{1}]{2}", stockNo, result.Code, result.Msg));
                    }
                } while (true);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }
    }
}
