using Common.Logging;
using Quartz;
using StockCrawler.Dao;
using StockCrawler.Dao.Schema;
using StockCrawler.Services.StockDailyPrice;
using System;
using System.Configuration;
using System.Reflection;

namespace StockCrawler.Services
{
    public class StockPriceUpdateJob : JobBase, IJob
    {
        private const string CONST_APPSETTING_DAILY_PRICE_COLLECTOR_TYPE = "DailyCollectorType";
#if(UNITTEST)
        public static ILog _logger { get; set; }
#else
        private static readonly ILog _logger = LogManager.GetLogger(typeof(StockPriceUpdateJob));
#endif

        public StockPriceUpdateJob()
            : base()
        {
#if(UNITTEST)
            if (null == _logger)
                _logger = LogManager.GetLogger(typeof(StockPriceUpdateJob));
#endif
        }

        public StockPriceUpdateJob(string collector_type_name)
            : this()
        {
            CollectorTypeName = collector_type_name;
        }

        public string CollectorTypeName { get; private set; }

        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            _logger.InfoFormat("Invoke [{0}]...", MethodBase.GetCurrentMethod().Name);
            if (string.IsNullOrEmpty(CollectorTypeName)) CollectorTypeName = ConfigurationManager.AppSettings[CONST_APPSETTING_DAILY_PRICE_COLLECTOR_TYPE];
            _logger.InfoFormat("[{0}] is going to executing its job by using [{1}].", GetType().FullName, CollectorTypeName);
            try
            {
                StockDataSet.StockPriceHistoryDataTable dt = new StockDataSet.StockPriceHistoryDataTable();
                using (var db = StockDataService.GetServiceInstance())
                {
                    IStockDailyInfoCollector collector = StockDailyInfoCollectorProvider.GetDailyPriceCollector(CollectorTypeName);
                    foreach (var d in db.GetStocks())
                    {
                        _logger.DebugFormat("Retrieving daily price of [{0}] stock.", d.StockNo);
                        StockDailyPriceInfo info = collector.GetStockDailyPriceInfo(d.StockNo);
                        if (null != info)
                        {
                            _logger.Debug(info);
                            db.UpdateStockName(info.StockNo, info.StockName);
                            if (info.Volume > 0)
                            {
                                StockDataSet.StockPriceHistoryRow dr = dt.NewStockPriceHistoryRow();
                                dr.StockNo = info.StockNo;
                                dr.StockDT = info.LastTradeDT.Date;
                                dr.OpenPrice = info.OpenPrice;
                                dr.HighPrice = info.HighPrice;
                                dr.LowPrice = info.LowPrice;
                                dr.ClosePrice = info.ClosePrice;
                                dr.Volume = info.Volume;
                                dr.AdjClosePrice = info.ClosePrice;

                                dt.AddStockPriceHistoryRow(dr);

                                _logger.InfoFormat("Finish the {0} stock daily price retrieving task.", d.StockNo);
                            }
                            else
                            {
                                _logger.WarnFormat("The {0} stock has no volumn today, skip it.", d.StockNo);
                            }
                        }
                    }
                    db.UpdateStockPriceHistoryDataTable(dt);
                }
            }
            catch (Exception Ex)
            {
                _logger.Error("Job executing failed!", Ex);
                throw;
            }
        }

        #endregion
    }

    public class StockDailyInfoCollectorProvider
    {
        private StockDailyInfoCollectorProvider() { }
        public static IStockDailyInfoCollector GetDailyPriceCollector(string class_assembly_qualified_name)
        {
            Type collector_type = Type.GetType(class_assembly_qualified_name);
            IStockDailyInfoCollector collector = (IStockDailyInfoCollector)Activator.CreateInstance(collector_type);
            if (collector == null) throw new NullReferenceException(class_assembly_qualified_name + " is unavailable collector type, please check your setting.");
            return collector;
        }
    }
}