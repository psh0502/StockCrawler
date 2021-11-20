using Common.Logging;
using Quartz;
using StockCrawler.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace StockCrawler.Services
{
    public class StockCaculationJob : JobBase, IJob
    {
        internal static ILog Logger { get; set; } = LogManager.GetLogger(typeof(StockCaculationJob));

        #region IJob Members

        public Task Execute(IJobExecutionContext context)
        {
            Logger.InfoFormat("Invoke [{0}]...", MethodBase.GetCurrentMethod().Name);
            try
            {
                string[] args = null;
                var targetDate = SystemTime.Today;
                if (context != null)
                {
                    args = ((string[])context.Get("args")) ?? new string[] { };
                    if (args.Length > 0)
                    {
                        if (DateTime.TryParse(args[0], out targetDate))
                            args = args.Skip(1).ToArray();
                    }
                    else
                        args = null;
                }
                if(targetDate == DateTime.MinValue) targetDate = SystemTime.Today;
                do
                {
                    CalculateMA(targetDate, args);
                    CalculateTechnicalIndicators(targetDate, args);
                    targetDate = targetDate.AddDays(1);
                } while (targetDate <= SystemTime.Today);
            }
            catch (Exception ex)
            {
                Logger.Error("Job executing failed!", ex);
                throw;
            }
            return null;
        }
        #endregion
        /// <summary>
        /// 根據本日收盤資料, 計算 均線(MA 移動線)
        /// </summary>
        /// <param name="date">計算日期</param>
        /// <param name="args">額外的指定參數, 可為股票代碼。</param>
        internal static void CalculateMA(DateTime date, string[] args = null)
        {
            Logger.InfoFormat("Begin caculation MA...{0}", date.ToString("yyyyMMdd"));
            if (null == args) args = new string[] { };
            var avgPrices = new List<GetStockAveragePriceResult>();
            using (var db = RepositoryProvider.GetRepositoryInstance())
            {
                foreach (var d in StockHelper.GetAllStockList())
                    if (!args.Any() || args.Contains(d.StockNo))
                    {
                        // 週線
                        var period_price = db.CaculateStockClosingAveragePrice(d.StockNo, date, 5);
                        if (period_price > 0) avgPrices.Add(new GetStockAveragePriceResult()
                        {
                            StockNo = d.StockNo,
                            StockDT = date,
                            Period = 5,
                            ClosePrice = period_price
                        });
                        // 雙週線
                        period_price = db.CaculateStockClosingAveragePrice(d.StockNo, date, 10);
                        if (period_price > 0) avgPrices.Add(new GetStockAveragePriceResult()
                        {
                            StockNo = d.StockNo,
                            StockDT = date,
                            Period = 10,
                            ClosePrice = period_price
                        });
                        // 月線
                        period_price = db.CaculateStockClosingAveragePrice(d.StockNo, date, 20);
                        if (period_price > 0) avgPrices.Add(new GetStockAveragePriceResult()
                        {
                            StockNo = d.StockNo,
                            StockDT = date,
                            Period = 20,
                            ClosePrice = period_price
                        });
                        // 季線
                        period_price = db.CaculateStockClosingAveragePrice(d.StockNo, date, 60);
                        if (period_price > 0) avgPrices.Add(new GetStockAveragePriceResult()
                        {
                            StockNo = d.StockNo,
                            StockDT = date,
                            Period = 60,
                            ClosePrice = period_price
                        });
                    }
                // 寫入均價
                if (avgPrices.Any())
                    db.InsertOrUpdateStockAveragePrice(avgPrices.ToArray());
            }
        }
        /// <summary>
        /// 根據本日收盤資料，計算 技術指標，如 KD, MACD
        /// </summary>
        /// <param name="date">計算日期</param>
        /// <param name="args">額外的指定參數, 第一參數可為股票代碼或是技術指標名稱。</param>
        private static void CalculateTechnicalIndicators(DateTime date, string[] args = null)
        {
            Logger.InfoFormat("Begin caculation Indicators...{0}", date.ToDateText());
            var indicators = new List<GetStockTechnicalIndicatorsResult>();
            if (null == args) args = new string[] { };
            var stock = StockHelper.GetStock(args.FirstOrDefault());
            if (null != stock) args = args.Skip(1).ToArray();
            foreach (var d in StockHelper.GetAllStockList())
                if (null == stock || d.StockNo == stock.StockNo)
                {
                    if (!args.Any() || args.Contains("KD"))
                        // 計算 KD 線
                        CalucateKD(d.StockNo, date, ref indicators);
                    if (!args.Any() || args.Contains("MACD"))
                        CaculateMACD(d.StockNo, date, ref indicators);
                    if (!args.Any() || args.Contains("RSI"))
                        CaculateRSI(d.StockNo, date, ref indicators);
                }

            using (var db = RepositoryProvider.GetRepositoryInstance())
                // 寫入均價
                if (indicators.Any())
                    db.InsertOrUpdateStockTechnicalIndicators(indicators.ToArray());
        }
        private static void CaculateRSI(string stockNo, DateTime date, ref List<GetStockTechnicalIndicatorsResult> indicators)
        {
            // TODO: 
        }
        /// <summary>
        /// 平滑異同移動平均線指標
        /// </summary>
        /// <param name="stockNo">股票代碼</param>
        /// <param name="date">計算日期</param>
        /// <param name="indicators">指標清單</param>
        /// <param name="period1">週期1，最常用的值為12天</param>
        /// <param name="period2">週期2，最常用的值為26天</param>
        /// <param name="period3">週期3，最常用的值為9天</param>
        private static void CaculateMACD(string stockNo, DateTime date, ref List<GetStockTechnicalIndicatorsResult> indicators, int period1 = 12, int period2 = 26, int period3 = 9)
        {
            // TODO: 
        }
        /// <summary>
        /// 計算 KD 隨機指標
        /// </summary>
        /// <param name="stockNo">股票代碼</param>
        /// <param name="date">計算日期</param>
        /// <param name="indicators">指標清單</param>
        /// <remarks>K 值 > D 值：上漲行情，適合做多。D 值 > K 值：下跌行情，適合空手或做空。</remarks>
        private static void CalucateKD(string stockNo, DateTime date, ref List<GetStockTechnicalIndicatorsResult> indicators)
        {
            const int period = 9; // 週期, 通常 9 or 14, 經過訪間觀察，大多採用 9 為標準週期
            var rsv = CaculateRSV(stockNo, date, period);
            var k = CaculateK(stockNo, date, rsv);
            var d = CaculateD(stockNo, date, k);
            Logger.Debug($"stockNo: {stockNo} Date: {date.ToShortDateString()} RSV: {rsv}, K: {k}, D: {d}");
            indicators.Add(new GetStockTechnicalIndicatorsResult()
            {
                StockNo = stockNo,
                StockDT = date,
                Type = "K",
                Value = k
            });
            indicators.Add(new GetStockTechnicalIndicatorsResult()
            {
                StockNo = stockNo,
                StockDT = date,
                Type = "D",
                Value = d
            });
        }
        /// <summary>
        /// 慢速平均值，又稱慢線。
        /// </summary>
        /// <param name="stockNo">股票代碼</param>
        /// <param name="date">計算日期</param>
        /// <param name="k">當日 K 值</param>
        /// <returns>以公式來看就知道，今天的 D 值是把昨天 D 值和今天的 K 值再加權平均一次的結果，經過兩次平均後，今天股價對 D 值的影響就比較小，所以 D值對股價變化的反應較不靈敏。</returns>
        private static decimal CaculateD(string stockNo, DateTime date, decimal k)
        {
            using (var db = RepositoryProvider.GetRepositoryInstance())
            {
                var yesterday_d = db.GetStockTechnicalIndicators(stockNo, date.AddDays(-20), date.AddDays(-1), "D")
                    .FirstOrDefault()?.Value;
                return (yesterday_d ?? 0) * 2 / 3 + k / 3;
            }
        }
        /// <summary>
        /// 快速平均值，又稱快線。
        /// </summary>
        /// <param name="stockNo">股票代碼</param>
        /// <param name="date">計算日期</param>
        /// <param name="rsv">未成熟隨機值</param>
        /// <returns>快速平均值(K), 若無資料可計算則回傳 0</returns>
        /// <remarks>以公式來看就知道，今天的 K 值是把昨天的 K 值和今天的 RSV 加權平均的結果，所以對股價變化的反應較靈敏、快速。</remarks>
        private static decimal CaculateK(string stockNo, DateTime date, decimal rsv)
        {
            using (var db = RepositoryProvider.GetRepositoryInstance())
            {
                var yesterday_k = db.GetStockTechnicalIndicators(stockNo, date.AddDays(-20), date.AddDays(-1), "K")
                    .FirstOrDefault()?.Value;
                return (yesterday_k ?? 0) * 2 / 3 + rsv / 3;
            }
        }
        /// <summary>
        /// 計算 RSV = (C-L)/(H-L), 中文:未成熟隨機值, 衡量當天收盤價在這 period 天內來說，股價是強勢還是弱勢
        /// </summary>
        /// <param name="stockNo">股票代碼</param>
        /// <param name="date">當天日期</param>
        /// <param name="period">週期, 通常 9 or 14</param>
        /// <returns>RSV 未成熟隨機值, 若無資料可計算則回傳 0</returns>
        /// <remarks>計算方式是「(該日收盤價 – 最近 period 天的最低價)÷(最近 period 天的最高價 – 最近 period 天最低價)」</remarks>
        private static decimal CaculateRSV(string stockNo, DateTime date, int period)
        {
            using (var db = RepositoryProvider.GetRepositoryInstance())
                try
                {
                    // 該日收盤價
                    var c = db.GetStockPriceHistory(stockNo, date, date).First().ClosePrice;
                    // 最近 period 天的最低價
                    var l = db.GetStockPriceHistory(stockNo, date.AddDays(-period * 2), date).Take(period).Min(d => d.LowPrice);
                    // 最近 period 天的最高價
                    var h = db.GetStockPriceHistory(stockNo, date.AddDays(-period * 2), date).Take(period).Max(d => d.HighPrice);
                    Logger.Debug($"stockNo: {stockNo} Date: {date.ToShortDateString()} C:{c},H:{h},L:{l}");
                    if ((h - l) > 0)
                        return (c - l) / (h - l) * 100;
                    else
                        return 0;
                }
                catch (InvalidOperationException)
                {
                    return 0;
                }
        }
    }
}