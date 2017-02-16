using System;
using System.IO;
using System.Text;
using StockCrawler.Services.StockDailyPrice;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StockCrawler.UnitTest
{
#if(DEBUG)
    /// <summary>
    ///This is a test class for PchomeStockHtmlInfoCollectorTest and is intended
    ///to contain all PchomeStockHtmlInfoCollectorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PchomeStockHtmlInfoCollectorTest : UnitTestBase
    {
        /// <summary>
        ///A test for GetStockDailyPriceInfo
        ///</summary>
        [TestMethod()]
        public void GetPcHomeStockDailyPriceInfoTest()
        {
            MockPchomeStockHtmlInfoCollector target = new MockPchomeStockHtmlInfoCollector();
            string stock_code = "2002";
            StockDailyPriceInfo expected = new StockDailyPriceInfo()
            {
                StockCode = "2002",
                Volumn = 18935,
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

        internal class MockPchomeStockHtmlInfoCollector : 
            PchomeStockHtmlInfoCollector
        {
            public override string GetHtmlText(string stock_code)
            {
                string htmlText = null;
                using (StreamReader sr = new StreamReader(Constants.TEST_MATERIAL_PATH + "pchome_SID_2002.htm", Encoding.Default))
                {
                    htmlText = sr.ReadToEnd();
                    sr.Close();
                }
                return htmlText;
            }
        }
    }
#endif
}
