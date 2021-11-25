using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace StockCrawler.Dao
{
    /// <summary>
    /// 透過快取設計，快速取得各種股票市場列表的輔助工具。
    /// </summary>
    public static class StockHelper
    {
        private static IList<GetStocksResult> _STOCK_LIST = null;
        private static IList<GetStocksResult> _STOCK_COMPANY_LIST = null;
        private static IList<GetStocksResult> _STOCK_INDEX_LIST = null;
        private static IList<GetStocksResult> _ETF_LIST = null;
        private static ConcurrentDictionary<string, GetStocksResult> _STOCK_DICT = null;
        static StockHelper()
        {
            Reload();
        }
        /// <summary>
        /// 取得完整的股票市場列表
        /// </summary>
        /// <returns>完整的股票市場列表</returns>
        public static IList<GetStocksResult> GetAllStockList()
        {
            if (null == _STOCK_LIST)
                lock (typeof(StockHelper))
                    if (null == _STOCK_LIST)
                        using (var db = RepositoryProvider.GetRepositoryInstance())
                            _STOCK_LIST = db.GetStocks().ToList();

            return _STOCK_LIST;
        }
        /// <summary>
        /// 取得基金型(ETF)列表
        /// </summary>
        /// <returns>基金型(ETF)列表</returns>
        public static IList<GetStocksResult> GetETFList()
        {
            if (null == _ETF_LIST)
                lock (typeof(StockHelper))
                    if (null == _ETF_LIST)
                        _ETF_LIST = GetAllStockList().Where(
                            d => d.Type == (short)EnumStockType.ETF).ToList();

            return _ETF_LIST;
        }
        /// <summary>
        /// 取得與關鍵字相關的股票列表
        /// </summary>
        /// <param name="fuzzyKeyword">關鍵字</param>
        /// <returns>與關鍵字相關的股票列表</returns>
        public static IList<GetStocksResult> GetFuzzyMatchedStockList(string fuzzyKeyword)
        {
            return GetAllStockList()
                .Where(d => d.StockNo.Contains(fuzzyKeyword) || d.StockName.Contains(fuzzyKeyword))
                .ToList();
        }
        /// <summary>
        /// 以股票代碼或是股票名稱取得對應的股票
        /// </summary>
        /// <param name="keyword">股票代碼或是股票名稱</param>
        /// <returns>對應的股票</returns>
        public static GetStocksResult GetMatchedStock(string keyword)
        {
            return GetStock(keyword) ?? GetAllStockList()
                .Where(d => d.StockName == keyword)
                .FirstOrDefault();
        }
        /// <summary>
        /// 以股票代碼取得對應的股票資料
        /// </summary>
        /// <param name="stockNo">股票代碼</param>
        /// <returns>股票資料</returns>
        public static GetStocksResult GetStock(string stockNo)
        {
            if (string.IsNullOrEmpty(stockNo)) return null;

            if (null == _STOCK_DICT)
                lock (typeof(StockHelper))
                    if (null == _STOCK_DICT)
                        _STOCK_DICT = new ConcurrentDictionary<string, GetStocksResult>(
                            GetAllStockList().ToDictionary(d => d.StockNo));

            if (_STOCK_DICT.ContainsKey(stockNo))
                return _STOCK_DICT[stockNo];
            else
                return null;
        }
        /// <summary>
        /// 取得完整的上市公司列表
        /// </summary>
        /// <returns>上市公司列表</returns>
        public static IList<GetStocksResult> GetCompanyStockList()
        {
            if (null == _STOCK_COMPANY_LIST)
                lock (typeof(StockHelper))
                    if (null == _STOCK_COMPANY_LIST)
                    {
                        _STOCK_COMPANY_LIST = GetAllStockList()
                            .Where(d => !d.StockNo.StartsWith("0")
                                && int.TryParse(d.StockNo, out _))
                            .ToList();
                    }

            return _STOCK_COMPANY_LIST;
        }
        /// <summary>
        /// 取得指數類型列表
        /// </summary>
        /// <returns>指數類型列表</returns>
        public static IList<GetStocksResult> GetIndexStockList()
        {
            if (null == _STOCK_INDEX_LIST)
                lock (typeof(StockHelper))
                    if (null == _STOCK_INDEX_LIST)
                        _STOCK_INDEX_LIST = GetAllStockList().Where(
                            d => d.StockNo.StartsWith("0")
                                && int.TryParse(d.StockNo, out int sno)
                                && sno < 50)
                                .ToList();

            return _STOCK_INDEX_LIST;
        }
        /// <summary>        
        /// 清空快取重新讀取所有列表
        /// </summary>
        public static void Reload()
        {
            _STOCK_LIST = null;
            _STOCK_COMPANY_LIST = null;
            _STOCK_INDEX_LIST = null;
            _STOCK_DICT = null;
            _ETF_LIST = null;
        }
    }
}
