using System;

namespace StockCrawler.Services
{
    /// <summary>
    /// 單元測試用, 取代系統內建時間 class
    /// </summary>
    internal static class SystemTime
    {
        private static DateTime FakeTime = DateTime.MinValue;

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

        public static void SetFakeTime(DateTime fakeTime)
        {
            FakeTime = fakeTime;
        }

        public static void Reset()
        {
            FakeTime = DateTime.MinValue;
        }
    }
}
