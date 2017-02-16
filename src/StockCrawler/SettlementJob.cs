using IRONMAN.DAO;
using Quartz;

namespace StockCrawler.Services
{
    public class SettlementJob : JobBase, IJob
    {
        #region IJob Members

        public void Execute(JobExecutionContext context)
        {
            using (var db = TransactionDataService.GetServiceInstance())
            {
                _logger.Debug("RoiSettlment job is working...");
                db.RoiSettlement();
                _logger.Info("RoiSettlment job has done.");
            }
        }

        #endregion
    }
}
