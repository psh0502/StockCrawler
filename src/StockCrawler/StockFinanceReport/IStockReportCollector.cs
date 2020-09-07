﻿using StockCrawler.Dao;

namespace StockCrawler.Services.StockFinanceReport
{
    /// <summary>
    /// 收集股票財務資訊的收集器介面
    /// </summary>
    public interface IStockReportCollector
    {
        /// <summary>
        /// 取得現金流量表
        /// </summary>
        /// <param name="stockNo">股票代碼</param>
        /// <param name="year">中華民國年度</param>
        /// <param name="season">第幾季</param>
        /// <returns>現金流量表</returns>
        GetStockReportCashFlowResult GetStockReportCashFlow(string stockNo, short year, short season);
        /// <summary>
        /// 取得綜合損益表
        /// </summary>
        /// <param name="stockNo">股票代碼</param>
        /// <param name="year">中華民國年度</param>
        /// <param name="season">第幾季</param>
        /// <returns>綜合損益表</returns>
        GetStockReportIncomeResult GetStockReportIncome(string stockNo, short year, short season);
        /// <summary>
        /// 取得資產負債表
        /// </summary>
        /// <param name="stockNo">股票代碼</param>
        /// <param name="year">中華民國年度</param>
        /// <param name="season">第幾季</param>
        /// <returns>資產負債表</returns>
        GetStockReportBalanceResult GetStockReportBalance(string stockNo, short year, short season);
        /// <summary>
        /// 取得每月營收報告
        /// </summary>
        /// <param name="stockNo">股票代碼</param>
        /// <param name="year">中華民國年度</param>
        /// <param name="month">月份</param>
        /// <returns>每月營收報告</returns>
        GetStockReportMonthlyNetProfitTaxedResult GetStockReportMonthlyNetProfitTaxed(string stockNo, short year, short month);
        /// <summary>
        /// 取得本益比(P/E ratio)
        /// </summary>
        /// <param name="stockNo">股票代碼</param>
        /// <param name="year">中華民國年度</param>
        /// <param name="month">月份</param>
        /// <returns>本益比 = 月均價 / 近4季EPS總和</returns>
        decimal GetStockMonthPE(string stockNo, short year, short month);
    }
}