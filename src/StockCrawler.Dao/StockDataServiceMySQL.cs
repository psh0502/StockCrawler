using log4net;
using MySql.Data.MySqlClient;
using StockCrawler.Dao.Schema;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace StockCrawler.Dao
{
    internal class StockDataServiceMySQL : IStockDataService
    {
        protected const string CONST_APP_CONNECTION_KEY = "StockCrawler.Dao.Properties.Settings.StockConnectionString";
        private static readonly ILog _logger = LogManager.GetLogger(typeof(StockDataServiceMySQL));
        public double GetStockAvgPriceByDate(string sNO, DateTime specifiedDT)
        {
            throw new NotImplementedException();
        }

        public double GetStockMaxPriceByPeriod(string sNO, DateTime beginDT, DateTime endDT)
        {
            throw new NotImplementedException();
        }

        public double GetStockPriceByDate(string sNO, DateTime specifiedDT)
        {
            throw new NotImplementedException();
        }

        public StockDataSet.StockPriceHistoryRow GetStockPriceDataRowByDate(string sNO, DateTime specifiedDT)
        {
            throw new NotImplementedException();
        }

        public StockDataSet.StockPriceHistoryDataTable GetStockPriceHistoryData(string sno, DateTime startDT, DateTime endDT)
        {
            throw new NotImplementedException();
        }

        public StockDataSet.StockDataTable GetStocks()
        {
            throw new NotImplementedException();
        }

        public StockDataSet.StockDataTable GetStocksSchema()
        {
            throw new NotImplementedException();
        }

        public void UpdateStockPriceHistoryDataTable(StockDataSet.StockPriceHistoryDataTable dt)
        {
            throw new NotImplementedException();
        }

        public void RenewStockList(StockDataSet.StockDataTable dt)
        {
            using (var conn = GetMySqlConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "DisableAllStocks";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                using (var da = new MySqlDataAdapter())
                {
                    da.InsertCommand = conn.CreateCommand();
                    da.InsertCommand.CommandText = "";
                    da.InsertCommand.CommandType = CommandType.StoredProcedure;
                    da.Update(dt);
                }
            }
        }

        private static MySqlConnection GetMySqlConnection()
        {
            return new MySqlConnection(ConfigurationManager.ConnectionStrings[CONST_APP_CONNECTION_KEY].ConnectionString);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
