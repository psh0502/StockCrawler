﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace StockCrawler.Dao
{
    internal class StockDataServiceMSSQL : IStockDataService
    {
        public IList<GetStocksResult> GetStocks()
        {
            List<GetStocksResult> dt = new List<GetStocksResult>();
            using (var db = GetMSSQLStockDataContext())
            {
                foreach (var d in db.GetStocks())
                {
                    var dr = new GetStocksResult
                    {
                        Enable = d.Enable,
                        StockName = d.StockName,
                        StockNo = d.StockNo
                    };

                    dt.Add(dr);
                }
            }
            return dt;
        }
        public void UpdateStockPriceHistoryDataTable(IList<GetStockHistoryResult> list)
        {
            using (var db = GetMSSQLStockDataContext())
                foreach (var dr in list)
                    db.InsertStockPriceHistoryData(
                        dr.StockNo,
                        dr.StockDT,
                        dr.OpenPrice,
                        dr.HighPrice,
                        dr.LowPrice,
                        dr.ClosePrice,
                        dr.Volume,
                        dr.AdjClosePrice);
        }
        public void RenewStockList(IList<GetStocksResult> list)
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
        public void UpdateStockBasicInfo(IEnumerable<GetStockBasicInfoResult> data)
        {
            foreach (var d in data)
                UpdateStockBasicInfo(d);
        }
        public void UpdateStockBasicInfo(GetStockBasicInfoResult info)
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
        public void UpdateStockCashflowReport(GetStockReportCashFlowResult info)
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
        public void UpdateStockIncomeReport(GetStockReportIncomeResult info)
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
                    info.NetProfitTaxed);
        }
        public void UpdateStockBalanceReport(GetStockReportBalanceResult info)
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
                    info.NetWorth);
            }
        }
        public void UpdateStockMonthlyNetProfitTaxedReport(GetStockReportMonthlyNetProfitTaxedResult info)
        {
            using (var db = new StockDataContext(ConnectionStringHelper.StockConnectionString))
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
        public void UpdateStockReportPerSeason(GetStockReportPerSeasonResult info)
        {
            using (var db = new StockDataContext(ConnectionStringHelper.StockConnectionString))
            {
                long releaseStockCount = db.GetStockBasicInfo(info.StockNo).First().ReleaseStockCount;
                if (releaseStockCount > 0)
                {
                    info.EPS /= releaseStockCount;
                    info.NetValue /= releaseStockCount;
                }

                db.InsertOrUpdateStockReportPerSeason(
                    info.StockNo,
                    info.Year,
                    info.Season,
                    info.EPS == 0 ? null : (decimal?)info.EPS,
                    info.NetValue == 0 ? null : (decimal?)info.NetValue);
            }
        }
    }
}
