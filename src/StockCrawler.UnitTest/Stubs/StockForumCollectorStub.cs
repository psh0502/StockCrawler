using StockCrawler.Services.Collectors;
using System;
using System.IO;

#if (DEBUG)
namespace StockCrawler.UnitTest.Stubs
{
    internal class StockForumCollectorStub : StockForumCollector
    {
        public StockForumCollectorStub() : base()
        {
            _logger = new UnitTestLogger();
        }
        /// <summary>
        /// 抽出下載資料部位, 方便單元測試替換掉塞固定資料
        /// </summary>
        /// <param name="uri">下載資料連結</param>
        /// <returns>從測試資料檔案內直接回傳已經下載的 html 資料</returns>
        protected override string DownloadData(Uri uri)
        {
            _logger.Info($"Mock DownloadData!!!uri={uri}");
            var file_name = uri.LocalPath.Replace("/", string.Empty);
            if (!string.IsNullOrEmpty(file_name))
                if (!file_name.EndsWith(".html")) file_name += ".html";

            var file = new FileInfo($@"..\..\..\StockCrawler.UnitTest\TestData\Ptt\{file_name}");
            if (file.Exists)
                using (var sr = file.OpenText())
                    return sr.ReadToEnd();
            else
                return null;
        }
    }
}
#endif