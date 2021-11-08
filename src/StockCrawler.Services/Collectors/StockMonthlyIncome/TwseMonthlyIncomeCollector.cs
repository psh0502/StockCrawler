using HtmlAgilityPack;
using StockCrawler.Dao;
using System;
using System.Collections.Generic;

namespace StockCrawler.Services.Collectors
{
    internal class TwseMonthlyIncomeCollector : TwseCollectorBase, IStockMonthlyIncomeCollector
    {
        public virtual IList<GetStockMonthlyIncomeResult> GetStockMonthlyIncome(string stockNo)
        {
            var url = "https://mops.twse.com.tw/mops/web/ajax_t146sb05";
            IList<GetStockMonthlyIncomeResult> result = null;
            var tableNode = GetTwseDataBack(url, stockNo, step: 2, xpath: "/html/body/div/table[2]");
            if (null != tableNode)
                result = TransformNodeToMonthlyIncome(stockNo, tableNode);

            return result;
        }
        private IList<GetStockMonthlyIncomeResult> TransformNodeToMonthlyIncome(string stockNo, HtmlNode bodyNode)
        {
            var result = new List<GetStockMonthlyIncomeResult>();
            for (var i = 3; i < bodyNode.ChildNodes.Count; i++)
            {
                var year_node = bodyNode.SelectSingleNode($"tr[{i}]/td[1]");
                var month_node = bodyNode.SelectSingleNode($"tr[{i}]/td[2]");
                try
                {
                    if (null != year_node && short.TryParse(year_node.InnerText, out short year))
                    {
                        short.TryParse(month_node.InnerText, out short month);

                        result.Add(new GetStockMonthlyIncomeResult()
                        {
                            StockNo = stockNo,
                            Year = year,
                            Month = month,
                            Income = GetNodeTextTo<decimal>(bodyNode.SelectSingleNode($"tr[{i}]/td[3]")), // 當月營收
                            PreIncome = GetNodeTextTo<decimal>(bodyNode.SelectSingleNode($"tr[{i}]/td[4]")), // 去年當月營收
                            DeltaPercent = GetNodeTextTo<decimal>(bodyNode.SelectSingleNode($"tr[{i}]/td[5]")), // 去年同月增減(%)
                            CumMonthIncome = GetNodeTextTo<decimal>(bodyNode.SelectSingleNode($"tr[{i}]/td[6]")), // 當月累計營收
                            PreCumMonthIncome = GetNodeTextTo<decimal>(bodyNode.SelectSingleNode($"tr[{i}]/td[7]")), // 去年累計營收
                            DeltaCumMonthIncomePercent = GetNodeTextTo<decimal>(bodyNode.SelectSingleNode($"tr[{i}]/td[8]")), // 前期比較增減(%)
                        });
                    }
                }catch(InvalidCastException ex)
                {
                    _logger.Warn(ex.Message);
                }
            }
            return result;
        }
    }
}
