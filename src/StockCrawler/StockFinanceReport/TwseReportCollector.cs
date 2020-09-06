using Common.Logging;
using HtmlAgilityPack;
using StockCrawler.Dao;

namespace StockCrawler.Services.StockFinanceReport
{
    internal class TwseReportCollector : TwseCollectorBase, IStockReportCollector
    {
        internal new static ILog _logger = LogManager.GetLogger(typeof(TwseReportCollector));

        public virtual GetStockReportCashFlowResult GetStockReportCashFlow(string stockNo, short year, short season)
        {
            var url = "https://mops.twse.com.tw/mops/web/ajax_t164sb05";
            GetStockReportCashFlowResult result = null;
            var tableNode = GetTwseDataBack(url, stockNo, year, season);
            if (null != tableNode)
            {
                result = TransformNodeToCashflowRow(tableNode);
                result.StockNo = stockNo;
                result.Year = year;
                result.Season = season;
            }
            return result;
        }
        private static GetStockReportCashFlowResult TransformNodeToCashflowRow(HtmlNode bodyNode)
        {
            var result = new GetStockReportCashFlowResult()
            {
                Depreciation = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "折舊")),
                AmortizationFee = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "攤銷")),
                BusinessCashflow = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "營業活動之淨現金流入")),
                InvestmentCashflow = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "投資活動之淨現金流入")),
                FinancingCashflow = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "籌資活動之淨現金流入")),
                CapitalExpenditures = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "取得不動產")) + GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "處分不動產")),
            };
            result.FreeCashflow = result.BusinessCashflow + result.InvestmentCashflow;
            result.NetCashflow = result.BusinessCashflow + result.InvestmentCashflow + result.FinancingCashflow;

            return result;
        }
        private static GetStockReportIncomeResult TransformNodeToIncomeRow(HtmlNode bodyNode)
        {
            return new GetStockReportIncomeResult()
            {
                Revenue = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "營業收入合計")),
                GrossProfit = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "營業毛利")),
                SalesExpense = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "推銷費用")),
                ManagementCost = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "管理費用")),
                RDExpense = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "研究發展費用")),
                OperatingExpenses = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "營業費用合計")),
                BusinessInterest = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "營業利益")),
                NetProfitTaxFree = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "稅前淨利")),
                NetProfitTaxed = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "本期淨利")),
            };
        }
        public virtual GetStockReportIncomeResult GetStockReportIncome(string stockNo, short year, short season)
        {
            var url = "https://mops.twse.com.tw/mops/web/ajax_t164sb04";
            var tableNode = GetTwseDataBack(url, stockNo, year, season);
            var result = TransformNodeToIncomeRow(tableNode);
            result.StockNo = stockNo;
            result.Year = year;
            result.Season = season;
            return result;
        }

        public virtual GetStockReportBalanceResult GetStockReportBalance(string stockNo, short year, short season)
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
                CashAndEquivalents = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "現金及約當現金")),
                ShortInvestments = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "金融資產－流動")),
                BillsReceivable = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "應收帳款淨額"))
                    + GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "應收帳款－關係人")),
                Stock = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "存貨")),
                OtherCurrentAssets = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "其他流動資產")),
                CurrentAssets = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "流動資產合計")),
                LongInvestment = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "採用權益法之投資")),
                FixedAssets = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "不動產、廠房及設備")),
                OtherAssets = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "透過其他綜合損益按公允價值衡量之金融資產－非流動"))
                    + GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "按攤銷後成本衡量之金融資產－非流動"))
                    + GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "使用權資產"))
                    + GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "無形資產"))
                    + GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "遞延所得稅資產"))
                    + GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "其他非流動資產")),
                TotalAssets = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "資產總額")),
                // Liabilities
                ShortLoan = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "短期借款")),
                ShortBillsPayable = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "應付短期票券")),
                AccountsAndBillsPayable = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "應付帳款"))
                    + GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "應付帳款－關係人")),
                AdvenceReceipt = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "預收款項")),
                LongLiabilitiesWithinOneYear = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "一年內到期長期負債")),
                OtherCurrentLiabilities = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "其他應付款"))
                    + GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "本期所得稅負債"))
                    + GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "其他流動負債")),
                CurrentLiabilities = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "流動負債合計")),
                LongLiabilities = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "應付公司債")),
                OtherLiabilities = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "遞延所得稅負債"))
                    + GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "租賃負債－非流動"))
                    + +GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "其他非流動負債")),
                TotalLiability = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "負債總額")),
                NetWorth = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "權益總額")),
            };
        }

        public virtual GetStockReportMonthlyNetProfitTaxedResult GetStockReportMonthlyNetProfitTaxed(string stockNo, short year, short month)
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
                NetProfitTaxed = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "本月", beginIndex:1, xpath1: "./tr[{0}]/th[1]", xpath2: "./tr[{0}]/td[1]")),
                LastYearNetProfitTaxed = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "去年同期", beginIndex: 1, xpath1: "./tr[{0}]/th[1]", xpath2: "./tr[{0}]/td[1]")),
                Delta = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "增減金額", beginIndex: 1, xpath1: "./tr[{0}]/th[1]", xpath2: "./tr[{0}]/td[1]")),
                DeltaPercent = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "增減百分比", beginIndex: 1, xpath1: "./tr[{0}]/th[1]", xpath2: "./tr[{0}]/td[1]")) / 100,
                ThisYearTillThisMonth = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "本年累計", beginIndex: 1, xpath1: "./tr[{0}]/th[1]", xpath2: "./tr[{0}]/td[1]")),
                LastYearTillThisMonth = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "去年累計", beginIndex: 1, xpath1: "./tr[{0}]/th[1]", xpath2: "./tr[{0}]/td[1]")),
                TillThisMonthDelta = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "增減金額", beginIndex: 8, xpath1: "./tr[{0}]/th[1]", xpath2: "./tr[{0}]/td[1]")),
                TillThisMonthDeltaPercent = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "增減百分比", beginIndex: 9, xpath1: "./tr[{0}]/th[1]", xpath2: "./tr[{0}]/td[1]")) / 100,
                Remark = SearchValueNode(bodyNode, "備註/營收變化原因說明", beginIndex: 1, xpath1: "./th[1]", xpath2: "./td[1]").InnerText.Trim().Replace(" ", string.Empty),
            };
        }
    }
}
