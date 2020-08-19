using Common.Logging;
using HtmlAgilityPack;
using StockCrawler.Dao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace StockCrawler.Services.StockFinanceReport
{
    internal class TwseReportCollector : IStockReportCashFlowCollector
    {
        internal static ILog _logger = LogManager.GetLogger(typeof(TwseReportCollector));
        public GetStockReportCashFlowResult GetStockReportCashFlow(string stockNo, short year, short season)
        {
            var url = "https://mops.twse.com.tw/mops/web/ajax_t164sb05";
            List<GetStockReportCashFlowResult> results = new List<GetStockReportCashFlowResult>();
            //// 因為 TWSE 的每季報表都是根據當年度累加上來的數字, 所以只有多抓前一期的報表數字差額才能顯示當季真正財務
            for (short i = (short)Math.Max(1, season - 1); i <= season; i++)
            {
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
                formData.Add("season", i.ToString("00"));
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
                    _logger.Info($"[{stockNo}] get the body node successfully.");
                    var result = TransformNodeToDataRow(tableNode);
                    result.StockNo = stockNo;
                    result.Year = year;
                    result.Season = season;
                    results.Add(result);
                }
            }
            //// 因為 TWSE 的每季報表都是根據當年度累加上來的數字, 所以只有減去前一期的報表數字差額才能顯示當季真正財務
            if (results.Count > 1)
            {
                var seasonRecord = results[1];
                var lastSeaonRecord = results[0];
                seasonRecord.Depreciation -= lastSeaonRecord.Depreciation;
                seasonRecord.AmortizationFee -= lastSeaonRecord.AmortizationFee;
                seasonRecord.BusinessCashflow -= lastSeaonRecord.BusinessCashflow;
                seasonRecord.InvestmentCashflow -= lastSeaonRecord.InvestmentCashflow;
                seasonRecord.FinancingCashflow -= lastSeaonRecord.FinancingCashflow;
                seasonRecord.CapitalExpenditures -= lastSeaonRecord.CapitalExpenditures;
                seasonRecord.FreeCashflow -= lastSeaonRecord.FreeCashflow;
                seasonRecord.NetCashflow -= lastSeaonRecord.NetCashflow;
                return seasonRecord;
            }
            return results[0];
        }

        private static GetStockReportCashFlowResult TransformNodeToDataRow(HtmlNode bodyNode)
        {
            var result = new GetStockReportCashFlowResult()
            {
                Depreciation = GetNodeTextToDecimal(SearchValueNode(bodyNode, "折舊")),
                AmortizationFee = GetNodeTextToDecimal(SearchValueNode(bodyNode, "攤銷")),
                BusinessCashflow = GetNodeTextToDecimal(SearchValueNode(bodyNode, "營業活動之淨現金流入")),
                InvestmentCashflow = GetNodeTextToDecimal(SearchValueNode(bodyNode, "投資活動之淨現金流入")),
                FinancingCashflow = GetNodeTextToDecimal(SearchValueNode(bodyNode, "籌資活動之淨現金流入")),
                CapitalExpenditures = GetNodeTextToDecimal(SearchValueNode(bodyNode, "取得不動產")) + GetNodeTextToDecimal(SearchValueNode(bodyNode, "處分不動產")),
            };
            result.FreeCashflow = result.BusinessCashflow + result.InvestmentCashflow;
            result.NetCashflow = result.BusinessCashflow + result.InvestmentCashflow + result.FinancingCashflow;

            return result;
        }
        private static HtmlNode SearchValueNode(HtmlNode bodyNode, string keyword)
        {
            int index = 5;
            do
            {
                var item = bodyNode.SelectSingleNode($"./tr[{index}]/td[1]");
                if (item.InnerText.Contains(keyword))
                    return bodyNode.SelectSingleNode($"./tr[{index}]/td[2]");
                if (null == item) return null;
                index++;
            } while (true);
        }
        private static decimal GetNodeTextToDecimal(HtmlNode node)
        {
            return decimal.Parse(node.InnerText.Trim().Replace(",", string.Empty));
        }

        public GetStockReportIncomeResult GetStockReportIncome(string stockNo, short year, short season)
        {
            throw new NotImplementedException();
        }
    }
}
