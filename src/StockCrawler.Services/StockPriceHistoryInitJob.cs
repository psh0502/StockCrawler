using Common.Logging;
using Quartz;
using StockCrawler.Dao;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace StockCrawler.Services
{
    public class StockPriceHistoryInitJob : JobBase, IJob
    {
        internal static ILog Logger { get; set; } = LogManager.GetLogger(typeof(StockPriceHistoryInitJob));
        public string ProcessingStockNo { get; set; }

        #region IJob Members
        public Task Execute(IJobExecutionContext context)
        {
            Logger.InfoFormat("Invoke [{0}]...", MethodBase.GetCurrentMethod().Name);
            // init stock list
            DownloadTwseLatestInfo();
#if (DEBUG)
            var bgnDate = SystemTime.Today.AddMonths(-2);
#else
            var bgnDate = SystemTime.Today.AddYears(-5);
#endif
            var endDate = SystemTime.Today;

            var collector = CollectorProviderService.GetStockHistoryPriceCollector();
            foreach (var d in StockHelper.GetAllStockList().Where(d => string.IsNullOrEmpty(ProcessingStockNo) || d.StockNo == ProcessingStockNo))
            {
                using (var db = GetDB())
                    db.DeleteStockPriceHistoryData(d.StockNo, null);

                var list = collector.GetStockHistoryPriceInfo(d.StockNo, bgnDate, endDate);

                if (list.Any())
                    using (var db = GetDB())
                        db.InsertOrUpdateStockPrice(list.ToArray());

                Logger.InfoFormat("Finish the {0} stock history task.", d.StockNo);
            }

            for (var date = bgnDate; date <= endDate; date = date.AddDays(1))
                Tools.CalculateMAAndPeriodK(date);

            return null;
        }
#endregion

        private void DownloadTwseLatestInfo()
        {
            var list = CollectorProviderService.GetStockDailyPriceCollector()
                .GetStockDailyPriceInfo()
                .Select(d => new GetStocksResult()
                {
                    StockNo = d.StockNo,
                    StockName = d.StockName,
                    Enable = true
                }).ToList();

            if (list.Any())
                using (var db = GetDB()) 
                    db.InsertOrUpdateStock(list.ToArray());
        }
    }
}