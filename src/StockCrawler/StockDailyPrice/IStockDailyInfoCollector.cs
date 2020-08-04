using System;

namespace StockCrawler.Services.StockDailyPrice
{
    public interface IStockDailyInfoCollector
    {
        StockDailyPriceInfo GetStockDailyPriceInfo(string stock_code);
    }

    public class StockDailyPriceInfo
    {
        public string StockCode { get; set; }
        public string StockName { get; set; }
        public DateTime LastTradeDT { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public long Volume { get; set; }
        public override string ToString()
        {
            return $"StockCode={StockCode}, StockName={StockName}, LastTradeDT={LastTradeDT:yyyyMMdd HH:mm}, OpenPrice={OpenPrice}, HighPrice={HighPrice}, LowPrice={LowPrice}, ClosePrice={ClosePrice}, Volume={Volume}";
        }
    }
}
