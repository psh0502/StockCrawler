using System;

namespace StockCrawler.Services.StockDailyPrice
{
    public enum StockMarketLineEnum
    {
        tse = 0,
        otc
    }
    public interface IStockDailyInfoCollector
    {
        StockDailyPriceInfo GetStockDailyPriceInfo(string stock_code);
        StockDailyPriceInfo[] GetAllStockDailyPriceInfo(StockMarketLineEnum marketline);
    }

    public class StockDailyPriceInfo
    {
        public string StockCode { get; set; }
        public string StockName { get; set; }
        public DateTime LastTradeDT { get; set; }
        public decimal LastTrade { get; set; }
        public decimal LastBid { get; set; }
        public decimal LastAsk { get; set; }
        public decimal Change { get; set; }
        public long Volume { get; set; }
        public decimal Open { get; set; }
        public decimal Top { get; set; }
        public decimal Lowest { get; set; }
        public decimal PrevClose { get; set; }
        public override string ToString()
        {
            return string.Format("[{0}]: StockCode={1} / LastTradeDT={2} / LastTrade={3} / LastBid={4} / LastAsk={5} / Change={6} / Volumn={7} / Open={8} / Top={9} / Lowest={10} / PrevClose={11} / StockName={12}",
                GetType().Name, StockCode, LastTradeDT, LastTrade, LastBid, LastAsk, Change, Volume, Open, Top, Lowest, PrevClose, StockName);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is StockDailyPriceInfo)) return false;
            StockDailyPriceInfo trg = (StockDailyPriceInfo)obj;
            return (
                trg.LastTradeDT == LastTradeDT &&
                trg.LastAsk == LastAsk &&
                trg.Change == Change &&
                trg.LastBid == LastBid &&
                trg.LastTrade == LastTrade &&
                trg.Lowest == Lowest &&
                trg.Open == Open &&
                trg.PrevClose == PrevClose &&
                trg.StockCode == StockCode &&
                trg.StockName == StockName &&
                trg.Top == Top &&
                trg.Volume == Volume);

        }

        public override int GetHashCode()
        {
            return base.GetType().GetHashCode();
        }
    }
}
