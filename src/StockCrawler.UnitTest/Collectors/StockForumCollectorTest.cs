using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockCrawler.Services;
using StockCrawler.Services.Collectors;
using System.Linq;

#if (DEBUG)
namespace StockCrawler.UnitTest.Collectors
{
    [TestClass]
    public class StockForumCollectorTest : UnitTestBase
    {
        [TestInitialize]
        public override void InitBeforeTest()
        {
            base.InitBeforeTest();
            SqlTool.ExecuteSql("TRUNCATE TABLE StockBasicInfo");
            SqlTool.ExecuteSql("DELETE Stock");
            SqlTool.ExecuteSqlFile(@"..\..\..\StockCrawler.UnitTest\TestData\Sql\Stock.data.sql");
        }
        [TestMethod]
        public void CollectorTestMethod_GetData_20200105()
        {
            var collector = new StockForumCollector()
            {
                _logger = new UnitTestLogger()
            };
            SystemTime.Reset();
            var testDate = SystemTime.Today;
            var r = collector.GetPttData(testDate);
            foreach(var d in r)
            {
                _logger.InfoFormat("title: {0}, source: {1}, url: {2}", d.Article.Subject, d.Article.Source, d.Article.Url);
                if (d.relateToStockNo.Any())
                {
                    Assert.IsTrue(d.Article.Source == "mops" || d.Article.Source == "ptt");
                    foreach (var s in d.relateToStockNo)
                        _logger.InfoFormat("[{0}]{1}", s.StockNo, s.StockName);
                }
                else
                {
                    if (d.Article.Source == "ptt")
                        Assert.IsTrue(d.Article.Url.StartsWith("https://www.ptt.cc/bbs/Stock"));
                    else
                        Assert.AreEqual("twse", d.Article.Source);
                }
                Assert.AreEqual(testDate, d.Article.ArticleDate);
            }
            Assert.IsTrue(r.Any());
        }
    }
}
#endif