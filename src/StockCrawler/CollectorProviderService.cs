using Microsoft.Practices.Unity.Configuration;
using StockCrawler.Services.StockBasicInfo;
using StockCrawler.Services.StockDailyPrice;
using StockCrawler.Services.StockFinanceReport;
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

        public static IStockDailyInfoCollector GetDailyPriceCollector()
        {
            return _container.Resolve<IStockDailyInfoCollector>();
        }
        public static IStockBasicInfoCollector GetBasicInfoCollector()
        {
            return _container.Resolve<IStockBasicInfoCollector>();
        }
        public static IStockReportCollector GetFinanceReportCashFlowCollector()
        {
            return _container.Resolve<IStockReportCollector>();
        }
    }
}
