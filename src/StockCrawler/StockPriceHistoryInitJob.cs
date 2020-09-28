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
#if(DEBUG)
                    var bgnDate = SystemTime.Today.AddYears(-1);
#else
                    var bgnDate = SystemTime.Today.AddYears(-5);
#endif
                    var endDate = SystemTime.Today;

                    var list = CollectorProviderService.GetStockHistoryPriceCollector()
                        .GetStockHistoryPriceInfo(d.StockNo, bgnDate, endDate);

                    if (list.Any())
                    {
                        // 寫入日價
                        db.InsertOrUpdateStockPrice(list);
                        for (var date = bgnDate; date <= endDate; date = date.AddDays(1))
                            Tools.CalculateMAAndPeriodK(date);
                    }

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