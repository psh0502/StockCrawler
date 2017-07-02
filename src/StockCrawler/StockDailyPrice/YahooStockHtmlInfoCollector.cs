using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace StockCrawler.Services.StockDailyPrice
{
    [Obsolete("Since Yahoo has changed the html structure, this class is no useful anymore. Please don't use it.", true)]
#if(DEBUG)
    public class YahooStockHtmlInfoCollector : StockHtmlInfoCollectorBase, IStockDailyInfoCollector
#else
    internal class YahooStockHtmlInfoCollector : StockHtmlInfoCollectorBase, IStockDailyInfoCollector
#endif
    {
        private Dictionary<string, StockDailyPriceInfo> _stockInfoDict = null;

        #region IStockHtmlTextCollector Members

        public virtual StockDailyPriceInfo GetStockDailyPriceInfo(string stock_code)
        {
            if (null == _stockInfoDict)
            {
                _logger.Info("Initialize all stock information cache.");
                _stockInfoDict = new Dictionary<string, StockDailyPriceInfo>();
                foreach (var info in GetAllStockDailyPriceInfo(StockMarketLineEnum.tse))
                {
                    _stockInfoDict.Add(info.StockCode, info);
                }
            }

            return (_stockInfoDict.ContainsKey(stock_code)) ? _stockInfoDict[stock_code] : null;
        }

        public StockDailyPriceInfo[] GetAllStockDailyPriceInfo(StockMarketLineEnum marketline)
        {
            const string 水泥_URL = "http://tw.stock.yahoo.com/s/list.php?c=%A4%F4%AAd";
            const string 食品_URL = "http://tw.stock.yahoo.com/s/list.php?c=%AD%B9%AB%7E";
            const string 塑膠_URL = "http://tw.stock.yahoo.com/s/list.php?c=%B6%EC%BD%A6";
            const string 紡織_URL = "http://tw.stock.yahoo.com/s/list.php?c=%AF%BC%C2%B4";
            const string 電機_URL = "http://tw.stock.yahoo.com/s/list.php?c=%B9q%BE%F7";
            const string 電器電纜_URL = "http://tw.stock.yahoo.com/s/list.php?c=%B9q%BE%B9%B9q%C6l";
            const string 化學_URL = "http://tw.stock.yahoo.com/s/list.php?c=%A4%C6%BE%C7";
            const string 生技醫療_URL = "http://tw.stock.yahoo.com/s/list.php?c=%A5%CD%A7%DE%C2%E5%C0%F8";
            const string 玻璃_URL = "http://tw.stock.yahoo.com/s/list.php?c=%AC%C1%BC%FE";
            const string 造紙_URL = "http://tw.stock.yahoo.com/s/list.php?c=%B3y%AF%C8";
            const string 鋼鐵_URL = "http://tw.stock.yahoo.com/s/list.php?c=%BF%FB%C5K";
            const string 橡膠_URL = "http://tw.stock.yahoo.com/s/list.php?c=%BE%F3%BD%A6";
            const string 汽車_URL = "http://tw.stock.yahoo.com/s/list.php?c=%A8T%A8%AE";
            const string 半導體_URL = "http://tw.stock.yahoo.com/s/list.php?c=%A5b%BE%C9%C5%E9";
            const string 電腦週邊_URL = "http://tw.stock.yahoo.com/s/list.php?c=%B9q%B8%A3%B6g%C3%E4";
            const string 光電_URL = "http://tw.stock.yahoo.com/s/list.php?c=%A5%FA%B9q";
            const string 通信網路_URL = "http://tw.stock.yahoo.com/s/list.php?c=%B3q%ABH%BA%F4%B8%F4";
            const string 電子零組件_URL = "http://tw.stock.yahoo.com/s/list.php?c=%B9q%A4l%B9s%B2%D5%A5%F3";
            const string 電子通路_URL = "http://tw.stock.yahoo.com/s/list.php?c=%B9q%A4l%B3q%B8%F4";
            const string 資訊服務_URL = "http://tw.stock.yahoo.com/s/list.php?c=%B8%EA%B0T%AAA%B0%C8";
            const string 其它電子_URL = "http://tw.stock.yahoo.com/s/list.php?c=%A8%E4%A5%A6%B9q%A4l";
            const string 營建_URL = "http://tw.stock.yahoo.com/s/list.php?c=%C0%E7%AB%D8";
            const string 航運_URL = "http://tw.stock.yahoo.com/s/list.php?c=%AF%E8%B9B";
            const string 觀光_URL = "http://tw.stock.yahoo.com/s/list.php?c=%C6%5B%A5%FA";
            const string 金融_URL = "http://tw.stock.yahoo.com/s/list.php?c=%AA%F7%BF%C4";
            const string 貿易百貨_URL = "http://tw.stock.yahoo.com/s/list.php?c=%B6T%A9%F6%A6%CA%B3f";
            const string 油電燃氣_URL = "http://tw.stock.yahoo.com/s/list.php?c=%AAo%B9q%BFU%AE%F0";
            const string 其他_URL = "http://tw.stock.yahoo.com/s/list.php?c=%A8%E4%A5L";
            const string 綜合_URL = "http://tw.stock.yahoo.com/s/list.php?c=%BA%EE%A6X";
            const string 憑證_URL = "http://tw.stock.yahoo.com/s/list.php?c=%BE%CC%C3%D2";

#if(DEBUG)
            string[] urls = { 水泥_URL, 食品_URL, 塑膠_URL, 紡織_URL, 電機_URL, 電器電纜_URL, 化學_URL, 生技醫療_URL, 玻璃_URL, 鋼鐵_URL };
#else
            string[] urls = { 
                                水泥_URL, 食品_URL, 塑膠_URL, 紡織_URL, 電機_URL, 電器電纜_URL, 化學_URL, 生技醫療_URL, 玻璃_URL, 造紙_URL, 鋼鐵_URL,
                                橡膠_URL,汽車_URL,半導體_URL,電腦週邊_URL,光電_URL,通信網路_URL,電子零組件_URL,電子通路_URL,資訊服務_URL,其它電子_URL,
                                營建_URL,航運_URL,觀光_URL,金融_URL,貿易百貨_URL,油電燃氣_URL,其他_URL,綜合_URL,憑證_URL
                            };
#endif

            //const string CONST_URL_TEMPLATE = "http://tw.stock.yahoo.com/s/list.php?c={0}";
            //string url = string.Format(CONST_URL_TEMPLATE, marketline);

            int year = 0;
            int month = 0;
            int day = 0;

            List<StockDailyPriceInfo> list = new List<StockDailyPriceInfo>();
            foreach (var url in urls)
            {
                string tmp = GetHtmlText(url);

                const string CONST_DATA_DT_BGN_KEY_WORD = "資料日期：";
                const string CONST_DATA_DT_END_KEY_WORD = "</FONT></td>";

                #region parsing data timestamp
                if (0 == year)  // If we had extracted datetime info
                {
                    string tmpDT = tmp.Substring(tmp.IndexOf(CONST_DATA_DT_BGN_KEY_WORD) + CONST_DATA_DT_BGN_KEY_WORD.Length);
                    tmpDT = tmpDT.Substring(0, tmpDT.IndexOf(CONST_DATA_DT_END_KEY_WORD));
                    string[] tmpDTs = tmpDT.Split('/');
                    year = int.Parse(tmpDTs[0]) + 1911;
                    month = int.Parse(tmpDTs[1]);
                    day = int.Parse(tmpDTs[2]);
                }
                #endregion

                // Strip useless html text
                const string CONST_BGN_KEY_WORD = "凱基證券下單";
                const string CONST_END_KEY_WORD = "取消全部選擇";
                tmp = tmp.Substring(tmp.IndexOf(CONST_BGN_KEY_WORD) + CONST_BGN_KEY_WORD.Length);
                tmp = tmp.Substring(0, tmp.IndexOf(CONST_END_KEY_WORD)).Replace("\t", string.Empty).Replace("\r\n", string.Empty).Replace("\n", string.Empty).Trim();

                // Strip HTML tag text and remove garbage text
                tmp = MergeTabChar(StripHTML(tmp, '\t'));
                tmp = tmp.Replace("\t買\t賣\t張\t零股交易\t", "\t\t").Replace("－", "-1");

                _logger.DebugFormat("Extracted info text = {0}", (tmp.Length > 500) ? tmp.Substring(0, 500) + "..." : tmp);

                string[] infoArray = tmp.Split(new string[] { "\t\t" }, StringSplitOptions.RemoveEmptyEntries);
                const int IDX_STOCKNAME = 0,
                    IDX_LAST_TRADE_DATETIME = 1,
                    IDX_LAST_TRADE = 2,
                    IDX_LAST_BID = 3,
                    IDX_LAST_ASK = 4,
                    IDX_CHANGE = 5,
                    IDX_VOLUME = 6,
                    IDX_PREV_CLOSE = 7,
                    IDX_OPEN = 8,
                    IDX_TOP = 9,
                    IDX_LOWEST = 10;

                string stock_code = null;
                foreach (var info in infoArray)
                {
                    try
                    {
                        stock_code = null;
                        //The string format is like: TWEI 發達指\t13:34\t7061\t－\t－\t▽7\t－\t7069\t7054\t7061\t7016
                        string[] s = info.Split('\t');
                        if (s.Length >= 11)
                        {
                            StockDailyPriceInfo infoObj = new StockDailyPriceInfo();

                            infoObj.StockCode = s[IDX_STOCKNAME].Split(' ')[0];
                            infoObj.StockName = s[IDX_STOCKNAME].Split(' ')[1];
                            stock_code = infoObj.StockCode;

                            s[IDX_VOLUME] = s[IDX_VOLUME].Replace(",", string.Empty);
                            s[IDX_CHANGE] = (s[IDX_CHANGE][0] == '△') ? s[IDX_CHANGE].Substring(1) : "-" + s[IDX_CHANGE].Substring(1);

                            long tlong = -1;
                            if (long.TryParse(s[IDX_VOLUME], out tlong)) infoObj.Volume = tlong; else _logger.Warn(string.Format("SNO={0} : string data parsing failure for \"{1}\"! raw string={2}", infoObj.StockCode, "Volumn", s[IDX_VOLUME]));
                            decimal tdecimal = -1;
                            if (decimal.TryParse(s[IDX_CHANGE], out tdecimal)) infoObj.Change = tdecimal; else _logger.Warn(string.Format("SNO={0} : string data parsing failure for \"{1}\"! raw string={2}", infoObj.StockCode, "Change", s[IDX_CHANGE]));
                            tdecimal = -1;
                            if (decimal.TryParse(s[IDX_LAST_BID], out tdecimal)) infoObj.LastBid = tdecimal; else _logger.Warn(string.Format("SNO={0} : string data parsing failure for \"{1}\"! raw string={2}", infoObj.StockCode, "LastBid", s[IDX_LAST_BID]));
                            tdecimal = -1;
                            if (decimal.TryParse(s[IDX_LAST_TRADE], out tdecimal)) infoObj.LastTrade = tdecimal; else _logger.Warn(string.Format("SNO={0} : string data parsing failure for \"{1}\"! raw string={2}", infoObj.StockCode, "LastTrade", s[IDX_LAST_TRADE]));
                            tdecimal = -1;
                            if (decimal.TryParse(s[IDX_LAST_ASK], out tdecimal)) infoObj.LastAsk = tdecimal; else _logger.Warn(string.Format("SNO={0} : string data parsing failure for \"{1}\"! raw string={2}", infoObj.StockCode, "LastAsk", s[IDX_LAST_ASK]));
                            tdecimal = -1;
                            if (decimal.TryParse(s[IDX_LOWEST], out tdecimal)) infoObj.Lowest = tdecimal; else _logger.Warn(string.Format("SNO={0} : string data parsing failure for \"{1}\"! raw string={2}", infoObj.StockCode, "Lowest", s[IDX_LOWEST]));
                            tdecimal = -1;
                            if (decimal.TryParse(s[IDX_OPEN], out tdecimal)) infoObj.Open = tdecimal; else _logger.Warn(string.Format("SNO={0} : string data parsing failure for \"{1}\"! raw string={2}", infoObj.StockCode, "Open", s[IDX_OPEN]));
                            tdecimal = -1;
                            if (decimal.TryParse(s[IDX_TOP], out tdecimal)) infoObj.Top = tdecimal; else _logger.Warn(string.Format("SNO={0} : string data parsing failure for \"{1}\"! raw string={2}", infoObj.StockCode, "Top", s[IDX_TOP]));
                            tdecimal = -1;
                            if (decimal.TryParse(s[IDX_PREV_CLOSE], out tdecimal)) infoObj.PrevClose = tdecimal; else _logger.Warn(string.Format("SNO={0} : string data parsing failure for \"{1}\"! raw string={2}", infoObj.StockCode, "PrevClose", s[IDX_PREV_CLOSE]));
                            DateTime tdatetime = new DateTime(1900, 1, 1);
                            if (DateTime.TryParse(s[IDX_LAST_TRADE_DATETIME], out tdatetime)) infoObj.LastTradeDT = tdatetime; else _logger.Warn(string.Format("SNO={0} : string data parsing failure for \"{1}\"! raw string={2}", infoObj.StockCode, "LastTradeDT", s[IDX_LAST_TRADE_DATETIME]));
                            list.Add(infoObj);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(string.Format("StockCode=[{0}]: {1}", stock_code, ex.Message), ex);
#if!(DEBUG)
                    throw;
#endif
                    }
                }
            }
            return list.ToArray();
        }
        #endregion
    }
}
