﻿using System;

namespace StockCrawler.Services.StockDailyPrice
{
    public interface IStockDailyInfoCollector
    {
        StockDailyPriceInfo GetStockDailyPriceInfo(string stockNo);
    }

    public class StockDailyPriceInfo
    {
        public string StockNo { get; set; }
        public string StockName { get; set; }
        public DateTime LastTradeDT { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public long Volume { get; set; }
        public override string ToString()
        {
            return $"StockCode={StockNo}, StockName={StockName}, LastTradeDT={LastTradeDT:yyyyMMdd}, OpenPrice={OpenPrice}, HighPrice={HighPrice}, LowPrice={LowPrice}, ClosePrice={ClosePrice}, Volume={Volume}";
        }
    }
}
