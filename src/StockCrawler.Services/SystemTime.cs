using System;

namespace StockCrawler.Services
{
    /// <summary>
    /// 單元測試用, 取代系統內建時間 class
    /// </summary>
    public static class SystemTime
    {
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
        /// 清除假時間的設定，恢復到正確的系統時間
        /// </summary>
        public static void Reset()
        {
            FakeTime = DateTime.MinValue;
        }
    }
}
