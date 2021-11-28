using HtmlAgilityPack;
using StockCrawler.Dao;
using StockCrawler.Services.Collectors;
using System.IO;
using System.Reflection;

namespace StockCrawler.UnitTest.Stubs
{
    class YuantaETFCollectorStub : YuantaETFCollector
    {
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

                var result = ParseBasicHtml(html, etfNo);
                file = new FileInfo($@"..\..\..\StockCrawler.UnitTest\TestData\ETF\{typeof(YuantaETFCollector).Name}\{etfNo}_ingredients.html");
                if (file.Exists)
                {
                    _logger.Info($"[{etfNo}]Local file found!");
                    using (var sr = file.OpenText())
                        html = sr.ReadToEnd();

                    var doc = new HtmlDocument();
                    doc.LoadHtml(html);
                    var data_body = doc.DocumentNode.SelectSingleNode("//*[@id='productinfoR']/div/div[2]/div[3]/div/div/div/div/div[2]/div[1]");
                    result.TotalAssetNAV = decimal.Parse(data_body.SelectSingleNode("div[1]/div/div[2]/span").InnerText.Replace(",", string.Empty).Replace("NTD $", string.Empty));
                    result.NAV = decimal.Parse(data_body.SelectSingleNode("div[2]/div/div[2]/span").InnerText.Replace("NTD $", string.Empty));
                    result.TotalPublish = long.Parse(data_body.SelectSingleNode("div[3]/div/div[2]/span").InnerText.Replace(",", string.Empty));
                    return result;
                }
                else
                {
                    _logger.Info($"[{etfNo}]Local file is not found! Try to download it online.");
                    return base.GetBasicInfo(etfNo);
                }
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
