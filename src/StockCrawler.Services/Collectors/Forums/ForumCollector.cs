using Common.Logging;
using Newtonsoft.Json;
using System;
using System.Text;

namespace StockCrawler.Services.Collectors
{
    internal class ForumCollector : IForumCollector
    {
        internal ILog _logger = LogManager.GetLogger(typeof(ForumCollector));
        public LazyStockData GetData(string stockNo)
        {
            try
            {
                var json = Tools.DownloadStringData(new Uri($"http://www.lazystock.tw/Data/GetStockInfo?StockNum={stockNo}&isFullYear=false"), Encoding.UTF8, out _, method: "POST");
                var result = JsonConvert.DeserializeObject<LazyStockData>(json);
                if (result.Code != 0) throw new ApplicationException("[" + stockNo + "]" + result.Msg);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }
    }
}
