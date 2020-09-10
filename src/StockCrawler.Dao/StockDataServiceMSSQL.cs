﻿using System;
using System.Collections.Generic;

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
        public void UpdateStockPriceHistoryDataTable(IList<GetStockPriceHistoryResult> list)
        {
            using (var db = GetMSSQLStockDataContext())
                foreach (var dr in list)
                    db.InsertOrUpdateStockPriceHistory(
                        dr.StockNo,
                        dr.StockDT,
                        dr.Period,
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
                    info.NetProfitTaxed,
                    info.EPS,
                    info.SEPS,
                    info.ReleaseStockCount);
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
                    info.Remark,
                    info.PE);
        }

        public void GetStockPriceAVG(string stockNo, DateTime begDate, DateTime endDate, short period, int top, out decimal avgClosePrice, out decimal avgOpenPrice, out decimal avgHighPrice, out decimal avgLowPrice, out long sumVolume)
        {
            decimal? oAvgClosePrice = null, 
                oAvgOpenPrice = null, 
                oAgHighPrice = null, 
                oAvgLowPrice = null;
            long? oSumVolume = null;

            using (var db = GetMSSQLStockDataContext())
                db.GetStockPriceAVG(
                    stockNo,
                    begDate,
                    endDate,
                    period,
                    top,
                    ref oAvgClosePrice,
                    ref oAvgOpenPrice,
                    ref oAgHighPrice,
                    ref oAvgLowPrice,
                    ref oSumVolume);

            avgClosePrice = oAvgClosePrice ?? 0;
            avgOpenPrice = oAvgOpenPrice ?? 0;
            avgHighPrice = oAgHighPrice ?? 0;
            avgLowPrice = oAvgLowPrice ?? 0;
            sumVolume = oSumVolume ?? 0;
        }
    }
}
