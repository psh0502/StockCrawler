using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace StockCrawler.Dao
{
    internal class StockDataServiceMSSQL : IStockDataService
    {
        #region 取得資料
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
        public GetStockReportIncomeResult[] GetStockReportIncome(int top, string stockNo, short year, short season)
        {
            using (var db = GetMSSQLStockDataContext())
                return db.GetStockReportIncome(top, stockNo, year, season).ToArray();
        }
        public GetStockReportCashFlowResult[] GetStockReportCashFlow(int top, string stockNo, short year, short season)
        {
            using (var db = GetMSSQLStockDataContext())
                return db.GetStockReportCashFlow(top, stockNo, year, season).ToArray();
        }
        public GetStockReportBalanceResult[] GetStockReportBalance(int top, string stockNo, short year, short season)
        {
            using (var db = GetMSSQLStockDataContext())
                return db.GetStockReportBalance(top, stockNo, year, season).ToArray();
        }
        public GetStockReportMonthlyNetProfitTaxedResult[] GetStockReportMonthlyNetProfitTaxed(int top, string stockNo, short year, short month)
        {
            using (var db = GetMSSQLStockDataContext())
                return db.GetStockReportMonthlyNetProfitTaxed(top, stockNo, year, month).ToArray();
        }
        public GetStockBasicInfoResult GetStockBasicInfo(string stockNo)
        {
            using (var db = GetMSSQLStockDataContext())
                return db.GetStockBasicInfo(stockNo).SingleOrDefault();
        }
        public GetLazyStockDataResult GetLazyStockData(string stockNo)
        {
            using (var db = GetMSSQLStockDataContext())
                return db.GetLazyStockData(stockNo).SingleOrDefault();
        }
        #endregion

        #region 新增修改
        public void InsertStockForumData(IList<(GetStockForumDataResult forum, IList<GetStocksResult> stock)> data)
        {
            using (var db = GetMSSQLStockDataContext())
            {
                db.Transaction = db.Connection.BeginTransaction();
                try
                {
                    foreach (var d in data)
                    {
                        long? id = 0;
                        db.InsertStockForums(d.forum.Source, d.forum.Subject, d.forum.Meta, d.forum.Url, d.forum.ArticleDate, ref id);
                        d.forum.ID = id ?? 0;
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
                    catch { }
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
                    }catch(SqlException)
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
                        (d.OpenPrice == 0) ? 0 : d.DeltaPrice / d.OpenPrice,
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
        private static StockDataContext GetMSSQLStockDataContext()
        {
            return new StockDataContext(ConnectionStringHelper.StockConnectionString);
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
        public void InsertOrUpdateStockCashflowReport(GetStockReportCashFlowResult info)
        {
            using (var db = GetMSSQLStockDataContext())
                db.InsertOrUpdateStockReportCashFlow(
                    info.StockNo,
                    info.Year,
                    info.Season,
                    info.Depreciation,
                    info.AmortizationFee,
                    info.BusinessCashflow,
                    info.InvestmentCashflow,
                    info.FinancingCashflow,
                    info.CapitalExpenditures,
                    info.FreeCashflow,
                    info.NetCashflow);
        }
        public void InsertOrUpdateStockIncomeReport(GetStockReportIncomeResult info)
        {
            using (var db = GetMSSQLStockDataContext())
                db.InsertOrUpdateStockReportIncome(
                    info.StockNo,
                    info.Year,
                    info.Season,
                    info.Revenue,
                    info.GrossProfit,
                    info.SalesExpense,
                    info.ManagementCost,
                    info.RDExpense,
                    info.OperatingExpenses,
                    info.BusinessInterest,
                    info.NetProfitTaxFree,
                    info.NetProfitTaxed,
                    info.EPS);
        }
        public void InsertOrUpdateStockBalanceReport(GetStockReportBalanceResult info)
        {
            using (var db = GetMSSQLStockDataContext())
            {
                db.InsertOrUpdateStockReportBalance(
                    info.StockNo,
                    info.Year,
                    info.Season,
                    info.CashAndEquivalents,
                    info.ShortInvestments,
                    info.BillsReceivable,
                    info.Stock,
                    info.OtherCurrentAssets,
                    info.CurrentAssets,
                    info.LongInvestment,
                    info.FixedAssets,
                    info.OtherAssets,
                    info.TotalAssets,
                    info.ShortLoan,
                    info.ShortBillsPayable,
                    info.AccountsAndBillsPayable,
                    info.AdvenceReceipt,
                    info.LongLiabilitiesWithinOneYear,
                    info.OtherCurrentLiabilities,
                    info.CurrentLiabilities,
                    info.LongLiabilities,
                    info.OtherLiabilities,
                    info.TotalLiability,
                    info.NetWorth,
                    info.NAV);
            }
        }
        public void InsertOrUpdateStockMonthlyNetProfitTaxedReport(GetStockReportMonthlyNetProfitTaxedResult info)
        {
            using (var db = GetMSSQLStockDataContext())
                db.InsertOrUpdateStockReportMonthlyNetProfitTaxed(
                    info.StockNo,
                    info.Year,
                    info.Month,
                    info.NetProfitTaxed,
                    info.LastYearNetProfitTaxed,
                    info.Delta,
                    info.DeltaPercent,
                    info.ThisYearTillThisMonth,
                    info.LastYearTillThisMonth,
                    info.TillThisMonthDelta,
                    info.TillThisMonthDeltaPercent,
                    info.Remark);
        }
        public void InsertOrUpdateStockAveragePrice((string stockNo, DateTime stockDT, short period, decimal averagePrice)[] avgPriceList)
        {
            using (var db = new StockDataContext(ConnectionStringHelper.StockConnectionString))
                foreach (var info in avgPriceList)
                    db.InsertOrUpdateStockAveragePrice(
                        info.stockNo,
                        info.stockDT,
                        info.period,
                        info.averagePrice);
        }
        public void InsertOrUpdateLazyStock(GetLazyStockDataResult data)
        {
            using (var db = new StockDataContext(ConnectionStringHelper.StockConnectionString))
                db.InsertOrUpdateLazyStockData(
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
