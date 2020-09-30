
using System.Configuration;

namespace StockCrawler.Services
{
    public abstract class JobBase
    {
        protected static readonly int _breakInternval = int.Parse(ConfigurationManager.AppSettings["CollectorBreakInternval"] ?? "0");
        protected static short GetTaiwanYear()
        {
            return GetTaiwanYear(SystemTime.Today.Year);
        }
        protected static short GetTaiwanYear(int westernYear)
        {
            return (short)(westernYear - 1911);
        }
        protected static short GetSeason()
        {
            return GetSeason(SystemTime.Today.Month);
        }
        protected static short GetSeason(int month)
        {
            return (short)(month / 3 + 1);
        }
    }
}
