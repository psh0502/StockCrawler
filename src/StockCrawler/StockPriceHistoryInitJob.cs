using Common.Logging;
using Quartz;
using StockCrawler.Dao;
using System.Data;
using System.Linq;
using System.Reflection;

namespace StockCrawler.Services
{
    public class StockPriceHistoryInitJob : JobBase, IJob
    {
        internal static ILog Logger { get; set; } = LogManager.GetLogger(typeof(StockPriceHistoryInitJob));
        public StockPriceHistoryInitJob()
            : base()
        {
            if (null == Logger)
                Logger = LogManager.GetLogger(typeof(StockPriceHistoryInitJob));
        }
        internal string ProcessingStockNo { get; set; }

        #region IJob Members
        public void Execute(IJobExecutionContext context)
        {
            Logger.InfoFormat("Invoke [{0}]...", MethodBase.GetCurrentMethod().Name);
            // init stock list
            DownloadTwseLatestInfo();

            using (var db = StockDataServiceProvider.GetServiceInstance())
            {
                foreach (var d in db.GetStocks().Where(d => string.IsNullOrEmpty(ProcessingStockNo) || d.StockNo == ProcessingStockNo))
                {
                    db.DeleteStockPriceHistoryData(d.StockNo, null);
                    var list = CollectorProviderService.GetStockHistoryPriceCollector()
                        .GetStockHistoryPriceInfo(d.StockNo, SystemTime.Today.AddYears(-5), SystemTime.Today.AddDays(1));

                    if (list.Any())
                        Tools.CalculateMAAndPeriodK(list);

                    Logger.InfoFormat("Finish the {0} stock history task.", d.StockNo);
                }
            }
        }
        #endregion

        private void DownloadTwseLatestInfo()
        {
            var list = CollectorProviderService.GetStockDailyPriceCollector().GetStockDailyPriceInfo()
                .Select(d => new GetStocksResult()
                {
                    StockNo = d.StockNo,
                    StockName = d.StockName,
                    Enable = true
                }).ToList();

            if (list.Any())
                StockDataServiceProvider.GetServiceInstance().RenewStockList(list);
        }
    }
}