using System;

namespace StockCrawler.Services.StockBasicInfo
{
    public interface IStockBasicInfoCollector
    {
        StockBasicInfo GetStockBasicInfo(string stockNo);
    }

    public class StockBasicInfo
    {
        public string StockNo { get; set; }
        public string StockName { get; set; }
        public DateTime LastTradeDT { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public long Volume { get; set; }
    }
}
