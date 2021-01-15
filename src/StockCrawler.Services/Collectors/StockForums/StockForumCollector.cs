using Common.Logging;
using HtmlAgilityPack;
using StockCrawler.Dao;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace StockCrawler.Services.Collectors
{
    internal class StockForumCollector : IStockForumCollector
    {
        // 由於 PTT 的文章順序不是依照最新日期排序, 所以可能要往下多翻幾頁找看看才能找到較多的文章
        private static readonly int MISSING_RIGHT_DATE_PAGE_MAX = int.Parse(ConfigurationManager.AppSettings["PTT_MISSING_RIGHT_DATE_PAGE_MAX"] ?? "2");
        internal ILog _logger = LogManager.GetLogger(typeof(StockForumCollector));
        public IList<(GetStockForumDataResult Article, IList<GetStocksResult> relateToStockNo)> GetPttData(DateTime date)
        {
            var article_date = DateTime.MinValue;
            IList<(GetStockForumDataResult, IList<GetStocksResult>)> result = new List<(GetStockForumDataResult, IList<GetStocksResult>)>();
            var uri = new Uri("https://www.ptt.cc/bbs/Stock/index.html");
            var doc = new HtmlDocument();
            var missing_right_date_page_counter = 0;
            do
            {
                var hasArticleInRightDate = false;
                var html = DownloadData(uri);
                doc.LoadHtml(html);
                var previos_url = doc.DocumentNode
                    .SelectSingleNode("//*[@id=\"action-bar-container\"]/div/div[2]/a[2]")
                    .Attributes["href"]?.Value;
                foreach (var node in doc.DocumentNode.SelectNodes("//div[@class=\"r-ent\"]"))
                {
                    var a = node.SelectSingleNode("div[@class=\"title\"]/a");
                    if (a != null)
                    {
                        var title = Tools.CleanString(a.InnerText);
                        var date_str = Tools.CleanString(node.SelectSingleNode("div[@class=\"meta\"]/div[@class=\"date\"]").InnerText);
                        article_date = DateTime.Parse(DateTime.Today.Year + "/" + date_str);
                        if (article_date == date)
                        {
                            hasArticleInRightDate = true;
                            if (!title.StartsWith("Re:")
                                && !title.StartsWith("[活動]")
                                && !title.StartsWith("[公告]"))
                            {
                                string url = a.Attributes["href"]?.Value;
                                if (string.IsNullOrEmpty(url) || !url.Contains("M."))
                                    continue;

                                url = string.Format("https://www.ptt.cc{0}", url);
                                var article = new GetStockForumDataResult()
                                {
                                    Subject = title,
                                    Hash = Tools.GenerateMD5Hash(string.Format("{0:yyyyMMdd}|{1}", article_date, title)),
                                    Url = url,
                                    Source = "ptt",
                                    ArticleDate = article_date
                                };
                                IList<GetStocksResult> related_stocks = AnalyzeArticle(ref article);
                                if (article.Source != "ptt" || (null != related_stocks && related_stocks.Any()))
                                {
                                    result.Add((article, related_stocks));
                                    _logger.DebugFormat("Subject: {0}, ArticleDate: {1}, Url: {2}", article.Subject, article.ArticleDate, article.Url);
                                }
                            }
                        }
                    }
                }
                //由於 PTT 的文章順序不是依照最新日期排序, 所以可能要往下多翻幾頁找看看才能找到較多的文章
                if (article_date > date || hasArticleInRightDate)
                    missing_right_date_page_counter = 0;
                else
                    missing_right_date_page_counter++;

                if (string.IsNullOrEmpty(previos_url))
                    break;
                else
                    uri = new Uri(uri.Scheme + "://" + uri.Authority + "/" + previos_url);

                _logger.DebugFormat("missing_right_date_page_counter: {0}", missing_right_date_page_counter);
#if(!DEBUG)
                Thread.Sleep(1000);
#endif
            } while (missing_right_date_page_counter < MISSING_RIGHT_DATE_PAGE_MAX);
            return result;
        }
        private IList<GetStocksResult> AnalyzeArticle(ref GetStockForumDataResult article)
        {
            var result = new List<GetStocksResult>();
            foreach (var s in StockHelper.GetCompanyStockList())
                if (article.Subject.Contains(s.StockNo)
                    || article.Subject.Contains(s.StockName))
                    result.Add(s);

            if (article.Subject.StartsWith("[新聞]"))
            {
                var html = DownloadData(new Uri(article.Url));
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                var node = doc.DocumentNode.SelectSingleNode("//*[@id=\"main-content\"]");
                if (null != node)
                {
                    const string keyword = "原文連結：";
                    var index = node.InnerHtml.IndexOf(keyword);
                    if (index > 0)
                        node.InnerHtml = node.InnerHtml.Substring(index + keyword.Length);
                    int i = 1;
                    do
                    {
                        var a_node = node.SelectSingleNode("a[" + i + "]");
                        if (null != a_node)
                        {
                            var url = a_node.Attributes["href"]?.Value;
                            if (!string.IsNullOrEmpty(url))
                            {
                                if (url == "https://bit.ly/")
                                {
                                    i++;
                                    continue;
                                }
                                article.Source = result.Any() ? "mops" : "twse";
                                var news = DownloadData(new Uri(url));
                                if (!string.IsNullOrEmpty(news))
                                {
                                    doc.LoadHtml(news);
                                    var title_node = doc.DocumentNode.SelectSingleNode("//title");
                                    if (null != title_node)
                                    {
                                        article.Subject = HttpUtility.UrlDecode(Tools.CleanString(title_node.InnerText));
                                        article.Url = url;
                                    }
                                }
                            }
                        }
                        break;
                    } while (true);
                }
            }
            return result;
        }
        /// <summary>
        /// 抽出下載資料部位, 以便單元測試覆寫
        /// </summary>
        /// <param name="uri">下載資料連結</param>
        /// <returns>Downloaded html string</returns>
        protected virtual string DownloadData(Uri uri)
        {
            var html = Tools.DownloadStringData(uri, out _);
#if (DEBUG)
            try
            {
                var file_name = uri.LocalPath.Replace("/", string.Empty);
                if (!string.IsNullOrEmpty(file_name))
                {
                    if (!file_name.EndsWith(".html")) file_name += ".html";
                    var file = new FileInfo(@"..\..\..\StockCrawler.UnitTest\TestData\Ptt\" + file_name);
                    if (file.Exists) file.Delete();
                    using (var sw = file.CreateText())
                        sw.Write(html);
                }
            }
            catch { }
#endif
            return html;
        }
    }
}
