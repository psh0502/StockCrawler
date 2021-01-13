using Microsoft.Practices.Unity.Configuration;
using Unity;

namespace StockCrawler.Dao
{
    public abstract class RepositoryProvider
    {
        private static readonly UnityContainer _container = new UnityContainer();
        static RepositoryProvider()
        {
            _container.LoadConfiguration();
        }
        /// <summary>
        /// Retrieve a new service instance. It's thread-safe.
        /// </summary>
        /// <returns>Database Repository instance</returns>
        public static IRepository GetRepositoryInstance()
        {
            return _container.Resolve<IRepository>();
        }
    }
}
