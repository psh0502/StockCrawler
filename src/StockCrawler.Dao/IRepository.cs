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
        GetStockPriceHistoryPagingResult[] GetStockPriceHistoryPaging(
            string stockNo
            , DateTime bgnDate
            , DateTime endDate
            , int top
            , int currentPage
            , int pageSize
            , out int? pageCount);
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
        /// <param name="bgnDate">起始日期</param>
        /// <param name="endDate">結束日期</param>
        /// <returns cref="GetStockPriceHistoryResult">每日收盤資料列表</returns>
        GetStockPriceHistoryResult[] GetStockPriceHistory(string stockNo, DateTime bgnDate, DateTime endDate);
        GetStockAveragePriceResult[] GetStockAveragePrice(string stockNo, DateTime bgnDate, DateTime endDate, short period = -1);
        GetStockTechnicalIndicatorsResult[] GetStockTechnicalIndicators(string stockNo, DateTime bgnDate, DateTime endDate, string type);
        GetStockAnalysisDataResult GetStockAnalysisData(string stockNo);
        GetStockForumDataResult[] GetStockForumData(int top, DateTime bgnDate, DateTime endDate, long? id = null, string stockNo = null);
        GetStockFinancialReportResult[] GetStockFinancialReport(int top, string stockNo, short year = -1, short season = -1);
        GetStockInterestIssuedInfoResult[] GetStockInterestIssuedInfo(int top, string stockNo, short year = -1, short season = -1);
        GetStockMonthlyIncomeResult[] GetStockMonthlyIncomeData(int top, string stockNo, short year = -1, short month = -1);
        /// <summary>
        /// 取得 ETF 的基本資料
        /// </summary>
        /// <param name="stockNo">股票代碼</param>
        /// <returns>ETF 的基本資料</returns>
        GetETFBasicInfoResult GetETFBasicInfo(string stockNo);
        /// <summary>
        /// 取得 ETF 成分股清單
        /// </summary>
        /// <param name="etfNo">ETF 股票代碼</param>
        /// <returns>ETF 成分股清單</returns>
        GetETFIngredientsResult[] GetETFIngredients(string etfNo);
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
        void InsertOrUpdateStockPrice(GetStockPriceHistoryResult[] data);
        void InsertOrUpdateStock(GetStocksResult[] data);
        /// <summary>
        /// Update stock name and category no by stock no
        /// </summary>
        /// <param name="stockNo">stock no</param>
        /// <param name="stockName">stock name</param>
        /// <param name="categoryNo">Industry index stock No</param>
        /// <param name="type">define the stock type whether INDEX, DR, ETF or STOCK</param>
        void InsertOrUpdateStock(string stockNo, string stockName, string categoryNo, EnumStockType type);
        void InsertOrUpdateStockAveragePrice(GetStockAveragePriceResult[] avgPrices);
        void InsertOrUpdateStockTechnicalIndicators(GetStockTechnicalIndicatorsResult[] indicators);
        void InsertOrUpdateStockAnalysis(GetStockAnalysisDataResult data);
        void InsertOrUpdateStockFinancialReport(GetStockFinancialReportResult data);
        /// <summary>
        /// Insert or update stock interest cash/stock issued.
        /// </summary>
        /// <param name="data">Interest issued info</param>
        void InsertOrUpdateStockInterestIssuedInfo(GetStockInterestIssuedInfoResult data);
        /// <summary>
        /// 新增或更新股票月營收數據
        /// </summary>
        /// <param name="data">股票月營收數據</param>
        void InsertOrUpdateStockMonthlyIncome(GetStockMonthlyIncomeResult[] data);
        /// <summary>
        /// Update ETF's basic information. If it doesn't exist, it will insert.
        /// </summary>
        /// <param name="data">Collection of ETF's basic information</param>
        void InsertOrUpdateETFBasicInfo(GetETFBasicInfoResult[] data);
        /// <summary>
        /// Update ETF's basic information. If it doesn't exist, it will insert.
        /// </summary>
        /// <param name="data">ETF's basic information</param>
        void InsertOrUpdateETFBasicInfo(GetETFBasicInfoResult data);
        /// <summary>
        /// Insert bunch of ingredients of ETF
        /// </summary>
        /// <param name="data">Bunch of ingredients</param>
        void InsertETFIngredients(GetETFIngredientsResult[] data);
        #endregion

        #region 刪除
        void DeleteStockPriceHistoryData(string stockNo, DateTime? tradeDate = null);
        void ClearETFIngredients(string ETFNo);
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
