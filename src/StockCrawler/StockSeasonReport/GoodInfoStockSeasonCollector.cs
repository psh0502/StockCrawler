using System;

namespace StockCrawler.Services.StockSeasonReport
{
    internal class GoodInfoStockSeasonCollector : GoodInfoCollectorBase, IStockSeasonReportCollector
    {
        public decimal GetStockSeasonEPS(string stockNo, short year, short season)
        {
            string url = "https://goodinfo.tw/StockInfo/StockList.asp?SEARCH_WORD=&SHEET=季累計獲利能力&MARKET_CAT=熱門排行&INDUSTRY_CAT=年度EPS最高@@每股稅後盈餘+(EPS)@@年度EPS最高&STOCK_CODE={0}&RANK=0&STEP=DATA&SHEET2=獲利能力&RPT_TIME=" + year + season;
            throw new NotImplementedException();
        }

        public decimal GetStockSeasonNetValue(string stockNo, short year, short season)
        {
            throw new NotImplementedException();
        }
    }
}
