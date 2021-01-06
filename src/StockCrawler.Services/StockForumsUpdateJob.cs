using Common.Logging;
using Quartz;
using StockCrawler.Dao;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace StockCrawler.Services
{
    public class StockForumsUpdateJob : JobBase, IJob
    {
        internal static ILog Logger { get; set; } = LogManager.GetLogger(typeof(StockForumsUpdateJob));

        #region IJob Members
        public Task Execute(IJobExecutionContext context)
        {
            Logger.InfoFormat("Invoke [{0}]...", MethodBase.GetCurrentMethod().Name);
            try
            {
                var collector = CollectorServiceProvider.GetStockForumCollector();
                var data = collector.GetPttData(SystemTime.Today);
                {
                    var twse = (from d in data
                                where d.Article.Source == "twse"
                                select new GetStockMarketNewsResult()
                                {
                                    NewsDate = d.Article.ArticleDate,
                                    Source = d.Article.Source,
                                    StockNo = "0000",
                                    Subject = d.Article.Subject,
                                    Url = d.Article.Url
                                }).ToList();
                    var mops = (from d in data
                                where d.Article.Source == "mops"
                                select new GetStockMarketNewsResult()
                                {
                                    NewsDate = d.Article.ArticleDate,
                                    Source = d.Article.Source,
                                    StockNo = d.relateToStockNo[0].StockNo,
                                    Subject = d.Article.Subject,
                                    Url = d.Article.Url
                                }).ToList();
                    var ptt = (from d in data
                               where d.Article.Source == "ptt"
                               select d).ToList();
                    var doing = "twse";
                    using (var db = StockDataServiceProvider.GetServiceInstance())
                        try
                        {
                            db.InsertStockMarketNews(twse.ToArray());
                            doing = "mops";
                            db.InsertStockMarketNews(mops.ToArray());
                            doing = "ptt";
                            db.InsertStockForumData(ptt);
                        }
                        catch (Exception e)
                        {
                            Logger.Warn(string.Format("[{0}] Fail to save.", doing), e);
                        }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Job executing failed!", ex);
            }
            return null;
        }
        #endregion
    }
}