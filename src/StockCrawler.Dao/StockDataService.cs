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
        /// <param name="dbType">DB service by the specified DB platform</param>
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
        /// <summary>
        /// Retrieve a new service instance. It's thread-safe.
        /// </summary>
        /// <param name="dbType">DB service by the specified DB platform</param>
        /// <exception cref="System.NotImplementedException">Not support the specified database yet.</exception>
        /// <returns>Database service instance</returns>
        public static IStockDataService GetServiceInstance(string dbType = "MYSQL")
        {
            EnumDBType db_type = EnumDBType.MSSQL;
            Enum.TryParse<EnumDBType>(dbType, out db_type);
            return GetServiceInstance(db_type);
        }
    }
}
