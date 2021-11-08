using System;
using System.Collections.Generic;
using StockCrawler.Dao;

namespace StockCrawler.Services.Collectors
{
    /// <summary>
    /// 收集股票每月營收紀錄的收集器介面
    /// </summary>
    public interface IStockMonthlyIncomeCollector
    {
        /// <summary>
        /// 取得簡易財務報表
        /// </summary>
        /// <param name="stockNo">股票代碼</param>
        /// <exception cref="ApplicationException">該公司股票不繼續公開發行</exception>
        /// <returns>每月營收紀錄</returns>
        IList<GetStockMonthlyIncomeResult> GetStockMonthlyIncome(string stockNo);
    }
}