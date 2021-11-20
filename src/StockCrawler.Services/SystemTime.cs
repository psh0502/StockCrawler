using System;

namespace StockCrawler.Services
{
    /// <summary>
    /// 單元測試用, 取代系統內建時間 class
    /// </summary>
    public static class SystemTime
    {
        public static string ToDateText(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }
        public static string ToDateTimeText(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }
        private static DateTime FakeTime = DateTime.MinValue;
        private static DateTime LastSetTime { get; set; } = DateTime.MinValue;
        /// <summary>
        /// 判斷該日期是否為周末六日假日
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>是否</returns>
        public static bool IsWeekend(this DateTime date)
        {
            return (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday);
        }
        /// <summary>
        /// 現在的日期，或是假設模式下的日期
        /// </summary>
        public static DateTime Today { get { return Now.Date; } }
        public static DateTime Now
        {
            get
            {
                if (FakeTime == DateTime.MinValue)
                    return DateTime.Now;
                else
                    return FakeTime;
            }
        }
        /// <summary>
        /// 若是進入假設時間模式，可以取得設定假設時間後，若是時間保持流動，現在的假設 Now
        /// </summary>
        public static DateTime RunningNow
        {
            get
            {
                if (FakeTime == DateTime.MinValue)
                    return DateTime.Now;
                else
                    return FakeTime + (DateTime.Now - LastSetTime);
            }
        }
        /// <summary>
        /// 明天的日期
        /// </summary>
        public static DateTime Tomorrow
        {
            get { return Today.AddDays(1); }
        }
        /// <summary>
        /// 昨天的日期
        /// </summary>
        public static DateTime Yesterday
        {
            get { return Today.AddDays(-1); }
        }
        /// <summary>
        /// 一週前的日期
        /// </summary>
        public static DateTime AWeekAgo
        {
            get { return Today.AddDays(-7); }
        }
        /// <summary>
        /// 指定一個假設時間，過去或是未來，以便測試不同時間系統的行為
        /// </summary>
        /// <param name="fakeTime">假設時間，過去或是未來</param>
        public static void SetFakeTime(DateTime fakeTime)
        {
            FakeTime = fakeTime;
        }
        /// <summary>
        /// 取得今日的台灣民國年
        /// </summary>
        /// <returns>回傳今日的台灣民國年</returns>
        public static short GetTaiwanYear()
        {
            return GetTaiwanYear(Today.Year);
        }
        public static short GetTaiwanYear(int westernYear)
        {
            return (short)(westernYear - 1911);
        }
        /// <summary>
        /// 取得日期對應的四季(Q?)
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>季</returns>
        public static short GetSeason(this DateTime date)
        {
            return GetSeason(date.Month);
        }
        public static short GetSeason(int month)
        {
            return (short)(month / 3 + (month % 3 == 0 ? 0 : 1));
        }
        /// <summary>
        /// 取得台灣的中華民國年
        /// </summary>
        /// <param name="date">西園日期物件</param>
        /// <returns>中華民國年</returns>
        public static short GetTaiwanYear(this DateTime date)
        {
            return GetTaiwanYear(date.Year);
        }
        public static DateTime AddSeason(this DateTime date, short season)
        {
            var current_season = GetSeason(date);
            return new DateTime(date.Year, current_season * 3, 1).AddMonths(3 * season);
        }
        /// <summary>
        /// 清除假時間的設定，恢復到正確的系統時間
        /// </summary>
        public static void Reset()
        {
            FakeTime = DateTime.MinValue;
        }
    }
}
