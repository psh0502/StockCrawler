using Newtonsoft.Json;
using StockCrawler.Dao;

namespace StockCrawler.Services.Collectors
{
    public class LazyStockData
    {
        [JsonProperty("result")]
        public Result Result { get; set; }
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("msg")]
        public string Msg { get; set; }
        /// <summary>
        /// 將 json 結構轉換成 dao 結構的物件
        /// </summary>
        /// <returns>dao 結構的物件</returns>
        public GetLazyStockDataResult ToDbObject()
        {
            return new GetLazyStockDataResult()
            {
                StockNo = Result.StockNum,
                StockCashDivi = Result.DData.StockCashDivi,
                DiviRatio = Result.DData.DiviRatio,
                DiviType = Result.DData.DiviType,
                IsPromisingEPS = Result.Analysis.IsPromisingEPS,
                IsGrowingUpEPS = Result.Analysis.IsGrowingUpEPS,
                IsGrowingUpRevenue = Result.Analysis.IsGrowingUpRevenue,
                IsRealMode = Result.PriceStg.IsRealMode,
                IsStableDivi = Result.Analysis.IsStableDivi,
                IsStableOutsideIncome = Result.Analysis.IsStableOutsideIncome,
                IsAlwaysIncomeEPS = Result.Analysis.IsAlwaysIncomeEPS,
                IsAlwaysPayDivi = Result.Analysis.IsAlwaysPayDivi,
                IsAlwaysRestoreDivi = Result.Analysis.IsAlwaysRestoreDivi,
                IsStableTotalAmount = Result.Analysis.IsStableTotalAmount,
                HasDivi = Result.PriceStg.HasDivi,
                Price = Result.DData.Price,
                Price5 = Result.PriceStg.Price5,
                Price6 = Result.PriceStg.Price6,
                Price7 = Result.PriceStg.Price7,
                CurrPrice = Result.PriceStg.CurrPrice,
                LastModifiedAt = SystemTime.Now
            };
        }
    }

    #region inner classes
    public class DData
    {
        public string Y { get; set; }
        public string StockName { get; set; }
        public string Category { get; set; }
        public string Price { get; set; }
        public decimal StockCashDivi { get; set; }
        public string DiviRatio { get; set; }
        public string DiviType { get; set; }
        public string TotalAmountSimple { get; set; }
        public string YOY { get; set; }
        [JsonProperty("isFullYear")]
        public bool IsFullYear { get; set; }
    }

    public class Analysis
    {
        public bool IsPromisingEPS { get; set; }
        public bool IsGrowingUpEPS { get; set; }
        public bool IsAlwaysIncomeEPS { get; set; }
        public bool IsAlwaysPayDivi { get; set; }
        public bool IsStableDivi { get; set; }
        public bool IsAlwaysRestoreDivi { get; set; }
        public bool IsStableOutsideIncome { get; set; }
        public bool IsStableTotalAmount { get; set; }
        public bool IsGrowingUpRevenue { get; set; }
    }

    public class PriceStg
    {
        [JsonProperty("hasDivi")]
        public bool HasDivi { get; set; }
        public bool IsRealMode { get; set; }
        public decimal Price5 { get; set; }
        public decimal Price6 { get; set; }
        public decimal Price7 { get; set; }
        public decimal CurrPrice { get; set; }
    }

    public class Result
    {
        public DData DData { get; set; }
        public Analysis Analysis { get; set; }
        public PriceStg PriceStg { get; set; }
        [JsonProperty("hasDivi")]
        public bool HasLogin { get; set; }
        public string StockNum { get; set; }
    }
    #endregion
}