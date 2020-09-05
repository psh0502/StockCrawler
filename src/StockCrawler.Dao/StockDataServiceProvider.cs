using Microsoft.Practices.Unity.Configuration;
using Unity;

namespace StockCrawler.Dao
{
    public abstract class StockDataServiceProvider
    {
        private static readonly UnityContainer _container = new UnityContainer();
        static StockDataServiceProvider()
        {
            _container.LoadConfiguration();
        }
        /// <summary>
        /// Retrieve a new service instance. It's thread-safe.
        /// </summary>
        /// <returns>Database service instance</returns>
        public static IStockDataService GetServiceInstance()
        {
            return _container.Resolve<IStockDataService>();
        }
    }
}
