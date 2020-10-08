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
            Assert.AreEqual("127.0.0.1", result);
        }
    }
}
#endif