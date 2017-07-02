using Common.Logging;
using Quartz;
using StockCrawler.Dao;
using StockCrawler.Dao.Schema;
using StockCrawler.Services.StockDailyPrice;
using System;
using System.Configuration;

namespace StockCrawler.Services
{
    public class StockPriceUpdateJob : JobBase, IJob
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(StockPriceUpdateJob));
        private const string CONST_APPSETTING_DAILY_PRICE_COLLECTOR_TYPE = "DailyCollectorType";
#if(DEBUG)
        private static readonly string _dbType = "MYSQL";
#else
        private static readonly string _dbType = ConfigurationManager.AppSettings["DB_TYPE"];
#endif

        public StockPriceUpdateJob() : base() { }
        public StockPriceUpdateJob(string collector_type_name)
            : base()
        {
            CollectorTypeName = collector_type_name;
        }

        public string CollectorTypeName { get; private set; }

        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            if (string.IsNullOrEmpty(CollectorTypeName)) CollectorTypeName = ConfigurationManager.AppSettings[CONST_APPSETTING_DAILY_PRICE_COLLECTOR_TYPE];
            _logger.InfoFormat("[{0}] is going to executing its job by using [{1}].", GetType().FullName, CollectorTypeName);
            try
            {
                StockDataSet.StockPriceHistoryDataTable dt = new StockDataSet.StockPriceHistoryDataTable();
                using (var db = StockDataService.GetServiceInstance(_dbType))
                {
                    IStockDailyInfoCollector collector = StockDailyInfoCollectorProvider.GetDailyPriceCollector(CollectorTypeName);
                    foreach (var d in db.GetStocks())
                    {
                        _logger.DebugFormat("Retrieving daily price of [{0}] stock.", d.StockNo);
                        StockDailyPriceInfo info = collector.GetStockDailyPriceInfo(d.StockNo);
                        if (null != info)
                        {
                            _logger.Debug(info);
                            if (info.Volume > 0)
                            {
                                StockDataSet.StockPriceHistoryRow dr = dt.NewStockPriceHistoryRow();
                                dr.StockID = d.StockID;
                                dr.StockDT = info.LastTradeDT.Date;
                                dr.OpenPrice = info.Open;
                                dr.HighPrice = info.Top;
                                dr.LowPrice = info.Lowest;
                                dr.ClosePrice = info.LastTrade;
                                dr.Volume = info.Volume;
                                dr.AdjClosePrice = info.LastTrade;
                                dr.DateCreated = DateTime.Now;

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