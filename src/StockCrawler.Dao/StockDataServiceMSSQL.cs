using System;
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
