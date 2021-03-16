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
            var tableNode = GetTwseDataBack(
                url
                , stockNo
                , year: Tools.GetTaiwanYear()
                , xpath: "/html/body/center/table[3]");

            result = TransformNodeToInterestIssuedResult(stockNo, tableNode);
            return result;
        }
        private IList<GetStockInterestIssuedInfoResult> TransformNodeToInterestIssuedResult(
            string stockNo, HtmlNode bodyNode)
        {
            var result = new List<GetStockInterestIssuedInfoResult>();
            var index = 0;
            do
            {
                var node = bodyNode.SelectNodes("tr[" + (4 + index) + "]/td");
                if (null == node) break;

                try
                {
                    short season = 0, year = 0;
                    ParseYearSeasonNumber(node[1].InnerText, ref season, ref year);
                    result.Add(new GetStockInterestIssuedInfoResult()
                    {
                        StockNo = stockNo,
                        Year = year,
                        Season = season,
                        DecisionDate = ConvertToDecisionDate(Tools.CleanString(node[4].InnerText)),
                        CapitalReserveCashIssued = decimal.Parse(Tools.CleanString(node[12].InnerText.Replace(",", string.Empty))),
                        CapitalReserveStockIssued = decimal.Parse(Tools.CleanString(node[16].InnerText.Replace(",", string.Empty))),
                        ProfitCashIssued = decimal.Parse(Tools.CleanString(node[10].InnerText.Replace(",", string.Empty))),
                        ProfitStockIssued = decimal.Parse(Tools.CleanString(node[14].InnerText.Replace(",", string.Empty))),
                        SsrCashIssued = decimal.Parse(Tools.CleanString(node[11].InnerText.Replace(",", string.Empty))),
                        SsrStockIssued = decimal.Parse(Tools.CleanString(node[15].InnerText.Replace(",", string.Empty))),
                        LastModifiedAt = SystemTime.Now,
                        CreatedAt = SystemTime.Now,
                    });
                }
                catch (InvalidCastException ex)
                {
                    _logger.Warn(ex.Message);
                }
                index++;
            } while (true);

            return result;
        }

        private DateTime ConvertToDecisionDate(string v)
        {
            if (string.IsNullOrEmpty(v)) return new DateTime(1900, 1, 1);

            var ss = v.Split('/');
            if (ss.Length == 3)
                return new DateTime(
                    int.Parse(ss[0]) + 1911
                    , int.Parse(ss[1])
                    , int.Parse(ss[2]));
            else
                throw new InvalidCastException("ConvertToDecisionDate: Can't parse [" + v + "] to DateTime");
        }

        private void ParseYearSeasonNumber(string y, ref short season, ref short year)
        {
            var ss = Tools.CleanString(y)
                .Replace("年度", string.Empty)
                .Replace("年", ";")
                .Replace("第", string.Empty)
                .Replace("季", string.Empty)
                .Split(';');

            if (ss.Length >= 2)
            {
                if (!short.TryParse(Tools.CleanString(ss[0]), out year))
                    throw new InvalidCastException("year can't be parsed, y=" + y);
                if (!short.TryParse(Tools.CleanString(ss[1]), out season))
                    season = -1; // 全年度單次發放
            }
            else
                throw new InvalidCastException(string.Format("期別無法解析, y={0}", y));
        }
    }
}
