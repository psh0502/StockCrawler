using System;
using System.Collections.Generic;
using StockCrawler.Dao;

namespace StockCrawler.Services.Collectors
{
    /// <summary>
    /// 收集股票除權息的收集器介面
    /// </summary>
    public interface IStockInterestIssuedCollector
    {
        /// <summary>
        /// 取得除權息資訊
        /// </summary>
        /// <param name="stockNo">股票代碼</param>
        /// <exception cref="ApplicationException">該公司股票不繼續公開發行</exception>
        /// <returns>除權息資訊</returns>
        IList<GetStockInterestIssuedInfoResult> GetStockInterestIssuedInfo(string stockNo);
    }
}