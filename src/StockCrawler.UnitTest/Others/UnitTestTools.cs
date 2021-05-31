using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockCrawler.Dao;
using StockCrawler.Services;
using StockCrawler.Services.Collectors;
using System;
using System.Collections.Generic;
using System.Threading;

namespace StockCrawler.UnitTest.Others
{
#if(DEBUG)
    /// <summary>
    /// These classes in [Others] folder are not real unit testing.
    /// They are some tools for fixing some issues. 
    /// I put them here for easy to launch.
    /// </summary>
    [TestClass]
    public class UnitTestTools
    {
        [TestMethod]
        public void ReGeneratePeriodPriceHistoryForPatchingDeltaDataMissing()
        {
            var _logger = new UnitTestLogger
            {
                IsDebugEnabled = false
            };
            Tools._logger = _logger;
            var bgnDate = new DateTime(2015, 09, 30);
            _logger.InfoFormat("bgnDate = {0:yyyy-MM-dd}", bgnDate);
            do
            {
                Tools.CalculateMAAndPeriodK(bgnDate);
                bgnDate = bgnDate.AddDays(1);
            } while (bgnDate < DateTime.Today);
        }
        [TestMethod]
        public void Patch0022dailyInfo()
        {
            var _logger = new UnitTestLogger { IsDebugEnabled = false };
            Tools._logger = _logger;
            var bgnDate = new DateTime(2016, 07, 02);
            _logger.InfoFormat("bgnDate = {0:yyyy-MM-dd}", bgnDate);
            var collector = new TwseStockDailyInfoCollector();
            do
            {
                var list = new List<GetStockPeriodPriceResult>();
                var d = collector.GetStockDailyPriceInfo("0022", bgnDate);
                if (null != d) list.Add(d);
                using (var db = RepositoryProvider.GetRepositoryInstance())
                    db.InsertOrUpdateStockPrice(list.ToArray());
                bgnDate = bgnDate.AddDays(1);
                Thread.Sleep(10 * 1000);

            } while (bgnDate < DateTime.Today);
        }
    }
#endif
}
