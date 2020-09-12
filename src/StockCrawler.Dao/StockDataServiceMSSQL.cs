using System;
using System.Collections.Generic;
using System.Linq;

namespace StockCrawler.Dao
{
    internal class StockDataServiceMSSQL : IStockDataService
    {
        public IEnumerable<GetStocksResult> GetStocks()
        {
            using (var db = GetMSSQLStockDataContext())
                return db.GetStocks().ToList();
        }
        public void InsertOrUpdateStockPriceHistory(IEnumerable<GetStockPriceHistoryResult> list)
        {
            using (var db = GetMSSQLStockDataContext())
                foreach (var d in list)
                    db.InsertOrUpdateStockPriceHistory(
                        d.StockNo,
                        d.StockDT,
                        d.Period,
                        d.OpenPrice,
                        d.HighPrice,
                        d.LowPrice,
                        d.ClosePrice,
                        d.Volume,
                        d.AdjClosePrice);
        }
        public void RenewStockList(IEnumerable<GetStocksResult> list)
        {
            using (var db = GetMSSQLStockDataContext())
            {
                db.DisableAllStocks();
                foreach (var dr in list)
                    db.InsertOrUpdateStock(dr.StockNo, dr.StockName);
            }
        }
        private static StockDataContext GetMSSQLStockDataContext()
        {
            return new StockDataContext(ConnectionStringHelper.StockConnectionString);
        }
        public void Dispose()
        {
        }
        public void DeleteStockPriceHistoryData(string stockNo, DateTime? tradeDate)
        {
            using (var db = GetMSSQLStockDataContext())
                db.DeleteStockPriceHistoryData(stockNo, tradeDate);
        }
        public void UpdateStockName(string stockNo, string stockName)
        {
            using (var db = GetMSSQLStockDataContext())
                db.InsertOrUpdateStock(stockNo, stockName);
        }
        public void InsertOrUpdateStockBasicInfo(IEnumerable<GetStockBasicInfoResult> data)
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
                    (info.MarketValue == 0) ? null : (decimal?)info.MarketValue,
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
                    info.EPS,
                    info.SEPS,
                    info.ReleaseStockCount);
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
                    info.Remark,
                    info.PE);
        }
        public decimal GetStockPriceAVG(string stockNo, DateTime endDate, short period)
        {
            decimal? oAvgClosePrice = null;

            using (var db = GetMSSQLStockDataContext())
                db.GetStockPriceAVG(
                    stockNo,
                    endDate,
                    period,
                    ref oAvgClosePrice);

            return oAvgClosePrice ?? 0;
        }
        public IEnumerable<GetStockPeriodPriceResult> GetStockPeriodPrice(string stockNo, DateTime bgnDate, DateTime endDate)
        {
            using (var db = GetMSSQLStockDataContext())
                return db.GetStockPeriodPrice(stockNo, bgnDate, endDate).ToList();
        }
        public void InsertOrUpdateStockAveragePrice(IEnumerable<(string StockNo, DateTime StockDT, short Period, decimal AveragePrice)> avgPriceList)
        {
            using (var db = new StockDataContext(ConnectionStringHelper.StockConnectionString))
                foreach (var info in avgPriceList)
                    db.InsertOrUpdateStockPriceAVG(
                        info.StockNo,
                        info.StockDT,
                        info.Period,
                        info.AveragePrice);
        }
    }
}
