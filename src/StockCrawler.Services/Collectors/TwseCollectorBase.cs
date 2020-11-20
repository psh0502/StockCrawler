using Common.Logging;
using HtmlAgilityPack;
using System;
using System.Configuration;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;

namespace StockCrawler.Services
{
    internal abstract class TwseCollectorBase
    {
        internal ILog _logger = LogManager.GetLogger(typeof(TwseCollectorBase));
        protected static readonly string UTF8SpacingChar = Encoding.UTF8.GetString(new byte[] { 0xC2, 0xA0 });
        protected const string _xpath_01 = "/html/body/center/table[2]";
        protected const string _xpath_02 = "/html/body/table[4]";
        internal static int _breakInternval = int.Parse(ConfigurationManager.AppSettings["CollectorBreakInternval"] ?? "0");
        public TwseCollectorBase()
        {
            _logger = LogManager.GetLogger(GetType());
        }
        protected static void GerneralizeNumberFieldData(string[] data)
        {
            // Generalize number fields data
            for (int i = 0; i < data.Length; i++)
                if (!string.IsNullOrEmpty(data[i]))
                    data[i] = data[i]
                        .Replace("--", "0")
                        .Replace(",", string.Empty)
                        .Replace("X", string.Empty)
                        .Trim();
        }

        protected HtmlNode SearchValueNode(HtmlNode bodyNode, string keyword, int beginIndex = 5, string xpath1 = "./tr[{0}]/td[1]", string xpath2 = "./tr[{0}]/td[2]")
        {
            if (null == bodyNode) throw new ArgumentException("The parameter can't be null", "bodyNode");
            int index = beginIndex;
            do
            {
                var item = bodyNode.SelectSingleNode(string.Format(xpath1, index));
                if (null != item)
                {
                    if (item.InnerText.Contains(keyword))
                        return bodyNode.SelectSingleNode(string.Format(xpath2, index));
                }
                index++;
            } while (index < bodyNode.ChildNodes.Count);
            _logger.WarnFormat("Can't find the keyword[{0}] in this html", keyword);
            return null;
        }
        protected static T GetNodeTextTo<T>(HtmlNode node)
        {
            if (null == node) return default;
            var innerText = HttpUtility.HtmlDecode(node.InnerText.Trim().Replace(",", string.Empty));
            innerText = innerText.Replace(UTF8SpacingChar, string.Empty);
            if (typeof(T) == typeof(int))
                return (T)((object)int.Parse(innerText));
            else if (typeof(T) == typeof(decimal))
                return (T)((object)decimal.Parse(innerText));
            else if (typeof(T) == typeof(double))
                return (T)((object)double.Parse(innerText));
            else if (typeof(T) == typeof(float))
                return (T)((object)float.Parse(innerText));
            else if (typeof(T) == typeof(string))
                return (T)((object)innerText.Trim());
            else if (typeof(T) == typeof(bool))
                return (T)((object)bool.Parse(innerText));
            else
                throw new InvalidCastException(typeof(T).Name + " is not defined in GetNodeTextTo.");
        }
        /// <summary>
        /// 取得 TWSE HTML 的資料節點
        /// </summary>
        /// <param name="url">網址</param>
        /// <param name="stockNo">股票代碼</param>
        /// <param name="year">中華民國年度</param>
        /// <param name="season">季度</param>
        /// <param name="month">月份</param>
        /// <param name="xpath">搜尋資料的 xpath 提示</param>
        /// <returns>含有資料的 html 節點</returns>
        /// <exception cref="WebException">網站讀取過於頻繁, 需要稍等後再讀取</exception>
        /// <exception cref="ApplicationException">該公司股票不繼續公開發行</exception>
        protected HtmlNode GetTwseDataBack(string url, string stockNo, short year = -1, short season = -1, short month = -1, string xpath = _xpath_01)
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
            if (year != -1) formData.Add("year", year.ToString());
            if (season != -1) formData.Add("season", season.ToString("00"));
            if (month != -1) formData.Add("month", month.ToString("00"));
            _logger.Debug("formData=" + formData.ToString());
            string html;
            while (true)
                try
                {
                    html = Tools.DownloadStringData(new Uri(url), Encoding.UTF8, out _, "application/x-www-form-urlencoded", null, "POST", formData);
                    if (html.Contains("不繼續公開發行")) 
                        throw new ApplicationException(string.Format("The target[{0}] is 不繼續公開發行... ", stockNo));

                    if (html.Contains("查無所需資料"))
                    {
                        _logger.InfoFormat("The target[{0}] is 查無所需資料... stockNo={0}, year={1}, season={2}, month={3}", stockNo, year, season, month);
                        return null;
                    }

                    if (html.Contains("Overrun") || html.Contains("請稍後再試"))
                        throw new WebException(string.Format("The target[{0}] is pissed off... stockNo={0}, year={1}, season={2}, month={3}", stockNo, year, season, month));

                    if (html.Contains("資料庫連線時發生下述問題"))
                    {
                        _logger.Warn("對方資料庫連線時發生問題, 暫停一分鐘後重試.");
                        Thread.Sleep(60 * 1000);
                        continue;
                    }
                    break;
                }
                catch (WebException)
                {
                    _logger.WarnFormat("Target website refuses our connection. Wait till it get peace. stockNo={0}, year={1}, season={2}, month={3}", stockNo, year, season, month);
                    Thread.Sleep((int)new TimeSpan(1, 30, 0).TotalMilliseconds);
                }

            var doc = new HtmlDocument();
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
    }
}
