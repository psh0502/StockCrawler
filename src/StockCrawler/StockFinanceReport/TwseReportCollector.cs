using Common.Logging;
using HtmlAgilityPack;
using StockCrawler.Dao;
using System;
using System.Text;
using System.Web;

namespace StockCrawler.Services.StockFinanceReport
{
    internal class TwseReportCollector : IStockFinanceReportCashFlowCollector
    {
        internal static ILog _logger = LogManager.GetLogger(typeof(TwseReportCollector));
        public GetStockReportCashFlowResult GetStockFinanceReportCashFlow(string stockNo, short year, short season)
        {
            var url = "https://mops.twse.com.tw/mops/web/ajax_t164sb05";
            var formData = HttpUtility.ParseQueryString(string.Empty);
            formData.Add("step", "1");
            formData.Add("firstin", "1");
            formData.Add("off", "1");
            formData.Add("queryName", "co_id");
            formData.Add("inpuType", "co_id");
            formData.Add("TYPEK", "all");
            formData.Add("isnew", true.ToString());
            formData.Add("co_id", stockNo);
            formData.Add("year", year.ToString());
            formData.Add("season", season.ToString("00"));
            _logger.Debug("formData=" + formData.ToString());

            var html = Tools.DownloadStringData(new Uri(url), Encoding.UTF8, out _, "application/x-www-form-urlencoded", null, "POST", formData);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            var tableNode = doc.DocumentNode.SelectSingleNode("/html/body/center/table[2]");
            if (null == tableNode || string.IsNullOrEmpty(tableNode.InnerText.Trim()))
            {
                _logger.Warn($"[{stockNo}] can't get the body node, html={html}");
                return null;
            }
            else
            {
                _logger.Info($"[{stockNo}] get the body node successfully. text={tableNode.InnerText}");
                var result = TransformNodeToDataRow(tableNode);
                result.StockNo = stockNo;
                result.Year = year;
                result.Season = season;
                return result;
            }
        }

        private static GetStockReportCashFlowResult TransformNodeToDataRow(HtmlNode bodyNode)
        {
            var result = new GetStockReportCashFlowResult()
            {
                Depreciation = GetNodeTextToDecimal(bodyNode.SelectSingleNode("./tr[8]/td[2]")),
                AmortizationFee = GetNodeTextToDecimal(bodyNode.SelectSingleNode("./tr[9]/td[2]")),
                //營業現金流, 營業活動之淨現金流入（流出）
                BusinessCashflow = GetNodeTextToDecimal(bodyNode.SelectSingleNode("./tr[43]/td[2]")),
                InvestmentCashflow = GetNodeTextToDecimal(bodyNode.SelectSingleNode("./tr[58]/td[2]")),
                FinancingCashflow = GetNodeTextToDecimal(bodyNode.SelectSingleNode("./tr[72]/td[2]")),
                // 資本支出, (取得不動產、廠房及設備 + 處分不動產、廠房及設備)
                CapitalExpenditures = GetNodeTextToDecimal(bodyNode.SelectSingleNode("./tr[50]/td[2]")) + GetNodeTextToDecimal(bodyNode.SelectSingleNode("./tr[51]/td[2]")),
            };
            // 自由現金流 = (營業現金流 - 資本支出 - 股利支出)
            result.FreeCashflow = result.BusinessCashflow - result.CapitalExpenditures - result.InvestmentCashflow - GetNodeTextToDecimal(bodyNode.SelectSingleNode("./tr[69]/td[2]"));
            // 淨現金流 = 營業現金流 - 投資現金流 + 融資現金流
            result.NetCashflow = result.BusinessCashflow - result.InvestmentCashflow + result.FinancingCashflow;

            return result;
        }
        private static decimal GetNodeTextToDecimal(HtmlNode node)
        {
            return decimal.Parse(node.InnerText.Trim().Replace(",", string.Empty));
        }
    }
}
