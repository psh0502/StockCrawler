using Microsoft.Practices.Unity.Configuration;
using StockCrawler.Services.Collectors;
using Unity;

namespace StockCrawler.Services
{
    /// <summary>
    /// 提供各種 collector 的服務器
    /// </summary>
    public static class CollectorServiceProvider
    {
        private static readonly UnityContainer _container = new UnityContainer();
        static CollectorServiceProvider()
        {
            _container.LoadConfiguration();
        }
        public static IStockForumCollector GetStockForumCollector()
        {
            return _container.Resolve<IStockForumCollector>();
        }
        internal static IStockInterestIssuedCollector GetStockInterestIssuedCollector()
        {
            return _container.Resolve<IStockInterestIssuedCollector>();
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
        public static IStockMarketNewsCollector GetMarketNewsCollector()
        {
            return _container.Resolve<IStockMarketNewsCollector>();
        }
    }
}
