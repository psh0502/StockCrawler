using System;
using System.Collections.Generic;

namespace StockCrawler.Dao
{
    public interface IStockDataService : IDisposable
    {
        /// <summary>
        /// Update stock company's basic information. If it doesn't exist, it will insert it.
        /// </summary>
        /// <param name="data">Collection of stock company's basic information</param>
        void UpdateStockBasicInfo(IEnumerable<GetStockBasicInfoResult> data);
        void UpdateStockBasicInfo(GetStockBasicInfoResult data);
        void UpdateStockCashflowReport(GetStockReportCashFlowResult info);

        ///// <summary>
        ///// Retrieve the average close price of the specified stock since the specified date.
        ///// </summary>
        ///// <param name="sNO">Stock number</param>
        ///// <param name="specifiedDT">Indicate the specified date as the begining of period</param>
        ///// <returns>The average close price since the specified date.</returns>
        //double GetStockAvgPriceByDate(string sNO, DateTime specifiedDT);
        ///// <summary>
        ///// Retrieve the MAX high price of the specified stock in the specified period.
        ///// </summary>
        ///// <param name="sNO">Stock number</param>
        ///// <param name="specifiedDT">Indicate the specified date as the begining of period</param>
        ///// <returns>The MAX close price since the specified date.</returns>
        //double GetStockMaxPriceByPeriod(string sNO, DateTime beginDT, DateTime endDT);
        ///// <summary>
        ///// Retrieve the close price of the specified stock by the specified date.
        ///// </summary>
        ///// <param name="sNO">Stock number</param>
        ///// <param name="specifiedDT">Indicate the specified date</param>
        ///// <returns>The close price on the specified date.</returns>
        //double GetStockPriceByDate(string sNO, DateTime specifiedDT);
        ///// <summary>
        ///// Retrieve the price data row of the specified stock by the specified date.
        ///// </summary>
        ///// <param name="sNO">Stock number</param>
        ///// <param name="specifiedDT">Indicate the specified date</param>
        ///// <returns>The price datarow on the specified date.</returns>
        //StockDataSet.StockPriceHistoryRow GetStockPriceDataRowByDate(string sNO, DateTime specifiedDT);
        //StockDataSet.StockPriceHistoryDataTable GetStockPriceHistoryData(string sno, DateTime startDT, DateTime endDT);
        IList<GetStocksResult> GetStocks();
        void InsertOrUpdateStockPriceHistory(IList<GetStockPriceHistoryResult> list);
        void RenewStockList(IList<GetStocksResult> list);
        /// <summary>
        /// Update stock name by stock no
        /// </summary>
        /// <param name="stockNo">stock no</param>
        /// <param name="stockName">stock name</param>
        void UpdateStockName(string stockNo, string stockName);
        void DeleteStockPriceHistoryData(string stockNo, DateTime? tradeDate = null);
        void UpdateStockIncomeReport(GetStockReportIncomeResult info);
        void UpdateStockBalanceReport(GetStockReportBalanceResult info);
        void UpdateStockMonthlyNetProfitTaxedReport(GetStockReportMonthlyNetProfitTaxedResult info);
        /// <summary>
        /// 取得股價週期平均值
        /// </summary>
        /// <param name="stockNo">股票代碼</param>
        /// <param name="endDate">結束日期</param>
        /// <param name="period">週期天數, 週線: 5, 雙週線: 10, 月線: 20, 季線: 60</param>
        /// <returns>平均收盤價</returns>
        decimal GetStockPriceAVG(string stockNo, DateTime endDate, short period);
        /// <summary>
        /// 取得指定期間的股價明細
        /// </summary>
        /// <param name="stockNo">股票代碼</param>
        /// <param name="endDate">結束日期</param>
        /// <param name="period">週期天數</param>
        /// <returns>每日收盤資料列表</returns>
        IList<GetStockPeriodPriceResult> GetStockPeriodPrice(string stockNo, DateTime endDate, short period);
    }
}
