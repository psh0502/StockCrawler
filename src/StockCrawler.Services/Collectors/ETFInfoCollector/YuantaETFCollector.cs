using Common.Logging;
using HtmlAgilityPack;
using Newtonsoft.Json;
using ServiceStack.Text;
using StockCrawler.Dao;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace StockCrawler.Services.Collectors
{
    internal class YuantaETFCollector : ETFCollectorBase, IETFInfoCollector
    {
        public YuantaETFCollector()
        {
            _logger = LogManager.GetLogger(typeof(YuantaETFCollector));
        }
        public override string BasicUrl => "https://www.yuantaetfs.com/product/detail/{0}/Basic_information";
        public override string IngredientsUrl => "https://www.yuantaetfs.com/product/detail/{0}/ratio";
        public override GetETFBasicInfoResult GetBasicInfo(string etfNo)
        {
            var basic_info = base.GetBasicInfo(etfNo);

            var url = string.Format(IngredientsUrl, etfNo);
            var html = Tools.DownloadStringData(new Uri(url), out _);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var data_body = doc.DocumentNode.SelectSingleNode("//*[@id='productinfoR']/div/div[2]/div[3]/div/div/div/div/div[2]/div[1]");
            basic_info.TotalAssetNAV = decimal.Parse(data_body.SelectSingleNode("div[1]/div/div[2]/span").InnerText.Replace(",", string.Empty).Replace("NTD $", string.Empty));
            basic_info.NAV = decimal.Parse(data_body.SelectSingleNode("div[2]/div/div[2]/span").InnerText.Replace("NTD $", string.Empty));
            basic_info.TotalPublish = long.Parse(data_body.SelectSingleNode("div[3]/div/div[2]/span").InnerText.Replace(",", string.Empty));
            return basic_info;
        }
        protected override GetETFBasicInfoResult ParseBasicHtml(string html, string etfNo)
        {
            SaveBasicDocument(html, etfNo); 
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var collection = doc.DocumentNode.SelectNodes("//*[@id='productInfoCell']/div/div[1]/div[2]/div/div[@class='col-14 py-2 px-1 fundData col-md-8']");
            var meta = doc.DocumentNode.SelectNodes("//meta");
            var result = new GetETFBasicInfoResult()
            {
                StockNo = etfNo,
                Category = Tools.CleanString(collection[1].InnerText),
                CompanyName = Tools.CleanString(doc.DocumentNode.SelectSingleNode("/html/head/title").InnerText
                    .Replace($"({etfNo})", string.Empty)
                    .Split(' ')[0]),
                BuildDate = DateTime.Parse(Tools.CleanString(collection[2].InnerText)),
                BuildPrice = decimal.Parse(Tools.CleanString(collection[3].InnerText)),
                PublishDate = DateTime.Parse(Tools.CleanString(collection[4].InnerText)),
                PublishPrice = decimal.Parse(Tools.CleanString(collection[5].InnerText)),
                KeepingBank = Tools.CleanString(collection[6].InnerText),
                CEO = Tools.CleanString(collection[8].InnerText),
                Url = string.Format(BasicUrl, etfNo),
                Distribution = Tools.CleanString(collection[10].InnerText) == "是",
                ManagementFee = decimal.TryParse(Tools.CleanString(collection[12].InnerText.Replace("%", string.Empty)), out _) ? decimal.Parse(Tools.CleanString(collection[12].InnerText.Replace("%", string.Empty))) / 100M : 0M,
                KeepFee = decimal.TryParse(Tools.CleanString(collection[13].InnerText.Replace("%", string.Empty)), out _) ? decimal.Parse(Tools.CleanString(collection[13].InnerText.Replace("%", string.Empty))) / 100M : 0M,
                Business = meta[13].Attributes["content"].Value
            };
            return result;
        }
        protected override List<GetETFIngredientsResult> ParseIngredientsHtml(string html, string etfNo)
        {
            SaveIngredientsDocument(html, etfNo);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var js = doc.DocumentNode.SelectSingleNode("/html/body/script[1]").InnerText;
            var js_function = js.Substring(js.IndexOf("function(") + "function(".Length);
            js_function = js_function.Substring(0, js_function.IndexOf(")"));
            _logger.Debug($"{nameof(js_function)}: " + js_function);
            var js_arguments = js_function.Split(',')
                .Select(d => (Key: d, Value: string.Empty))
                .ToList();
            _logger.Debug($"{nameof(js_arguments)} has arguments count: {js_arguments.Count}");
            var js_valueString = js.Substring(js.LastIndexOf("(null,") + 1);
            js_valueString = js_valueString.Substring(0, js_valueString.LastIndexOf(")") - 1);
            js_valueString = js_valueString
                .Replace(@"\n", string.Empty)
                .Replace("\\\"", string.Empty);
            _logger.Debug($"{ nameof(js_valueString)}: {js_valueString}");
            var js_values = CsvReader.ParseFields(js_valueString).ToArray();
            _logger.Debug($"{nameof(js_values)} has arguments count: {js_values.Length}");
            //Debug.Assert(js_arguments.Count == js_values.Length, "argument count != value count");
            var js_argument_map = new Dictionary<string, string>();
            for (var i = 0; i < js_arguments.Count; i++)
            {
                var values = js_values;
                _logger.Debug($"Key: {js_arguments[i].Key}: {values[i].Replace("\\n", string.Empty)}");
                js_argument_map.Add(
                    js_arguments[i].Key,
                    "null" == values[i] ? null : values[i].Replace("\\n", string.Empty));
            }

            js = js.Substring(js.IndexOf("StockWeights:") + "StockWeights:".Length);
            var ingredient_content = js.Substring(0, js.IndexOf("FutureWeights") - 1);
            ingredient_content = ingredient_content
                .Replace("},{", "},\r\n{")
                .Replace("code:", "code:'")
                .Replace(",ym:", "',ym:'")
                .Replace(",name:", "',name:'")
                .Replace(",ename:", "',ename:'")
                .Replace(",weights:", "',weights:'")
                .Replace(",qty:", "',qty:")
                .Replace("'\"", "'")
                .Replace("\"'", "'")
                .Replace(",qty:", ",qty:'")
                .Replace("},\r\n", "'},\r\n")
                .Replace("}]", "'}]");

            _logger.Debug($"{nameof(ingredient_content)}: {ingredient_content}");
            var data = JsonConvert.DeserializeObject<JsonData[]>(ingredient_content);
            int j = 0;
            foreach (var d in data)
            {
                d.code = js_argument_map.ContainsKey(d.code) ? Tools.CleanString(js_argument_map[d.code]) : d.code;
                d.name = js_argument_map.ContainsKey(d.name) ? Tools.CleanString(js_argument_map[d.name]) : d.name;
                d.weights = js_argument_map.ContainsKey(d.weights) ? Tools.CleanString(js_argument_map[d.weights]) : d.weights;
                d.qty = js_argument_map.ContainsKey(d.qty) ? Tools.CleanString(js_argument_map[d.qty]) : d.qty;
                _logger.Debug($"{nameof(d.code)}: {d.code}, " +
                    $"{nameof(d.name)}: {d.name}, " +
                    $"{nameof(d.qty)}: {d.qty}, " +
                    $"{nameof(d.weights)}: {d.weights}");
                j++;
            }
            var result = data.Select(
                da => new GetETFIngredientsResult()
                {
                    ETFNo = etfNo,
                    StockNo = da.code,
                    Quantity = long.Parse(da.qty),
                    Weight = decimal.Parse(da.weights) / 100M,
                }).ToList();
            return result;
        }
        private class JsonData
        {
            public string code { get; set; }
            public string name { get; set; }
            public string weights { get; set; }
            public string qty { get; set; }
        }
#if(DEBUG)
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
#endif
    }
}
