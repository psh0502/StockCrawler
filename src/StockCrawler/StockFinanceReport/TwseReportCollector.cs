using Common.Logging;
using HtmlAgilityPack;
using StockCrawler.Dao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace StockCrawler.Services.StockFinanceReport
{
    internal class TwseReportCollector : IStockReportCollector
    {
        internal static ILog _logger = LogManager.GetLogger(typeof(TwseReportCollector));
        private const string _xpath_01 = "/html/body/center/table[2]";
        private const string _xpath_02 = "/html/body/table[4]";
        private static readonly string UTF8SpacingChar = Encoding.UTF8.GetString(new byte[] { 0xC2, 0xA0 });

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
                formData.Add("isnew", false.ToString());
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
                    var result = TransformNodeToCashflowRow(tableNode);
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
        private static GetStockReportCashFlowResult TransformNodeToCashflowRow(HtmlNode bodyNode)
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
        private static GetStockReportIncomeResult TransformNodeToIncomeRow(HtmlNode bodyNode)
        {
            return new GetStockReportIncomeResult()
            {
                Revenue = GetNodeTextToDecimal(SearchValueNode(bodyNode, "營業收入合計")),
                GrossProfit = GetNodeTextToDecimal(SearchValueNode(bodyNode, "營業毛利")),
                SalesExpense = GetNodeTextToDecimal(SearchValueNode(bodyNode, "推銷費用")),
                ManagementCost = GetNodeTextToDecimal(SearchValueNode(bodyNode, "管理費用")),
                RDExpense = GetNodeTextToDecimal(SearchValueNode(bodyNode, "研究發展費用")),
                OperatingExpenses = GetNodeTextToDecimal(SearchValueNode(bodyNode, "營業費用合計")),
                BusinessInterest = GetNodeTextToDecimal(SearchValueNode(bodyNode, "營業利益")),
                NetProfitTaxFree = GetNodeTextToDecimal(SearchValueNode(bodyNode, "稅前淨利")),
                NetProfitTaxed = GetNodeTextToDecimal(SearchValueNode(bodyNode, "本期淨利")),
            };
        }
        private static HtmlNode SearchValueNode(HtmlNode bodyNode, string keyword, int beginIndex = 5, string xpath1 = "./tr[{0}]/td[1]", string xpath2 = "./tr[{0}]/td[2]")
        {
            if (null == bodyNode) throw new ArgumentException("The parameter can't be null", "bodyNode");
            int index = beginIndex;
            do
            {
                var item = bodyNode.SelectSingleNode(string.Format(xpath1, index));
                if (null == item)
                {
                    _logger.WarnFormat("Can't find the keyword[{0}] in this html:\r\n{1}", keyword, bodyNode.InnerHtml);
                    return null;
                }
                if (item.InnerText.Contains(keyword))
                    return bodyNode.SelectSingleNode(string.Format(xpath2, index));
                index++;
            } while (true);
        }
        private static decimal GetNodeTextToDecimal(HtmlNode node)
        {
            if (null == node) return 0m;
            var innerText = HttpUtility.HtmlDecode(node.InnerText.Trim().Replace(",", string.Empty));
            return decimal.Parse(innerText.Replace(UTF8SpacingChar, string.Empty));
        }
        public GetStockReportIncomeResult GetStockReportIncome(string stockNo, short year, short season)
        {
            var url = "https://mops.twse.com.tw/mops/web/ajax_t164sb04";
            var tableNode = GetTwseDataBack(url, stockNo, year, season);
            var result = TransformNodeToIncomeRow(tableNode);
            result.StockNo = stockNo;
            result.Year = year;
            result.Season = season;
            return result;
        }

        private static HtmlNode GetTwseDataBack(string url, string stockNo, short year, short season = -1, short month = -1, string xpath = _xpath_01)
        {
            var formData = HttpUtility.ParseQueryString(string.Empty);
            formData.Add("step", "1");
            formData.Add("firstin", "1");
            formData.Add("off", "1");
            formData.Add("queryName", "co_id");
            formData.Add("inpuType", "co_id");
            formData.Add("TYPEK", "all");
            formData.Add("isnew", false.ToString());
            formData.Add("co_id", stockNo);
            formData.Add("year", year.ToString());
            if (season != -1) formData.Add("season", season.ToString("00"));
            if (month != -1) formData.Add("month", month.ToString("00"));
            _logger.Debug("formData=" + formData.ToString());

            var html = Tools.DownloadStringData(new Uri(url), Encoding.UTF8, out _, "application/x-www-form-urlencoded", null, "POST", formData);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            var tableNode = doc.DocumentNode.SelectSingleNode(xpath);
            if (null == tableNode || string.IsNullOrEmpty(tableNode.InnerText.Trim()))
            {
                _logger.Warn($"[{stockNo}] can't get the body node, html={html}");
                return null;
            }
            else
            {
                _logger.Info($"[{stockNo}] get the body node successfully.");
                return tableNode;
            }
        }

        public GetStockReportBalanceResult GetStockReportBalance(string stockNo, short year, short season)
        {
            var url = "https://mops.twse.com.tw/mops/web/ajax_t164sb03";
            var tableNode = GetTwseDataBack(url, stockNo, year, season);
            var result = TransformNodeToBalanceRow(tableNode);
            result.StockNo = stockNo;
            result.Year = year;
            result.Season = season;
            return result;
        }

        private static GetStockReportBalanceResult TransformNodeToBalanceRow(HtmlNode bodyNode)
        {
            return new GetStockReportBalanceResult()
            {
                // asset
                CashAndEquivalents = GetNodeTextToDecimal(SearchValueNode(bodyNode, "現金及約當現金")),
                ShortInvestments = GetNodeTextToDecimal(SearchValueNode(bodyNode, "金融資產－流動")),
                BillsReceivable = GetNodeTextToDecimal(SearchValueNode(bodyNode, "應收帳款淨額"))
                    + GetNodeTextToDecimal(SearchValueNode(bodyNode, "應收帳款－關係人")),
                Stock = GetNodeTextToDecimal(SearchValueNode(bodyNode, "存貨")),
                OtherCurrentAssets = GetNodeTextToDecimal(SearchValueNode(bodyNode, "其他流動資產")),
                CurrentAssets = GetNodeTextToDecimal(SearchValueNode(bodyNode, "流動資產合計")),
                LongInvestment = GetNodeTextToDecimal(SearchValueNode(bodyNode, "採用權益法之投資")),
                FixedAssets = GetNodeTextToDecimal(SearchValueNode(bodyNode, "不動產、廠房及設備")),
                OtherAssets = GetNodeTextToDecimal(SearchValueNode(bodyNode, "透過其他綜合損益按公允價值衡量之金融資產－非流動"))
                    + GetNodeTextToDecimal(SearchValueNode(bodyNode, "按攤銷後成本衡量之金融資產－非流動"))
                    + GetNodeTextToDecimal(SearchValueNode(bodyNode, "使用權資產"))
                    + GetNodeTextToDecimal(SearchValueNode(bodyNode, "無形資產"))
                    + GetNodeTextToDecimal(SearchValueNode(bodyNode, "遞延所得稅資產"))
                    + GetNodeTextToDecimal(SearchValueNode(bodyNode, "其他非流動資產")),
                TotalAssets = GetNodeTextToDecimal(SearchValueNode(bodyNode, "資產總額")),
                // Liabilities
                ShortLoan = GetNodeTextToDecimal(SearchValueNode(bodyNode, "短期借款")),
                ShortBillsPayable = GetNodeTextToDecimal(SearchValueNode(bodyNode, "應付短期票券")),
                AccountsAndBillsPayable = GetNodeTextToDecimal(SearchValueNode(bodyNode, "應付帳款"))
                    + GetNodeTextToDecimal(SearchValueNode(bodyNode, "應付帳款－關係人")),
                AdvenceReceipt = GetNodeTextToDecimal(SearchValueNode(bodyNode, "預收款項")),
                LongLiabilitiesWithinOneYear = GetNodeTextToDecimal(SearchValueNode(bodyNode, "一年內到期長期負債")),
                OtherCurrentLiabilities = GetNodeTextToDecimal(SearchValueNode(bodyNode, "其他應付款"))
                    + GetNodeTextToDecimal(SearchValueNode(bodyNode, "本期所得稅負債"))
                    + GetNodeTextToDecimal(SearchValueNode(bodyNode, "其他流動負債")),
                CurrentLiabilities = GetNodeTextToDecimal(SearchValueNode(bodyNode, "流動負債合計")),
                LongLiabilities = GetNodeTextToDecimal(SearchValueNode(bodyNode, "應付公司債")),
                OtherLiabilities = GetNodeTextToDecimal(SearchValueNode(bodyNode, "遞延所得稅負債"))
                    + GetNodeTextToDecimal(SearchValueNode(bodyNode, "租賃負債－非流動"))
                    + +GetNodeTextToDecimal(SearchValueNode(bodyNode, "其他非流動負債")),
                TotalLiability = GetNodeTextToDecimal(SearchValueNode(bodyNode, "負債總額")),
                NetWorth = GetNodeTextToDecimal(SearchValueNode(bodyNode, "權益總額")),
            };
        }

        public GetStockReportMonthlyNetProfitTaxedResult GetStockReportMonthlyNetProfitTaxed(string stockNo, short year, short month)
        {
            var url = "https://mops.twse.com.tw/mops/web/ajax_t05st10_ifrs";
            var tableNode = GetTwseDataBack(url, stockNo, year, month: month, xpath: _xpath_02);
            var result = TransformNodeToMonlyNetProfitTaxedRow(tableNode);
            result.StockNo = stockNo;
            result.Year = year;
            result.Month = month;
            return result;
        }

        private GetStockReportMonthlyNetProfitTaxedResult TransformNodeToMonlyNetProfitTaxedRow(HtmlNode bodyNode)
        {
            return new GetStockReportMonthlyNetProfitTaxedResult()
            {
                NetProfitTaxed = GetNodeTextToDecimal(SearchValueNode(bodyNode, "本月", beginIndex:1, xpath1: "./tr[{0}]/th[1]", xpath2: "./tr[{0}]/td[1]")),
                LastYearNetProfitTaxed = GetNodeTextToDecimal(SearchValueNode(bodyNode, "去年同期", beginIndex: 1, xpath1: "./tr[{0}]/th[1]", xpath2: "./tr[{0}]/td[1]")),
                Delta = GetNodeTextToDecimal(SearchValueNode(bodyNode, "增減金額", beginIndex: 1, xpath1: "./tr[{0}]/th[1]", xpath2: "./tr[{0}]/td[1]")),
                DeltaPercent = GetNodeTextToDecimal(SearchValueNode(bodyNode, "增減百分比", beginIndex: 1, xpath1: "./tr[{0}]/th[1]", xpath2: "./tr[{0}]/td[1]")) / 100,
                ThisYearTillThisMonth = GetNodeTextToDecimal(SearchValueNode(bodyNode, "本年累計", beginIndex: 1, xpath1: "./tr[{0}]/th[1]", xpath2: "./tr[{0}]/td[1]")),
                LastYearTillThisMonth = GetNodeTextToDecimal(SearchValueNode(bodyNode, "去年累計", beginIndex: 1, xpath1: "./tr[{0}]/th[1]", xpath2: "./tr[{0}]/td[1]")),
                TillThisMonthDelta = GetNodeTextToDecimal(SearchValueNode(bodyNode, "增減金額", beginIndex: 8, xpath1: "./tr[{0}]/th[1]", xpath2: "./tr[{0}]/td[1]")),
                TillThisMonthDeltaPercent = GetNodeTextToDecimal(SearchValueNode(bodyNode, "增減百分比", beginIndex: 9, xpath1: "./tr[{0}]/th[1]", xpath2: "./tr[{0}]/td[1]")) / 100,
                Remark = SearchValueNode(bodyNode, "備註/營收變化原因說明", beginIndex: 1, xpath1: "./th[1]", xpath2: "./td[1]").InnerText.Trim().Replace(" ", string.Empty),
            };
        }
    }
}
