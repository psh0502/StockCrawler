using System.Configuration;

namespace StockCrawler.Dao
{
    internal static class ConnectionStringHelper
    {
        public static string StockConnectionString
        {
            get
            {
#if (DEBUG)
                return @"Data Source=.\SQLEXPRESS;Initial Catalog=Stock;User ID=crawler;password=crawler";
#else
                return ConfigurationManager.ConnectionStrings["StockCrawler.Dao.Properties.Settings.StockConnectionString"]?.ToString();
#endif
            }
        }
    }
}
