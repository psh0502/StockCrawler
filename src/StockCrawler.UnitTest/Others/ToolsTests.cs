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
            Assert.AreEqual("220.135.20.48", result);
        }
    }
}
#endif