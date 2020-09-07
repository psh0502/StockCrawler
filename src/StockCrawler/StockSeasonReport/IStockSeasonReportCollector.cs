namespace StockCrawler.Services.StockSeasonReport
{
    /// <summary>
    /// 收集股票季度數據的收集器介面
    /// </summary>
    public interface IStockSeasonReportCollector
    {
        /// <summary>
        /// 取得每股淨值(季度)
        /// </summary>
        /// <param name="stockNo">股票代碼</param>
        /// <param name="year">中華民國年度</param>
        /// <param name="season">第幾季</param>
        /// <returns>每股淨值</returns>
        decimal GetStockSeasonNetValue(string stockNo, short year, short season);
        /// <summary>
        /// 取得每股營利 EPS
        /// </summary>
        /// <param name="stockNo">股票代碼</param>
        /// <param name="year">中華民國年度</param>
        /// <param name="season">第幾季</param>
        /// <returns>每股營利 EPS</returns>
        decimal GetStockSeasonEPS(string stockNo, short year, short season);
    }
}