using System;
using System.Net;

namespace StockCrawler.Services.StockDailyPrice
{
    internal class YahooFinanceDownloader : IStockDailyInfoCollector
    {
        private const string CONST_URL_TEMPLATE = "http://download.finance.yahoo.com/d/quotes.csv?s={0}.TW&f=sl1d1t1c1ohgv&e=.csv";
        #region IStockHtmlInfoCollector Members

        public string GetHtmlText(string stock_code)
        {
            string rtnText = null;
            using (WebClient wc = new WebClient())
            {
                rtnText = wc.DownloadString(string.Format(CONST_URL_TEMPLATE, stock_code));
            }
            return rtnText;
        }

        public StockDailyPriceInfo GetStockDailyPriceInfo(string stock_code)
        {
            string tmp = GetHtmlText(stock_code);

            StockDailyPriceInfo info = new StockDailyPriceInfo();

            return info;
        }

        public StockDailyPriceInfo[] GetAllStockDailyPriceInfo(StockMarketLineEnum marketline)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
