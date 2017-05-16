using StockCrawler.Dao.Schema;
using System;

namespace StockCrawler.Dao
{
    public interface IStockDataService
    {
        /// <summary>
        /// Retrieve the average close price of the specified stock since the specified date.
        /// </summary>
        /// <param name="sNO">Stock number</param>
        /// <param name="specifiedDT">Indicate the specified date as the begining of period</param>
        /// <returns>The average close price since the specified date.</returns>
        public double GetStockAvgPriceByDate(string sNO, DateTime specifiedDT);
        /// <summary>
        /// Retrieve the MAX high price of the specified stock in the specified period.
        /// </summary>
        /// <param name="sNO">Stock number</param>
        /// <param name="specifiedDT">Indicate the specified date as the begining of period</param>
        /// <returns>The MAX close price since the specified date.</returns>
        public double GetStockMaxPriceByPeriod(string sNO, DateTime beginDT, DateTime endDT);
        /// <summary>
        /// Retrieve the close price of the specified stock by the specified date.
        /// </summary>
        /// <param name="sNO">Stock number</param>
        /// <param name="specifiedDT">Indicate the specified date</param>
        /// <returns>The close price on the specified date.</returns>
        public double GetStockPriceByDate(string sNO, DateTime specifiedDT);
        /// <summary>
        /// Retrieve the price data row of the specified stock by the specified date.
        /// </summary>
        /// <param name="sNO">Stock number</param>
        /// <param name="specifiedDT">Indicate the specified date</param>
        /// <returns>The price datarow on the specified date.</returns>
        public StockDataSet.StockPriceHistoryRow GetStockPriceDataRowByDate(string sNO, DateTime specifiedDT);
        public StockDataSet.StockPriceHistoryDataTable GetStockPriceHistoryData(string sno, DateTime startDT, DateTime endDT);
        public StockDataSet.StockDataTable GetStocks();
        public StockDataSet.StockDataTable GetStocksSchema();
        public void UpdateStockPriceHistoryDataTable(StockDataSet.StockPriceHistoryDataTable dt);
        public void RenewStockList(StockDataSet.StockDataTable dt);
    }
}
