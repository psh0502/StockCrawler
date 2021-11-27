using StockCrawler.Dao;
using System;
using System.Collections.Generic;
using System.IO;

namespace StockCrawler.Services.Collectors
{
    internal abstract class ETFCollectorBase
    {
        public abstract string BasicUrl { get; }
        public abstract string IngredientsUrl { get; }
        public virtual GetETFBasicInfoResult GetBasicInfo(string etfNo)
        {
            string url = string.Format(BasicUrl, etfNo);
            var html = Tools.DownloadStringData(new Uri(url), out _);
            return ParseBasicHtml(html, etfNo);
        }
        public virtual GetETFIngredientsResult[] GetIngredients(string etfNo)
        {
            string url = string.Format(IngredientsUrl, etfNo);
            var html = Tools.DownloadStringData(new Uri(url), out _);
            var result = ParseIngredientsHtml(html, etfNo);
            return result.ToArray();
        }
        protected abstract List<GetETFIngredientsResult> ParseIngredientsHtml(string html, string etfNo);
        protected abstract GetETFBasicInfoResult ParseBasicHtml(string html, string etfNo);
        protected virtual void SaveIngredientsDocument(string html, string etfNo)
        {
            var file = new FileInfo($@"..\..\..\StockCrawler.UnitTest\TestData\ETF\{GetType().Name}\{etfNo}_ingredients.html");
            SaveFile(file, html);
        }
        protected virtual void SaveBasicDocument(string html, string etfNo)
        {
            var file = new FileInfo($@"..\..\..\StockCrawler.UnitTest\TestData\ETF\{GetType().Name}\{etfNo}_basic.html");
            SaveFile(file, html);
        }
        protected virtual void SaveFile(FileInfo file, string html)
        {
#if (DEBUG)
            if (!file.Directory.Exists) file.Directory.Create();
            if (!file.Exists)
                using (var sw = file.CreateText())
                    sw.Write(html);
#endif
        }
    }
}