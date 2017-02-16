using StockCrawler.Services.StockDailyPrice;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StockCrawler.UnitTest
{
#if(DEBUG)
    /// <summary>
    ///This is a test class for StockHtmlInfoCollectorBaseTest and is intended
    ///to contain all StockHtmlInfoCollectorBaseTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StockHtmlInfoCollectorBaseTest : UnitTestBase
    {
        public override void MyTestInitialize()
        {
            // Do nothing
        }
        /// <summary>
        ///A test for MergeTabChar
        ///</summary>
        [TestMethod()]
        public void MergeTabCharTest()
        {
            string input = "\ttest\t \ttest \t \t test\t\t\ttest\t\ttest\ttest\t\t\t\t\t\t\t\t\t\t";
            string expected = "test\ttest \ttest\ttest\ttest\ttest";
            string actual = null;
            actual = StockHtmlInfoCollectorBase.MergeTabChar(input);
            Assert.AreEqual<string>(expected, actual, "Result string is not expected.");
        }
    }
#endif
}
