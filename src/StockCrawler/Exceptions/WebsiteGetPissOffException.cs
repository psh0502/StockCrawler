using System;

namespace StockCrawler.Services.Exceptions
{
    /// <summary>
    /// 表示網站主發火了, 因為查詢過於頻繁
    /// </summary>
    internal class WebsiteGetPissOffException : ApplicationException
    {
        public WebsiteGetPissOffException() : base() { }
        public WebsiteGetPissOffException(string message) : base(message) { }
    }
}
