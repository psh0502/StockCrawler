using HtmlAgilityPack;
using StockCrawler.Dao;

namespace StockCrawler.Services.Collectors
{
    internal class TwseReportCollector : TwseCollectorBase, IStockReportCollector
    {
        public virtual GetStockFinancialReportResult GetStockFinancialReport(
            string stockNo, short year, short season)
        {
            var url = "https://mops.twse.com.tw/mops/web/ajax_t146sb05";
            GetStockFinancialReportResult result = null;
            var tableNode = GetTwseDataBack(url, stockNo, year, season);
            if (null != tableNode)
            {
                result = TransformNodeToFinancial(tableNode);
                result.StockNo = stockNo;
                result.Year = year;
                result.Season = season;
            }
            return result;
        }
        private GetStockFinancialReportResult TransformNodeToFinancial(HtmlNode bodyNode)
        {
            var result = new GetStockFinancialReportResult()
            {
                TotalAssets = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "資產總計")),
                TotalLiability = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "負債總計")),
                NetWorth = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "權益總計")),
                NAV = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "每股淨值")),
                Revenue = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "營業收入")),
                BusinessInterest = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "營業利益")),
                NetProfitTaxFree = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "稅前淨利")),
                EPS = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "每股盈餘")),
                BusinessCashflow = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "營業活動之淨現金流入")),
                InvestmentCashflow = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "投資活動之淨現金流入")),
                FinancingCashflow = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "籌資活動之淨現金流入")),
            };

            return result;
        }
    }
}
