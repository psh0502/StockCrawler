using StockCrawler.Dao;
using StockCrawler.Services.Collectors;
using System;

namespace StockCrawler.UnitTest.Stubs
{
    internal class StockBasicInfoCollectorStub : IStockBasicInfoCollector
    {
        private const string TEST_STOCK_NO_1 = "2330";
        public GetStockBasicInfoResult GetStockBasicInfo(string stockNo)
        {
            switch (stockNo)
            {
                case TEST_STOCK_NO_1:
                    return new GetStockBasicInfoResult()
                    {
                        StockNo = TEST_STOCK_NO_1,
                        StockName = "台積電",
                        BuildDate = new DateTime(1987, 2, 21),
                        PublishDate = new DateTime(1994, 9, 5),
                        Capital = 259303804580M,
                        Chairman = "劉德音",
                        CEO = "總裁: 魏哲家",
                        Url = "http://www.tsmc.com",
                        Category = "半導體業",
                        Business = "依客戶之訂單與其提供之產品設計說明，以從事製造與銷售積體電路以及其他晶圓半導體裝置。提供前述產品之封裝與測試服務、積體電路之電腦輔助設計技術服務。提供製造光罩及其設計服務。",
                        CompanyID = "22099131",
                        CompanyName = "台灣積體電路製造股份有限公司",
                        ReleaseStockCount = 25930380458,
                    };
                case "2888":
                    return new GetStockBasicInfoResult()
                    {
                        StockNo = "2888",
                        StockName = "新光金",
                        BuildDate = new DateTime(2002, 2, 19),
                        PublishDate = new DateTime(2002, 2, 19),
                        Capital = 1309.5M * 100000000,
                        Chairman = "吳東進",
                        CEO = "吳欣儒",
                        Url = "https://www.skfh.com.tw",
                        Category = "金控業",
                        Business = "H801011金融控股公司業",
                        CompanyID = "80328219",
                        CompanyName = "新光金融控股股份有限公司",
                        ReleaseStockCount = 13020394063,
                    };
                default:
                    return null;
            }
        }
    }
}
