using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockCrawler.Services;

#if (DEBUG)
namespace StockCrawler.UnitTest.Others
{
    [TestClass]
    public class ToolsTests : UnitTestBase
    {
        [TestMethod]
        public void GetMyIpAddressTest()
        {
            var result = Tools.GetMyIpAddress();
            var expect = Tools.GetMyIpAddress2();
            Assert.AreEqual(expect, result);
        }
    }
}
#endif