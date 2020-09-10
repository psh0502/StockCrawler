
using System.Configuration;

namespace StockCrawler.Services
{
    public abstract class JobBase
    {
        protected static readonly int _breakInternval = int.Parse(ConfigurationManager.AppSettings["CollectorBreakInternval"] ?? "0");
    }
}
