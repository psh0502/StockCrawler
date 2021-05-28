using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace StockCrawler.Dao
{
    internal class MsSqlRepository : IRepository
    {
        private static StockDataContext GetMSSQLStockDataContext()
        {
            return new StockDataContext(ConnectionStringHelper.StockConnectionString);
        }

        #region 取得資料
        public GetStockPriceHistoryPagingResult[] GetStockPriceHistoryPaging(
            string stockNo
            , DateTime bgnDate
            , DateTime endDate
            , short period
            , int top
            , int currentPage
            , int pageSize
            , out int? pageCount)
        {
            pageCount = 0;
            using (var db = GetMSSQLStockDataContext())
                return db.GetStockPriceHistoryPaging(stockNo, bgnDate, endDate, period, top, currentPage, pageSize, ref pageCount).ToArray();
        }
        public GetCategoryMappingResult[] GetCategoryMapping()
        {
            using (var db = GetMSSQLStockDataContext())
                return db.GetCategoryMapping().ToArray();
        }
        public GetStocksResult[] GetStocks()
        {
            using (var db = GetMSSQLStockDataContext())
                return db.GetStocks(null).ToArray();
        }
        public GetStocksResult GetStock(string stockNo)
        {
            using (var db = GetMSSQLStockDataContext())
                return db.GetStocks(stockNo).SingleOrDefault();
        }
        public GetStockMarketNewsResult[] GetStockMarketNews(int top, string stockNo, string source, DateTime startDate, DateTime endDate)
        {
            using (var db = GetMSSQLStockDataContext())
                return db.GetStockMarketNews(top, stockNo, source, startDate, endDate).ToArray();
        }
        public GetStockPeriodPriceResult[] GetStockPeriodPrice(string stockNo, short period, DateTime bgnDate, DateTime endDate)
        {
            using (var db = GetMSSQLStockDataContext())
                return db.GetStockPeriodPrice(stockNo, period, bgnDate, endDate).ToArray();
        }
        public GetStockAveragePriceResult[] GetStockAveragePrice(string stockNo, DateTime bgnDate, DateTime endDate, short period)
        {
            using (var db = GetMSSQLStockDataContext())
                return db.GetStockAveragePrice(stockNo, bgnDate, endDate, period).ToArray();
        }
        public GetStockBasicInfoResult GetStockBasicInfo(string stockNo)
        {
            using (var db = GetMSSQLStockDataContext())
                return db.GetStockBasicInfo(stockNo).SingleOrDefault();
        }
        public GetStockAnalysisDataResult GetStockAnalysisData(string stockNo)
        {
            using (var db = GetMSSQLStockDataContext())
                return db.GetStockAnalysisData(stockNo).SingleOrDefault();
        }
        public GetStockForumDataResult[] GetStockForumData(int top, DateTime bgnDate, DateTime endDate, long? id = null, string stockNo = null)
        {
            using (var db = GetMSSQLStockDataContext())
                return db.GetStockForumData(top, id, stockNo, bgnDate, endDate).ToArray();
        }
        public GetStockFinancialReportResult[] GetStockFinancialReport(int top, string stockNo, short year, short season)
        {
            using (var db = GetMSSQLStockDataContext())
                return db.GetStockFinancialReport(top, stockNo, year, season).ToArray();
        }
        public GetStockInterestIssuedInfoResult[] GetStockInterestIssuedInfo(int top, string stockNo, short year, short season)
        {
            using (var db = GetMSSQLStockDataContext())
                return db.GetStockInterestIssuedInfo(top, stockNo, year, season).ToArray();
        }
        #endregion

        #region 新增修改
        public void InsertStockForumData(IList<(GetStockForumDataResult forum, IList<GetStocksResult> stock)> data)
        {
            using (var db = GetMSSQLStockDataContext())
            {
                db.Connection.Open();
                db.Transaction = db.Connection.BeginTransaction();
                try
                {
                    foreach (var d in data)
                    {
                        long? id = 0;
                        db.InsertStockForums(d.forum.Source, d.forum.Subject, d.forum.Hash, d.forum.Url, d.forum.ArticleDate, ref id);
                        d.forum.ID = id ?? 0;
                        if (d.forum.ID > 0)
                            foreach (var s in d.stock)
                                db.InsertStockForumRelations(s.StockNo, d.forum.ID);
                    }
                    db.Transaction.Commit();
                }
                catch
                {
                    try
                    {
                        db.Transaction.Rollback();
                    }
                    catch(SqlException) { }
                    throw;
                }
            }
        }
        public void InsertStockMarketNews(GetStockMarketNewsResult[] data)
        {
            using (var db = GetMSSQLStockDataContext())
                foreach (var d in data)
                    try
                    {
                        db.InsertStockMarketNews(d.StockNo, d.Source, d.NewsDate, d.Subject, d.Url);
                    }
                    catch (SqlException)
                    {
                        Debug.WriteLine(string.Format("[{0}] news can't be wrote.", d.StockNo));
                        throw;
                    }
        }
        public void InsertOrUpdateStockPrice(GetStockPeriodPriceResult[] data)
        {
            using (var db = GetMSSQLStockDataContext())
                foreach (var d in data)
                    db.InsertOrUpdateStockPriceHistory(
                        d.StockNo,
                        d.StockDT,
                        d.Period,
                        d.OpenPrice,
                        d.HighPrice,
                        d.LowPrice,
                        d.ClosePrice,
                        d.DeltaPrice,
                        d.DeltaPercent,
                        d.PE,
                        d.Volume);
        }
        public void InsertOrUpdateStock(GetStocksResult[] data)
        {
            using (var db = GetMSSQLStockDataContext())
            {
                db.DisableAllStocks();
                foreach (var d in data)
                    db.InsertOrUpdateStock(d.StockNo, d.StockName, d.CategoryNo);
            }
        }
        public void InsertOrUpdateStock(string stockNo, string stockName, string categoryNo)
        {
            using (var db = GetMSSQLStockDataContext())
                db.InsertOrUpdateStock(stockNo, stockName, categoryNo);
        }
        public void InsertOrUpdateStockBasicInfo(GetStockBasicInfoResult[] data)
        {
            foreach (var d in data)
                InsertOrUpdateStockBasicInfo(d);
        }
        public void InsertOrUpdateStockBasicInfo(GetStockBasicInfoResult info)
        {
            using (var db = GetMSSQLStockDataContext())
                db.InsertOrUpdateStockBasicInfo(
                    info.StockNo,
                    info.Category,
                    info.CompanyName,
                    info.CompanyID,
                    info.BuildDate,
                    info.PublishDate,
                    info.Capital,
                    info.ReleaseStockCount,
                    info.Chairman,
                    info.CEO,
                    info.Url,
                    info.Business);
        }
        public void InsertOrUpdateStockAveragePrice((string stockNo, DateTime stockDT, short period, decimal averagePrice)[] avgPriceList)
        {
            using (var db = GetMSSQLStockDataContext())
                foreach (var info in avgPriceList)
                    db.InsertOrUpdateStockAveragePrice(
                        info.stockNo,
                        info.stockDT,
                        info.period,
                        info.averagePrice);
        }
        public void InsertOrUpdateStockAnalysis(GetStockAnalysisDataResult data)
        {
            using (var db = GetMSSQLStockDataContext())
                db.InsertOrUpdateStockAnalysisData(
                    data.StockNo,
                    data.Price,
                    data.StockCashDivi,
                    data.DiviRatio,
                    data.DiviType,
                    data.IsPromisingEPS,
                    data.IsGrowingUpEPS,
                    data.IsAlwaysIncomeEPS,
                    data.IsAlwaysPayDivi,
                    data.IsStableDivi,
                    data.IsAlwaysRestoreDivi,
                    data.IsStableOutsideIncome,
                    data.IsStableTotalAmount,
                    data.IsGrowingUpRevenue,
                    data.HasDivi,
                    data.IsRealMode,
                    data.Price5,
                    data.Price6,
                    data.Price7,
                    data.CurrPrice);
        }
        public void InsertOrUpdateStockFinancialReport(GetStockFinancialReportResult data)
        {
            using (var db = GetMSSQLStockDataContext())
                db.InsertOrUpdateStockFinancialReport(
                    data.StockNo,
                    data.Year,
                    data.Season,
                    data.TotalAssets,
                    data.TotalLiability,
                    data.NetWorth,
                    data.NAV,
                    data.Revenue,
                    data.BusinessInterest,
                    data.NetProfitTaxFree,
                    data.EPS,
                    data.BusinessCashflow,
                    data.InvestmentCashflow,
                    data.FinancingCashflow
                    );
        }
        public void InsertOrUpdateStockInterestIssuedInfo(GetStockInterestIssuedInfoResult data)
        {
            using (var db = GetMSSQLStockDataContext())
                db.InsertOrUpdateStockInterestIssuedInfo(
                    data.StockNo,
                    data.Year,
                    data.Season,
                    data.DecisionDate,
                    data.ProfitCashIssued,
                    data.ProfitStockIssued,
                    data.SsrCashIssued,
                    data.SsrStockIssued,
                    data.CapitalReserveCashIssued,
                    data.CapitalReserveStockIssued
                    );
        }
        #endregion

        #region 刪除
        public void DeleteStockPriceHistoryData(string stockNo, DateTime? tradeDate)
        {
            using (var db = GetMSSQLStockDataContext())
                db.DeleteStockPriceHistoryData(stockNo, tradeDate);
        }
        #endregion

        public decimal CaculateStockClosingAveragePrice(string stockNo, DateTime endDate, short period)
        {
            decimal? oAvgClosePrice = null;

            using (var db = GetMSSQLStockDataContext())
                db.CalculateStockPriceAverage(
                    stockNo,
                    endDate,
                    period,
                    ref oAvgClosePrice);

            return oAvgClosePrice ?? 0;
        }
        public void Dispose()
        {
        }
    }
}
