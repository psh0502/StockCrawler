using Common.Logging;
using HtmlAgilityPack;
using StockCrawler.Dao;
using System;
using System.Web;

namespace StockCrawler.Services.Collectors
{
    internal class TwseStockBasicInfoCollector : TwseCollectorBase, IStockBasicInfoCollector
    {
        internal new static ILog _logger = LogManager.GetLogger(typeof(TwseStockBasicInfoCollector));

        public virtual GetStockBasicInfoResult GetStockBasicInfo(string stockNo)
        {
            var url = "https://mops.twse.com.tw/mops/web/ajax_t05st03";
            var node = GetTwseDataBack(url, stockNo, xpath: "/html/body/table[2]");
            var result = TransformNodeToBasicInfoRow(node);
            result.StockNo = stockNo;
            return result;
        }

        private GetStockBasicInfoResult TransformNodeToBasicInfoRow(HtmlNode tableNode)
        {
            try
            {
                var result = new GetStockBasicInfoResult()
                {
                    StockName = null,
                    Category = GetNodeTextTo<string>(SearchValueNode(tableNode, "產業類別", beginIndex: 1, xpath1: "./tr[{0}]/th[2]", xpath2: "./tr[{0}]/td[2]")),
                    CompanyName = GetNodeTextTo<string>(SearchValueNode(tableNode, "公司名稱", beginIndex: 1, xpath1: "./tr[{0}]/th[1]", xpath2: "./tr[{0}]/td[1]")),
                    Capital = ParseCapital((SearchValueNode(tableNode, "實收資本額", beginIndex: 1, xpath1: "./tr[{0}]/th[1]", xpath2: "./tr[{0}]/td[1]").InnerText)),
                    ReleaseStockCount = ParseStockCount((SearchValueNode(tableNode, "已發行普通股數或TDR原股發行股數", beginIndex: 1, xpath1: "./tr[{0}]/th[1]", xpath2: "./tr[{0}]/td[1]").InnerText)),
                    Chairman = GetNodeTextTo<string>(SearchValueNode(tableNode, "董事長", beginIndex: 1, xpath1: "./tr[{0}]/th[1]", xpath2: "./tr[{0}]/td[1]")),
                    CEO = GetNodeTextTo<string>(SearchValueNode(tableNode, "總經理", beginIndex: 1, xpath1: "./tr[{0}]/th[2]", xpath2: "./tr[{0}]/td[2]")),
                    CompanyID = GetNodeTextTo<string>(SearchValueNode(tableNode, "營利事業統一編號", beginIndex: 1, xpath1: "./tr[{0}]/th[2]", xpath2: "./tr[{0}]/td[2]")),
                    Url = null,
                    Business = GetNodeTextTo<string>(SearchValueNode(tableNode, "主要經營業務", beginIndex: 1, xpath1: "./tr[{0}]/th[1]", xpath2: "./tr[{0}]/td[1]"))
                };
                var texts = SearchValueNode(tableNode, "公司成立日期", beginIndex: 1, xpath1: "./tr[{0}]/th[1]", xpath2: "./tr[{0}]/td[1]").InnerText.Split('/');
                result.BuildDate = DateTime.Parse(string.Join("/", int.Parse(texts[0]) + 1911, texts[1], texts[2]));
                texts = SearchValueNode(tableNode, "上市日期", beginIndex: 1, xpath1: "./tr[{0}]/th[2]", xpath2: "./tr[{0}]/td[2]").InnerText.Split('/');
                result.PublishDate = DateTime.Parse(string.Join("/", int.Parse(texts[0]) + 1911, texts[1], texts[2]));

                return result;
            }
            catch (Exception ex)
            {
                _logger.Warn(ex.Message, ex);
                return null;
            }
        }

        private static long ParseStockCount(string innerText)
        {
            innerText = HttpUtility.HtmlDecode(innerText).Trim();
            var position = innerText.IndexOf("股");
            return long.Parse(innerText.Substring(0, position).Replace(",", string.Empty));
        }

        private static decimal ParseCapital(string innerText)
        {
            return decimal.Parse(innerText.Replace(",",string.Empty).Replace("元", string.Empty).Trim());
        }
    }
}
