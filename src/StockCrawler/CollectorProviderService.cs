using Common.Logging;
using StockCrawler.Services.StockBasicInfo;
using StockCrawler.Services.StockDailyPrice;
using StockCrawler.Services.StockFinanceReport;
using System;
using System.Configuration;

namespace StockCrawler.Services
{
    internal static class CollectorProviderService
    {
        private const string CONST_APPSETTING_FINANIC_REPORT_COLLECTOR_TYPE = "FinanceReportCollectorType";
        private const string CONST_APPSETTING_BASIC_INFO_COLLECTOR_TYPE = "BasicInfoCollectorType";
        private const string CONST_APPSETTING_DAILY_PRICE_COLLECTOR_TYPE = "DailyCollectorType";
        private static readonly ILog _logger = LogManager.GetLogger(typeof(CollectorProviderService));

        public static IStockDailyInfoCollector GetDailyPriceCollector(string classAssemblyQualifiedName)
        {
            return GetCollectorInstance<IStockDailyInfoCollector>(classAssemblyQualifiedName);
        }
        public static IStockBasicInfoCollector GetBasicInfoCollector(string classAssemblyQualifiedName)
        {
            return GetCollectorInstance<IStockBasicInfoCollector>(classAssemblyQualifiedName);
        }
        public static IStockDailyInfoCollector GetDailyPriceCollector()
        {   
            return GetDailyPriceCollector(ConfigurationManager.AppSettings[CONST_APPSETTING_DAILY_PRICE_COLLECTOR_TYPE]);
        }
        public static IStockBasicInfoCollector GetBasicInfoCollector()
        {
            return GetBasicInfoCollector(ConfigurationManager.AppSettings[CONST_APPSETTING_BASIC_INFO_COLLECTOR_TYPE]);
        }
        public static IStockFinanceReportCashFlowCollector GetFinanceReportCashFlowCollector()
        {
            return GetFinanceReportCashFlowCollector(ConfigurationManager.AppSettings[CONST_APPSETTING_FINANIC_REPORT_COLLECTOR_TYPE]);
        }
        public static IStockFinanceReportCashFlowCollector GetFinanceReportCashFlowCollector(string classAssemblyQualifiedName)
        {
            return GetCollectorInstance<IStockFinanceReportCashFlowCollector>(classAssemblyQualifiedName);
        }
        private static T GetCollectorInstance<T>(string classAssemblyQualifiedName)
        {
            if (string.IsNullOrEmpty(classAssemblyQualifiedName)) throw new ArgumentNullException("classAssemblyQualifiedName", "No value passed in.");
            var collector_type = Type.GetType(classAssemblyQualifiedName);
            var collector = (T)Activator.CreateInstance(collector_type);
            if (collector == null) throw new NullReferenceException(classAssemblyQualifiedName + " is unavailable collector type, please check your setting.");
            _logger.InfoFormat("Going to executing its job by using [{0}].", classAssemblyQualifiedName);
            return collector;
        }
    }
}
