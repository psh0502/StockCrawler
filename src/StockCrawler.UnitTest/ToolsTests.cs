using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockCrawler.UnitTest;

namespace StockCrawler.Services.Tests
{
    [TestClass()]
    public class ToolsTests : UnitTestBase
    {
        [TestMethod()]
        public void GetMyIpAddressTest()
        {
            var result = Tools.GetMyIpAddress();
            Assert.AreEqual("220.135.20.48", result);
        }
    }
}