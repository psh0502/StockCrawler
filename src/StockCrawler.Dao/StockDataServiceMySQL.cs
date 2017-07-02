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

        public StockDataSet.StockDataTable GetStocks()
        {
            StockDataSet.StockDataTable dt = new StockDataSet.StockDataTable();
            using (var conn = GetMySqlConnection())
            {
                using (var da = new MySqlDataAdapter())
                {
                    da.SelectCommand = conn.CreateCommand();
                    da.SelectCommand.CommandText = "GetStocks";
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public void UpdateStockPriceHistoryDataTable(StockDataSet.StockPriceHistoryDataTable dt)
        {
            using (var conn = GetMySqlConnection())
            {
                using (var da = new MySqlDataAdapter())
                {
                    da.InsertCommand = conn.CreateCommand();
                    da.InsertCommand.CommandText = "InsertStockPriceHistoryData";
                    da.InsertCommand.CommandType = CommandType.StoredProcedure;
                    da.InsertCommand.Parameters.Add("@pStockID", MySqlDbType.Int32, 11, "StockID");
                    da.InsertCommand.Parameters.Add("@pStockDT", MySqlDbType.DateTime, 20, "StockDT");
                    da.InsertCommand.Parameters.Add("@pOpenPrice", MySqlDbType.Decimal, 50, "OpenPrice");
                    da.InsertCommand.Parameters.Add("@pHighPrice", MySqlDbType.Decimal, 50, "HighPrice");
                    da.InsertCommand.Parameters.Add("@pLowPrice", MySqlDbType.Decimal, 50, "LowPrice");
                    da.InsertCommand.Parameters.Add("@pClosePrice", MySqlDbType.Decimal, 50, "ClosePrice");
                    da.InsertCommand.Parameters.Add("@pVolume", MySqlDbType.Int64, 11, "Volume");
                    da.InsertCommand.Parameters.Add("@pAdjClosePrice", MySqlDbType.Decimal, 50, "AdjClosePrice");
                    da.Update(dt);
                }
            }
        }

        public void RenewStockList(StockDataSet.StockDataTable dt)
        {
            using (var conn = GetMySqlConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "DisableAllStocks";
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                using (var da = new MySqlDataAdapter())
                {
                    da.InsertCommand = conn.CreateCommand();
                    da.InsertCommand.CommandText = "InsertOrUpdateStockList";
                    da.InsertCommand.CommandType = CommandType.StoredProcedure;
                    da.InsertCommand.Parameters.Add("@pStockNo", MySqlDbType.VarChar, 10, "StockNo");
                    da.InsertCommand.Parameters.Add("@pStockName", MySqlDbType.VarChar, 50, "StockName");
                    da.Update(dt);
                }
            }
        }

        private static MySqlConnection GetMySqlConnection()
        {
#if(UNITTEST)
            return new MySqlConnection("server=localhost;uid=tester;pwd=12345;database=stock");
#else
            return new MySqlConnection(ConfigurationManager.ConnectionStrings[CONST_APP_CONNECTION_KEY].ConnectionString);
#endif
        }

        public void Dispose()
        {
        }
    }
}
