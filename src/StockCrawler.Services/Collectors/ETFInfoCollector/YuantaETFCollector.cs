using HtmlAgilityPack;
using StockCrawler.Dao;
using System;
using System.Collections.Generic;

namespace StockCrawler.Services.Collectors
{
    internal class YuantaETFCollector : ETFCollectorBase, IETFInfoCollector
    {
        public override string BasicUrl => "https://www.yuantaetfs.com/product/detail/{0}/Basic_information";
        public override string IngredientsUrl => "https://www.yuantaetfs.com/product/detail/{0}/ratio";
        public override GetETFBasicInfoResult GetBasicInfo(string etfNo)
        {
            var basic_info = base.GetBasicInfo(etfNo);
            string url = string.Format(IngredientsUrl, etfNo);
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
                ManagementFee = decimal.Parse(Tools.CleanString(collection[12].InnerText.Replace("%", string.Empty))) / 100M,
                KeepFee = decimal.Parse(Tools.CleanString(collection[13].InnerText.Replace("%", string.Empty))) / 100M,
                Business = meta[13].Attributes["content"].Value
            };
            return result;
        }
        protected override List<GetETFIngredientsResult> ParseIngredientsHtml(string html, string etfNo)
        {
            SaveIngredientsDocument(html, etfNo);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var collection = doc.DocumentNode.SelectNodes("//*[@id='productInfoCell']/div/div[1]/div[2]/div/div[@class='col-14 py-2 px-1 fundData col-md-8']");
            throw new NotImplementedException();
        }
    }
}
