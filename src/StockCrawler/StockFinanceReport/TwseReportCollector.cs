using Common.Logging;
using HtmlAgilityPack;
using StockCrawler.Dao;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;

namespace StockCrawler.Services.StockFinanceReport
{
    internal class TwseReportCollector : IStockFinanceReportCashFlowCollector
    {
        private static string session_string = "jHttpSession@65edd889";
        private static readonly ILog _logger = LogManager.GetLogger(typeof(TwseReportCollector));
        public GetStockReportCashFlowResult GetStockFinanceReportCashFlow(string stockNo, short year, short season)
        {
            var url = "https://mops.twse.com.tw/mops/web/ajax_t164sb05";
            bool first = true;
            HtmlNode bodyNode;
            do
            {
                if (!first) Thread.Sleep(5 * 1000);
                List<Cookie> cookies = new List<Cookie>
                {
                    new Cookie("jcsession", session_string, "/mops/web", "mops.twse.com.tw"),
                    new Cookie("newmops2", HttpUtility.UrlEncode($"selfObj=tagCon1|co_id={stockNo}|year={year}|season=01|"), "/", "mops.twse.com.tw"),
                    new Cookie("_ga", "GA1.3.1575499580.1596435762", "/", ".twse.com.tw"),
                    new Cookie("_gid", "GA1.3.62473710.1596869685", "/", ".twse.com.tw")
                };
                cookies[0].HttpOnly = true;
                Dictionary<string, string> formData = new Dictionary<string, string>
            {
                { "encodeURIComponent", "1" },
                { "step", "1" },
                { "firstin", "1" },
                { "off", "1" },
                { "queryName", "co_id" },
                { "inpuType", "co_id" },
                { "TYPEK", "all" },
                { "isnew", "true" },
                { "co_id", stockNo },
                { "year", year.ToString() },
                { "season", season.ToString("00") }
            };
                var html = Tools.DownloadStringData(url, Encoding.UTF8, out Cookie[] returnCookies, cookies.ToArray(), "POST", formData, "https://mops.twse.com.tw/mops/web/t164sb05");
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                bodyNode = doc.DocumentNode.SelectSingleNode("/html/body");
                if (returnCookies.Length > 0) session_string = returnCookies[0].Value;
                first = false;
            } while (string.IsNullOrEmpty(bodyNode.InnerText.Trim()));

            return new List<GetStockReportCashFlowResult>() { new GetStockReportCashFlowResult() };
        }
    }
}
