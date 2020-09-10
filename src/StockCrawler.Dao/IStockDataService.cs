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
        void UpdateStockPriceHistoryDataTable(IList<GetStockPriceHistoryResult> list);
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
        /// 取得股價週其平均值
        /// </summary>
        /// <param name="stockNo">股票代碼</param>
        /// <param name="begDate">開始日期</param>
        /// <param name="endDate">結束日期</param>
        /// <param name="period">週期天數</param>
        /// <param name="top">取多少筆資料</param>
        /// <param name="avgClosePrice">平均收盤價</param>
        /// <param name="avgOpenPrice">平均開盤價</param>
        /// <param name="avgHighPrice">平均最高價</param>
        /// <param name="avgLowPrice">平均最低價</param>
        /// <param name="sumVolume">總交易量</param>
        void GetStockPriceAVG(string stockNo, DateTime begDate, DateTime endDate, short period, int top, out decimal avgClosePrice, out decimal avgOpenPrice, out decimal avgHighPrice, out decimal avgLowPrice, out long sumVolume);
    }
}
