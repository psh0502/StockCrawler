using StockCrawler.Dao.Schema;
using System;
using System.Collections.Generic;

namespace StockCrawler.Dao
{
    internal class StockDataServiceMSSQL : IStockDataService
    {
        public StockDataSet.StockDataTable GetStocks()
        {
            StockDataSet.StockDataTable dt = new StockDataSet.StockDataTable();
            using (var db = GetMSSQLStockDataContext())
            {
                foreach (var d in db.GetStocks())
                {
                    var dr = dt.NewStockRow();
                    dr.Enable = d.Enable;
                    dr.StockName = d.StockName;
                    dr.StockNo = d.StockNo;

                    dt.AddStockRow(dr);
                }
            }
            return dt;
        }

        public void UpdateStockPriceHistoryDataTable(StockDataSet.StockPriceHistoryDataTable dt)
        {
            using (var db = GetMSSQLStockDataContext())
                foreach (var dr in dt)
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

        public void RenewStockList(StockDataSet.StockDataTable dt)
        {
            using (var db = GetMSSQLStockDataContext())
            {
                db.DisableAllStocks();
                foreach (var dr in dt)
                    db.InsertOrUpdateStock(dr.StockNo, dr.StockName);
            }
        }

        private static StockDataContext GetMSSQLStockDataContext()
        {
#if(UNITTEST)
            return new StockDataContext(@"Data Source=.\SQLEXPRESS;Initial Catalog=Stock;User ID=crawler;password=crawler");
#else
            return new StockDataContext();
#endif
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
        public void UpdateStockBasicInfo(GetStockBasicInfoResult d)
        {
            using (var db = GetMSSQLStockDataContext())
                db.InsertOrUpdateStockBasicInfo(
                    d.StockNo,
                    d.Category,
                    d.CompanyName,
                    d.CompanyID,
                    d.BuildDate,
                    d.PublishDate,
                    d.Capital,
                    d.MarketValue,
                    d.ReleaseStockCount,
                    d.Chairman,
                    d.CEO,
                    d.Url,
                    d.Businiess);
        }

        public void UpdateStockFinaniceCashflowReport(GetStockReportCashFlowResult d)
        {
            using (var db = GetMSSQLStockDataContext())
                db.InsertOrUpdateStockReportCashFlow(
                    d.StockNo,
                    d.Year,
                    d.Season,
                    d.Depreciation,
                    d.AmortizationFee,
                    d.BusinessCashflow,
                    d.InvestmentCashflow,
                    d.FinancingCashflow,
                    d.CapitalExpenditures,
                    d.FreeCashflow,
                    d.NetCashflow);
        }
    }
}
