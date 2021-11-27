using Common.Logging;
using StockCrawler.Dao;
using StockCrawler.Services.Collectors;
using System.IO;
using System.Reflection;

namespace StockCrawler.UnitTest.Stubs
{
    class YuantaETFCollectorStub : YuantaETFCollector
    {
        private static readonly ILog _logger = new UnitTestLogger();

        public override GetETFBasicInfoResult GetBasicInfo(string etfNo)
        {
            _logger.Info($"Mock {MethodBase.GetCurrentMethod().Name}!!!");
            var file = new FileInfo($@"..\..\..\StockCrawler.UnitTest\TestData\ETF\{typeof(YuantaETFCollector).Name}\{etfNo}_basic.html");
            if (!file.Directory.Exists) file.Directory.Create();
            string html;
            if (file.Exists)
            {
                _logger.Info($"[{etfNo}]Local file found!");
                using (var sr = file.OpenText())
                    html = sr.ReadToEnd();

                return ParseBasicHtml(html, etfNo);
            }
            else
            {
                _logger.Info($"[{etfNo}]Local file is not found! Try to download it online.");
                return base.GetBasicInfo(etfNo);
            }
        }
        public override GetETFIngredientsResult[] GetIngredients(string etfNo)
        {
            _logger.Info($"Mock {MethodBase.GetCurrentMethod().Name}!!!");
            var file = new FileInfo($@"..\..\..\StockCrawler.UnitTest\TestData\ETF\{typeof(YuantaETFCollector).Name}\{etfNo}_ingredients.html");
            if (!file.Directory.Exists) file.Directory.Create();
            string html;
            if (file.Exists)
            {
                _logger.Info($"[{etfNo}]Local file found!");
                using (var sr = file.OpenText())
                    html = sr.ReadToEnd();

                return ParseIngredientsHtml(html, etfNo).ToArray();
            }
            else
            {
                _logger.Info($"[{etfNo}]Local file is not found! Try to download it online.");
                return base.GetIngredients(etfNo);
            }
        }
    }
}
