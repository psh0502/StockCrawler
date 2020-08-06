using Common.Logging;
using StockCrawler.Services.StockBasicInfo;
using StockCrawler.Services.StockDailyPrice;
using System;
using System.Configuration;

namespace StockCrawler.Services
{
    internal static class CollectorProviderService
    {
        private const string CONST_APPSETTING_BASIC_INFO_COLLECTOR_TYPE = "BasicInfoCollectorType";
        private const string CONST_APPSETTING_DAILY_PRICE_COLLECTOR_TYPE = "DailyCollectorType";
        private static readonly ILog _logger = LogManager.GetLogger(typeof(CollectorProviderService));

        public static IStockDailyInfoCollector GetDailyPriceCollector(string classAssemblyQualifiedName)
        {
            if (string.IsNullOrEmpty(classAssemblyQualifiedName)) throw new ArgumentNullException("classAssemblyQualifiedName", "No value passed in.");

            var collector_type = Type.GetType(classAssemblyQualifiedName);
            var collector = (IStockDailyInfoCollector)Activator.CreateInstance(collector_type);
            if (collector == null) throw new NullReferenceException(classAssemblyQualifiedName + " is unavailable collector type, please check your setting.");
            _logger.InfoFormat("Going to executing its job by using [{0}].", classAssemblyQualifiedName);
            return collector;
        }
        public static IStockBasicInfoCollector GetBasicInfoCollector(string classAssemblyQualifiedName)
        {
            if (string.IsNullOrEmpty(classAssemblyQualifiedName)) throw new ArgumentNullException("classAssemblyQualifiedName", "No value passed in.");

            var collector_type = Type.GetType(classAssemblyQualifiedName);
            var collector = (IStockBasicInfoCollector)Activator.CreateInstance(collector_type);
            if (collector == null) throw new NullReferenceException(classAssemblyQualifiedName + " is unavailable collector type, please check your setting.");
            _logger.InfoFormat("Going to executing its job by using [{0}].", classAssemblyQualifiedName);
            return collector;
        }
        public static IStockDailyInfoCollector GetDailyPriceCollector()
        {   
            return GetDailyPriceCollector(ConfigurationManager.AppSettings[CONST_APPSETTING_DAILY_PRICE_COLLECTOR_TYPE]);
        }

        public static IStockBasicInfoCollector GetBasicInfoCollector()
        {
            return GetBasicInfoCollector(ConfigurationManager.AppSettings[CONST_APPSETTING_BASIC_INFO_COLLECTOR_TYPE]);
        }
    }
}
