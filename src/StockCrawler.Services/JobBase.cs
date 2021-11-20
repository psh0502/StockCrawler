using StockCrawler.Dao;
using System.Configuration;

namespace StockCrawler.Services
{
    public abstract class JobBase
    {
        protected static readonly int _breakInternval = int.Parse(ConfigurationManager.AppSettings["CollectorBreakInternval"] ?? "0");

        protected static IRepository GetDB()
        {
            return RepositoryProvider.GetRepositoryInstance();
        }
    }
}
