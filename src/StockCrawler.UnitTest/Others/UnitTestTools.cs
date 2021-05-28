using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockCrawler.Dao;
using StockCrawler.Services;
using System;
using System.Linq;

namespace StockCrawler.UnitTest.Others
{
#if(DEBUG)
    /// <summary>
    /// These classes in [Others] folder are not real unit testing.
    /// They are some tools for fixing some issues. 
    /// I put them here for easy to launch.
    /// </summary>
    [TestClass]
    public class UnitTestTools : UnitTestBase
    {
        [TestMethod]
        public void ReGeneratePeriodPriceHistoryForPatchingDeltaDataMissing()
        {
            var bgnDate = DateTime.MinValue;
            using (var db = RepositoryProvider.GetRepositoryInstance())
                bgnDate = db.GetStockPriceHistoryPaging("0000", new DateTime(2000, 1, 1), DateTime.Today, 1, 1, 1, 10, out int? _)
                    .First().StockDT;
            do
            {
                Tools.CalculateMAAndPeriodK(bgnDate);
                bgnDate = bgnDate.AddDays(1);
            } while (bgnDate < DateTime.Today);
        }
    }
#endif
}
