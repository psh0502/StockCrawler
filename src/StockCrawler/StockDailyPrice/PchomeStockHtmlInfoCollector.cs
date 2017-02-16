using System;
using System.Net;

namespace StockCrawler.Services.StockDailyPrice
{
#if(DEBUG)
    public class PchomeStockHtmlInfoCollector : StockHtmlInfoCollectorBase, IStockDailyInfoCollector
#else
    internal class PchomeStockHtmlInfoCollector : StockHtmlInfoCollectorBase, IStockDailyInfoCollector
#endif
    {
        #region IStockHtmlTextCollector Members

        public virtual StockDailyPriceInfo GetStockDailyPriceInfo(string stock_code)
        {
            string CONST_URL_TEMPLATE = "http://pchome.syspower.com.tw/stock/sid{0}_desc.html"; 
            throw new NotImplementedException();
        }

        public StockDailyPriceInfo[] GetAllStockDailyPriceInfo(StockMarketLineEnum marketline)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
