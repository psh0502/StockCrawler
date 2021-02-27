using HtmlAgilityPack;
using StockCrawler.Dao;
using System;
using System.Collections.Generic;

namespace StockCrawler.Services.Collectors
{
    internal class TwseReportCollector : TwseCollectorBase, IStockReportCollector
    {
        public virtual IList<GetStockFinancialReportResult> GetStockFinancialReport(string stockNo)
        {
            var url = "https://mops.twse.com.tw/mops/web/ajax_t146sb05";
            IList<GetStockFinancialReportResult> result = null;
            var tableNode = GetTwseDataBack(url, stockNo, xpath: "/html/body/div/table[4]");
            if (null != tableNode)
                result = TransformNodeToFinancial(stockNo, tableNode);

            return result;
        }
        private IList<GetStockFinancialReportResult> TransformNodeToFinancial(string stockNo, HtmlNode bodyNode)
        {
            var result = new List<GetStockFinancialReportResult>();
            var year_nodes = bodyNode.SelectNodes("tr[1]/td");
            for (int i = 1; i < year_nodes.Count; i++)
            {
                var y = year_nodes[i].InnerText.Replace("年度", string.Empty);
                short season = 0;
                if(short.TryParse(y, out short year))
                    season = 4;
                else
                    ParseYearSeasonNumber(y, ref season, ref year);

                result.Add(new GetStockFinancialReportResult()
                {
                    StockNo = stockNo,
                    Year = year,
                    Season = season,
                    TotalAssets = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "資產總計", xpath1: "./tr[{0}]/td[2]", xpath2: "./tr[{0}]/td[" + (2 + i) + "]")),
                    TotalLiability = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "負債總計", xpath2: "./tr[{0}]/td[" + (1 + i) + "]")),
                    NetWorth = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "權益總計", xpath2: "./tr[{0}]/td[" + (1 + i) + "]")),
                    NAV = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "每股淨值", xpath2: "./tr[{0}]/td[" + (1 + i) + "]")),
                    Revenue = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "營業收入", xpath1: "./tr[{0}]/td[2]", xpath2: "./tr[{0}]/td[" + (2 + i) + "]")),
                    BusinessInterest = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "營業利益", xpath2: "./tr[{0}]/td[" + (1 + i) + "]")),
                    NetProfitTaxFree = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "稅前淨利", xpath2: "./tr[{0}]/td[" + (1 + i) + "]")),
                    EPS = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "每股盈餘", xpath2: "./tr[{0}]/td[" + (1 + i) + "]")),
                    BusinessCashflow = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "營業活動之淨現金流入", xpath1: "./tr[{0}]/td[2]", xpath2: "./tr[{0}]/td[" + (2 + i) + "]")),
                    InvestmentCashflow = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "投資活動之淨現金流入", xpath2: "./tr[{0}]/td[" + (1 + i) + "]")),
                    FinancingCashflow = GetNodeTextTo<decimal>(SearchValueNode(bodyNode, "籌資活動之淨現金流入", xpath2: "./tr[{0}]/td[" + (1 + i) + "]")),
                });
            }
            return result;
        }
        private void ParseYearSeasonNumber(string y, ref short season, ref short year)
        {
            var ss = y.Split(' ');
            if (ss.Length == 2)
            {
                if (short.TryParse(ss[0], out year))
                    throw new InvalidCastException("year can't be parsed.");
                if (short.TryParse(ss[1], out season))
                    throw new InvalidCastException("season can't be parsed.");
            }
            else
                _logger.WarnFormat("期別無法解析, y={0}", y);
        }
    }
}
