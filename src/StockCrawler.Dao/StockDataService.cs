using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using TomTang.DbAccess;
using StockCrawler.Dao.Schema;

namespace StockCrawler.Dao
{
    public class StockDataService : DBServiceBase
    {
        protected const string CONST_APP_CONNECTION_KEY = "StockCrawler.Dao.Properties.Settings.StockConnectionString";
        protected static readonly string _currentAssemblyLocation = null;
#if(DEBUG)
        static StockDataService()
        {
            FileInfo file = new FileInfo(Assembly.GetExecutingAssembly().Location);
            _currentAssemblyLocation = file.DirectoryName;
            Debug.WriteLine(string.Format("_currentAssemblyCodebase={0}", _currentAssemblyLocation));
        }

        private StockDataService() :
            base(string.Format(ConfigurationManager.ConnectionStrings[CONST_APP_CONNECTION_KEY].ConnectionString, _currentAssemblyLocation + @"\..\..\..\StockCrawler.UnitTest\database\"), 
            DBOperatorType.OleDBOperator) {}
#else
        private StockDataService() :
            base(ConfigurationManager.ConnectionStrings[CONST_APP_CONNECTION_KEY].ConnectionString, 
            DBOperatorType.OleDBOperator) {}
#endif

        protected DBOperatorBase _db { get { return op; } }
        /// <summary>
        /// Retrieve a new service instance. It's thread-safe.
        /// </summary>
        /// <returns>Database service instance</returns>
        public static StockDataService GetServiceInstance()
        {
            return new StockDataService();
        }

        public StockDataSet.StockDataTable GetStocksSchema()
        {
            return new StockDataSet.StockDataTable();
        }

        public StockDataSet.StockDataTable GetStocks()
        {
            StockDataSet.StockDataTable dt = new StockDataSet.StockDataTable();
            FillDt("SELECT * FROM Stock", dt);
            return dt;
        }

        public StockDataSet.StockPriceHistoryDataTable GetStockPriceHistoryData(string sno, DateTime startDT, DateTime endDT)
        {
            StockDataSet.StockPriceHistoryDataTable dt = new StockDataSet.StockPriceHistoryDataTable();
            FillDt(string.Format("SELECT sph.* FROM StockPriceHistory sph INNER JOIN Stock s ON s.StockID = sph.StockID WHERE s.StockNo = {0} AND StockDT BETWEEN {1} AND {2}", 
                OleDbStrHelper.getParamStr(sno), 
                OleDbStrHelper.getParamStr(startDT), 
                OleDbStrHelper.getParamStr(endDT)), dt);
            return dt;
        }

        public void UpdateStockPriceHistoryDataTable(StockDataSet.StockPriceHistoryDataTable dt)
        {
            _db.BeginTrans();
            try
            {
                foreach (var r in dt)
                {
                    _db.ExecuteCommand.CommandText = string.Format("INSERT INTO StockPriceHistory(StockID, StockDT, OpenPrice, HighPrice, LowPrice, ClosePrice, Volumn, AdjClosePrice) VALUES({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})",
                        OleDbStrHelper.getParamStr(r.StockID), 
                        OleDbStrHelper.getParamStr(r.StockDT), 
                        OleDbStrHelper.getParamStr(r.OpenPrice), 
                        OleDbStrHelper.getParamStr(r.HighPrice), 
                        OleDbStrHelper.getParamStr(r.LowPrice), 
                        OleDbStrHelper.getParamStr(r.ClosePrice), 
                        OleDbStrHelper.getParamStr(r.Volumn), 
                        OleDbStrHelper.getParamStr(r.AdjClosePrice));
                    _db.Execute();
                }
                _db.CommitTrans();
            }
            catch
            {
                _db.RollBack();
                throw;
            }
            finally
            {
                _db.Close();
            }
        }
        /// <summary>
        /// Retrieve the close price of the specified stock by the specified date.
        /// </summary>
        /// <param name="sNO">Stock number</param>
        /// <param name="specifiedDT">Indicate the specified date</param>
        /// <returns>The close price on the specified date.</returns>
        public double GetStockPriceByDate(string sNO, DateTime specifiedDT)
        {
            _db.ExecuteCommand.CommandText = string.Format("SELECT TOP 1 sph.ClosePrice FROM Stock s INNER JOIN StockPriceHistory sph ON s.StockID = sph.StockID WHERE s.StockNo = {0} AND sph.StockDT < {1} ORDER BY sph.StockDT DESC",
                OleDbStrHelper.getParamStr(sNO), 
                OleDbStrHelper.getParamStr(specifiedDT));
            Debug.WriteLine("GetStockPriceByDate = " + _db.ExecuteCommand.CommandText);
            object resultFromDB = _db.ExecuteScalar();
            return Convert.ToDouble((DBNull.Value == resultFromDB) ? 0 : resultFromDB);
        }
        /// <summary>
        /// Retrieve the price data row of the specified stock by the specified date.
        /// </summary>
        /// <param name="sNO">Stock number</param>
        /// <param name="specifiedDT">Indicate the specified date</param>
        /// <returns>The price datarow on the specified date.</returns>
        public StockDataSet.StockPriceHistoryRow GetStockPriceDataRowByDate(string sNO, DateTime specifiedDT)
        {
            StockDataSet.StockPriceHistoryDataTable dt = new StockDataSet.StockPriceHistoryDataTable();
            FillDt(string.Format("SELECT TOP 1 sph.* FROM Stock s INNER JOIN StockPriceHistory sph ON s.StockID = sph.StockID WHERE s.StockNo = {0} AND sph.StockDT < {1} ORDER BY sph.StockDT DESC",
                OleDbStrHelper.getParamStr(sNO),
                OleDbStrHelper.getParamStr(specifiedDT)), dt);
            return (dt.Count > 0) ? dt[0] : null;
        }
        /// <summary>
        /// Retrieve the MAX high price of the specified stock in the specified period.
        /// </summary>
        /// <param name="sNO">Stock number</param>
        /// <param name="specifiedDT">Indicate the specified date as the begining of period</param>
        /// <returns>The MAX close price since the specified date.</returns>
        public double GetStockMaxPriceByPeriod(string sNO, DateTime beginDT, DateTime endDT)
        {
            _db.ExecuteCommand.CommandText = string.Format("SELECT MAX(sph.HighPrice) FROM Stock s INNER JOIN StockPriceHistory sph ON s.StockID = sph.StockID WHERE s.StockNo = {0} AND sph.StockDT BETWEEN {1} AND {2}",
                OleDbStrHelper.getParamStr(sNO),
                OleDbStrHelper.getParamStr(beginDT),
                OleDbStrHelper.getParamStr(endDT));
            Debug.WriteLine("GetStockMaxPriceByPeriod = " + _db.ExecuteCommand.CommandText);
            object resultFromDB = _db.ExecuteScalar();
            return Convert.ToDouble((DBNull.Value == resultFromDB) ? 0 : resultFromDB);
        }
        /// <summary>
        /// Retrieve the average close price of the specified stock since the specified date.
        /// </summary>
        /// <param name="sNO">Stock number</param>
        /// <param name="specifiedDT">Indicate the specified date as the begining of period</param>
        /// <returns>The average close price since the specified date.</returns>
        public double GetStockAvgPriceByDate(string sNO, DateTime specifiedDT)
        {
            _db.ExecuteCommand.CommandText = string.Format("SELECT AVG(sph.ClosePrice) FROM Stock s INNER JOIN StockPriceHistory sph ON s.StockID = sph.StockID WHERE s.StockNo = {0} AND sph.StockDT > {1}",
                OleDbStrHelper.getParamStr(sNO), 
                OleDbStrHelper.getParamStr(specifiedDT));
            Debug.WriteLine("GetStockAvgPriceByDate = " + _db.ExecuteCommand.CommandText);
            object resultFromDB = _db.ExecuteScalar();
            return Convert.ToDouble((DBNull.Value == resultFromDB) ? 0 : resultFromDB);
        }
    }
}
