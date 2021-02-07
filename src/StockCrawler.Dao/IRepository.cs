using System;
using System.Collections.Generic;

namespace StockCrawler.Dao
{
    /// <summary>
    /// 股票資訊收集器的資料存取介面
    /// </summary>
    public interface IRepository : IDisposable
    {
        #region 取得資料
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
        GetCategoryMappingResult[] GetCategoryMapping();
        GetStockBasicInfoResult GetStockBasicInfo(string stockNo);
        GetStocksResult[] GetStocks();
        GetStocksResult GetStock(string stockNo);
        GetStockMarketNewsResult[] GetStockMarketNews(int top, string stockNo, string source, DateTime startDate, DateTime endDate);
        /// <summary>
        /// 取得指定期間的股價明細
        /// </summary>
        /// <param name="stockNo">股票代碼</param>
        /// <param name="endDate">結束日期</param>
        /// <param name="period">週期天數</param>
        /// <returns>每日收盤資料列表</returns>
        GetStockPeriodPriceResult[] GetStockPeriodPrice(string stockNo, short period, DateTime bgnDate, DateTime endDate);
        GetStockAveragePriceResult[] GetStockAveragePrice(string stockNo, DateTime bgnDate, DateTime endDate, short period);
        GetLazyStockDataResult GetLazyStockData(string stockNo);
        GetStockForumDataResult[] GetStockForumData(int top, DateTime bgnDate, DateTime endDate, long? id = null, string stockNo = null);
        GetStockFinancialReportResult[] GetStockFinancialReport(int top, string stockNo, short year, short season);
        #endregion

        #region 新增修改
        void InsertStockForumData(IList<(GetStockForumDataResult forum, IList<GetStocksResult> stock)> data);
        void InsertStockMarketNews(GetStockMarketNewsResult[] data);
        /// <summary>
        /// Update stock company's basic information. If it doesn't exist, it will insert it.
        /// </summary>
        /// <param name="data">Collection of stock company's basic information</param>
        void InsertOrUpdateStockBasicInfo(GetStockBasicInfoResult[] data);
        void InsertOrUpdateStockBasicInfo(GetStockBasicInfoResult data);
        void InsertOrUpdateStockPrice(GetStockPeriodPriceResult[] data);
        void InsertOrUpdateStock(GetStocksResult[] data);
        /// <summary>
        /// Update stock name and category no by stock no
        /// </summary>
        /// <param name="stockNo">stock no</param>
        /// <param name="stockName">stock name</param>
        /// <param name="categoryNo">Industry index stock No</param>
        void InsertOrUpdateStock(string stockNo, string stockName, string categoryNo);
        void InsertOrUpdateStockAveragePrice((string stockNo, DateTime stockDT, short period, decimal averagePrice)[] avgPriceList);
        void InsertOrUpdateLazyStock(GetLazyStockDataResult data);
        void InsertOrUpdateStockFinancialReport(GetStockFinancialReportResult data);
        #endregion

        #region 刪除
        void DeleteStockPriceHistoryData(string stockNo, DateTime? tradeDate = null);
        #endregion

        /// <summary>
        /// 取得股價週期平均值
        /// </summary>
        /// <param name="stockNo">股票代碼</param>
        /// <param name="endDate">結束日期</param>
        /// <param name="period">週期天數, 週線: 5, 雙週線: 10, 月線: 20, 季線: 60</param>
        /// <returns>平均收盤價</returns>
        decimal CaculateStockClosingAveragePrice(string stockNo, DateTime endDate, short period);
    }
}
