using Newtonsoft.Json;
using StockCrawler.Services.Collectors;

namespace StockCrawler.UnitTest.Stubs
{
    internal class LazyStockCollectorStub : ILazyStockCollector
    {
        public LazyStockData GetData(string stockNo)
        {
            return JsonConvert.DeserializeObject<LazyStockData>("{ \"result\":{ \"DData\":{ \"Y\":\"2020\",\"StockName\":\"2330 台積電\",\"Category\":\"半導體業\",\"Price\":\"515(2020-12-29)\",\"StockCashDivi\":2.5000,\"DiviRatio\":\"0.5%\",\"DiviType\":\"每季\",\"TotalAmountSimple\":\"2,593億\",\"YOY\":\"26.4%\",\"isFullYear\":false},\"Analysis\":{ \"IsPromisingEPS\":true,\"IsGrowingUpEPS\":false,\"IsAlwaysIncomeEPS\":true,\"IsAlwaysPayDivi\":true,\"IsStableDivi\":false,\"IsAlwaysRestoreDivi\":true,\"IsStableOutsideIncome\":true,\"IsStableTotalAmount\":true,\"IsGrowingUpRevenue\":true},\"PriceStg\":{ \"hasDivi\":true,\"IsRealMode\":true,\"Price5\":50.00,\"Price6\":41.67,\"Price7\":35.71,\"CurrPrice\":515.0000},\"hasLogin\":true,\"StockNum\":\"2330\"},\"code\":0,\"msg\":\"成功\"}");
        }
    }
}
