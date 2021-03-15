using HtmlAgilityPack;
using StockCrawler.Dao;
using System;
using System.Collections.Generic;

namespace StockCrawler.Services.Collectors
{
    internal class TwseInterestIssuedCollector : TwseCollectorBase, IStockInterestIssuedCollector
    {
        public virtual IList<GetStockInterestIssuedInfoResult> GetStockInterestIssuedInfo(string stockNo)
        {
            var url = "https://mops.twse.com.tw/mops/web/ajax_t05st09_2";
            IList<GetStockInterestIssuedInfoResult> result = null;
            var tableNode = GetTwseDataBack(url, stockNo, xpath: "/html/body/div/table[4]");
            if (null != tableNode)
                result = TransformNodeToInterestIssuedResult(stockNo, tableNode);

            return result;
        }
        private IList<GetStockInterestIssuedInfoResult> TransformNodeToInterestIssuedResult(string stockNo, HtmlNode bodyNode)
        {
            var result = new List<GetStockInterestIssuedInfoResult>();
            var year_nodes = bodyNode.SelectNodes("tr[1]/td");
            for (int i = 1; i < year_nodes.Count; i++)
            {
                try
                {
                    var y = year_nodes[i].InnerText.Replace("年度", string.Empty);
                    short season = 0;
                    if (short.TryParse(y, out short year))
                        season = 4;
                    else
                        ParseYearSeasonNumber(y, ref season, ref year);

                    result.Add(new GetStockInterestIssuedInfoResult()
                    {
                        StockNo = stockNo,
                        Year = year,
                        Season = season,
                        // TODO: fill up other fields
                    });
                }
                catch(InvalidCastException ex)
                {
                    _logger.Warn(ex.Message);
                }
            }
            return result;
        }
        private void ParseYearSeasonNumber(string y, ref short season, ref short year)
        {
            var ss = y.Split('Q');
            if (ss.Length == 2)
            {
                if (!short.TryParse(ss[0], out year))
                    throw new InvalidCastException("year can't be parsed, y=" + y);
                if (!short.TryParse(ss[1], out season))
                    throw new InvalidCastException("season can't be parsed, y=" + y);
            }
            else
                throw new InvalidCastException(string.Format("期別無法解析, y={0}", y));
        }
    }
}
