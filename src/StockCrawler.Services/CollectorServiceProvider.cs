using Microsoft.Practices.Unity.Configuration;
using StockCrawler.Services.Collectors;
using System.Collections.Concurrent;
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
            ETFCollectorDictionary = new ConcurrentDictionary<string, IETFInfoCollector>();
            ETFCollectorDictionary.TryAdd("元大", new YuantaETFCollector());
        }
        internal static IStockForumCollector GetStockForumCollector()
        {
            return _container.Resolve<IStockForumCollector>();
        }
        internal static IStockInterestIssuedCollector GetStockInterestIssuedCollector()
        {
            return _container.Resolve<IStockInterestIssuedCollector>();
        }
        internal static IStockDailyInfoCollector GetStockDailyPriceCollector()
        {
            return _container.Resolve<IStockDailyInfoCollector>();
        }
        private static readonly ConcurrentDictionary<string, IETFInfoCollector> ETFCollectorDictionary = null;
        internal static IETFInfoCollector GetETFInfoCollector(string etfName)
        {
            etfName = TryExtractETFCompnatName(etfName);
            if (!string.IsNullOrEmpty(etfName))
                if (ETFCollectorDictionary.TryGetValue(etfName, out IETFInfoCollector collector))
                    return collector;
            return null;
        }
        private static readonly string[] etfKeywords = { 
            "元大", "富邦", "國泰", "永豐", "群益", "中信", 
            "統一", "新光", "台新", "元富", "永昌", "凱基"
        };
        private static string TryExtractETFCompnatName(string etfName)
        {
            foreach (var keyword in etfKeywords)
                if (etfName.StartsWith(keyword)) return keyword;

            return null;
        }

        internal static IStockBasicInfoCollector GetStockBasicInfoCollector()
        {
            return _container.Resolve<IStockBasicInfoCollector>();
        }
        internal static IStockMonthlyIncomeCollector GetStockMonthlyIncomeCollector()
        {
            return _container.Resolve<IStockMonthlyIncomeCollector>();
        }
        internal static IStockReportCollector GetStockReportCollector()
        {
            return _container.Resolve<IStockReportCollector>();
        }
        internal static IStockMarketNewsCollector GetMarketNewsCollector()
        {
            return _container.Resolve<IStockMarketNewsCollector>();
        }
    }
}
