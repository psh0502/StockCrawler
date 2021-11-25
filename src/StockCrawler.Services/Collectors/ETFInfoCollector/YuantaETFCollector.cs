using HtmlAgilityPack;
using StockCrawler.Dao;
using System;
using System.Collections.Generic;

namespace StockCrawler.Services.Collectors
{
    internal class YuantaETFCollector : IETFInfoCollector
    {
        public GetETFBasicInfoResult GetBasicInfo(string etfNo)
        {
            string url = $"https://www.yuantaetfs.com/product/detail/{etfNo}/Basic_information";
            var html = Tools.DownloadStringData(new Uri(url), out _);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            var result = new GetETFBasicInfoResult()
            {
                StockNo = etfNo,
            };
            throw new NotImplementedException();
            return result;
        }

        public GetETFIngredientsResult[] GetIngredients(string etfNo)
        {
            string url = $"https://www.yuantaetfs.com/product/detail/{etfNo}/ratio";
            var html = Tools.DownloadStringData(new Uri(url), out _);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            var result = new List<GetETFIngredientsResult>();

            result.Add(new GetETFIngredientsResult()
            {
                ETFNo = etfNo
            });
            throw new NotImplementedException();
            return result.ToArray();
        }
    }
}
