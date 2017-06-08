using System;
using System.IO;
using System.Text;
using StockCrawler.Services.StockDailyPrice;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StockCrawler.UnitTest
{
    /// <summary>
    ///This is a test class for YahooStockHtmlInfoCollectorTest and is intended
    ///to contain all YahooStockHtmlInfoCollectorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class YahooStockHtmlInfoCollectorTest : UnitTestBase
    {
        /// <summary>
        ///A test for GetStockDailyPriceInfo
        ///</summary>
        [TestMethod()]
        public void GetYahooStockDailyPriceInfoTest()
        {
            MockYahooStockHtmlInfoCollector target = new MockYahooStockHtmlInfoCollector();
            string stock_code = "2002";
            StockDailyPriceInfo expected = new StockDailyPriceInfo()
            {
                StockCode = "2002",
                StockName = "中鋼",
                Volume = 18935,
                Change = new decimal(0.2),
                LastBid = new decimal(31.75),
                LastTrade = new decimal(31.75),
                LastTradeDT = DateTime.Today.AddHours(14.5),
                LastAsk = new decimal(31.80),
                Lowest = new decimal(31.65),
                Open = new decimal(31.7),
                Top = new decimal(31.80),
                PrevClose = new decimal(31.55)
            };
            StockDailyPriceInfo actual = target.GetStockDailyPriceInfo(stock_code);
            Assert.AreEqual<StockDailyPriceInfo>(expected, actual, "The extracted price information is incorrect!");
        }

        /// <summary>
        ///A test for GetAllStockDailyPriceInfo
        ///</summary>
        [TestMethod()]
        public void GetAllStockDailyPriceInfoByTSEMarketLineTest()
        {
            MockYahooStockHtmlInfoCollector target = new MockYahooStockHtmlInfoCollector();
            StockMarketLineEnum marketline = StockMarketLineEnum.tse;
            StockDailyPriceInfo[] actual = target.GetAllStockDailyPriceInfo(marketline);
            Assert.AreEqual<int>(233, actual.Length, "The total stock count is not expected value.");
        }
    }

    internal class MockYahooStockHtmlInfoCollector :
#if(DEBUG)
            YahooStockHtmlInfoCollector
#else
            IStockDailyInfoCollector
#endif
    {
#if(DEBUG)
        public override string GetHtmlText(string url)
#else
        public virtual string GetHtmlText(string url)
#endif
        {
            string htmlText = null;
            string material_file_name = null;
            switch(url){
                case "http://tw.stock.yahoo.com/s/list.php?c=%A4%F4%AAd":
                    material_file_name = "yahoo_cement.htm";
                    break;
                case "http://tw.stock.yahoo.com/s/list.php?c=%AD%B9%AB%7E":
                    material_file_name = "yahoo_food.htm";
                    break;
                case "http://tw.stock.yahoo.com/s/list.php?c=%B6%EC%BD%A6":
                    material_file_name = "yahoo_plastic.htm";
                    break;
                case "http://tw.stock.yahoo.com/s/list.php?c=%AF%BC%C2%B4":
                    material_file_name = "yahoo_spinning.htm";
                    break;
                case "http://tw.stock.yahoo.com/s/list.php?c=%B9q%BE%F7":
                    material_file_name = "yahoo_engine.htm";
                    break;
                case "http://tw.stock.yahoo.com/s/list.php?c=%B9q%BE%B9%B9q%C6l":
                    material_file_name = "yahoo_electronic.htm";
                    break;
                case "http://tw.stock.yahoo.com/s/list.php?c=%A4%C6%BE%C7":
                    material_file_name = "yahoo_chemical.htm";
                    break;
                case "http://tw.stock.yahoo.com/s/list.php?c=%A5%CD%A7%DE%C2%E5%C0%F8":
                    material_file_name = "yahoo_bio.htm";
                    break;
                case "http://tw.stock.yahoo.com/s/list.php?c=%AC%C1%BC%FE":
                    material_file_name = "yahoo_glass.htm";
                    break;
                case "http://tw.stock.yahoo.com/s/list.php?c=%BF%FB%C5K":
                    material_file_name = "yahoo_steel.htm";
                    break;
                case "http://tw.stock.yahoo.com/s/list.php?c=tse":
                    material_file_name = "yahoo_tse_all_stock.htm";
                    break;
                case "http://tw.stock.yahoo.com/s/list.php?c=otc":
                    material_file_name = "yahoo_otc_all_stock.htm";
                    break;
                default:
                    material_file_name = "yahoo_SID_2002.htm";
                    break;
            }
            using (StreamReader sr = new StreamReader(Constants.TEST_MATERIAL_PATH + material_file_name, Encoding.Default))
            {
                htmlText = sr.ReadToEnd();
                sr.Close();
            }
            return htmlText;
        }
#if(!DEBUG)
        public StockDailyPriceInfo GetStockDailyPriceInfo(string stock_code)
        {
            throw new NotImplementedException();
        }

        public StockDailyPriceInfo[] GetAllStockDailyPriceInfo(StockMarketLineEnum marketline)
        {
            throw new NotImplementedException();
        }
#endif
    }
}
