using Microsoft.Practices.Unity.Configuration;
using StockCrawler.Services.StockBasicInfo;
using StockCrawler.Services.StockDailyPrice;
using StockCrawler.Services.StockFinanceReport;
using StockCrawler.Services.StockHistoryPrice;
using Unity;

namespace StockCrawler.Services
{
    internal static class CollectorProviderService
    {
        private static readonly UnityContainer _container = new UnityContainer();
        static CollectorProviderService()
        {
            _container.LoadConfiguration();
        }

        public static IStockDailyInfoCollector GetStockDailyPriceCollector()
        {
            return _container.Resolve<IStockDailyInfoCollector>();
        }
        public static IStockBasicInfoCollector GetStockBasicInfoCollector()
        {
            return _container.Resolve<IStockBasicInfoCollector>();
        }
        public static IStockReportCollector GetStockReportCollector()
        {
            return _container.Resolve<IStockReportCollector>();
        }
        public static IStockHistoryPriceCollector GetStockHistoryPriceCollector()
        {
            return _container.Resolve<IStockHistoryPriceCollector>();
        }

    }
}
