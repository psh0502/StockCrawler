using System;

namespace StockCrawler.Dao
{
    public abstract class StockDataService
    {
        public enum EnumDBType
        {
            ACCESS,
            MYSQL,
            MSSQL
        }

        /// <summary>
        /// Retrieve a new service instance. It's thread-safe.
        /// </summary>
        /// <exception cref="System.NotImplementedException">Not support the specified database yet.</exception>
        /// <returns>Database service instance</returns>
        public static IStockDataService GetServiceInstance(EnumDBType dbType)
        {
            switch (dbType)
            {
                case EnumDBType.ACCESS:
                    return new StockDataServiceACCESS();
                case EnumDBType.MYSQL:
                    return new StockDataServiceMySQL();
                default:
                    throw new NotImplementedException(dbType.ToString());
            }
        }
    }
}
