using System;
using StockCrawler.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StockCrawler.UnitTest
{
    /// <summary>
    ///This is a test class for TimeMachineTest and is intended
    ///to contain all TimeMachineTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TimeMachineTest : UnitTestBase
    {
        private const string formatString = "yyyy/MM/dd HH:mm:ss";
        public override void MyTestInitialize() { }
        /// <summary>
        ///A test for Today
        ///</summary>
        [TestMethod()]
        public void TodayTest()
        {
            TimeMachine.TellMeWhenIsNow(DateTime.Now.AddYears(-1));
            DateTime actual = TimeMachine.Today;
            Assert.AreEqual<string>(DateTime.Today.AddYears(-1).ToString(formatString), actual.ToString(formatString), "TimeMachine can't return correct Today!");
        }
        /// <summary>
        ///A test for Now
        ///</summary>
        [TestMethod()]
        public void NowTest()
        {
            TimeMachine.TellMeWhenIsNow(DateTime.Now.AddYears(-1));
            DateTime actual = TimeMachine.Now;
            Assert.AreEqual<string>(DateTime.Now.AddYears(-1).ToString(formatString), actual.ToString(formatString), "TimeMachine can't return correct Now!");
        }
        /// <summary>
        ///A test for TellMeWhenIsNow
        ///</summary>
        [TestMethod()]
        public void TellMeWhenIsNowTest()
        {
            TimeMachine.TellMeWhenIsNow(DateTime.Now.AddYears(-1));
            Assert.AreEqual<string>(DateTime.Now.AddYears(-1).ToString(formatString), TimeMachine.Now.ToString(formatString), "TimeMachine can't return correct NOW!");
            TimeMachine.TellMeWhenIsNow(DateTime.Now.AddYears(1));
            Assert.AreEqual<string>(DateTime.Now.AddYears(1).ToString(formatString), TimeMachine.Now.ToString(formatString), "TimeMachine can't return correct NOW!");
        }
        /// <summary>
        ///A test for CurrentDifference
        ///</summary>
        [TestMethod()]
        public void CurrentDifferenceTest()
        {
            TimeMachine.Reset();
            TimeSpan expected = new TimeSpan(0);
            TimeSpan actual = TimeMachine.CurrentDifference;
            Assert.AreEqual<TimeSpan>(expected, actual, "CurrentDifference is incorrect!");
        }
    }
}
