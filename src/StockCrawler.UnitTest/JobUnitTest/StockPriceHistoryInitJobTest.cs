using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz;
using StockCrawler.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using TomTang.DbAccess;

namespace StockCrawler.UnitTest.JobUnitTest
{
    /// <summary>
    ///This is a test class for StockPriceHistoryInitJobTest and is intended
    ///to contain all StockPriceHistoryInitJobTest Unit Tests
    ///</summary>
    [TestClass]
    public class StockPriceHistoryInitJobTest // : UnitTestBase
    {
        /// <summary>
        ///A test for Execute StockPriceHistoryInit
        ///</summary>
        [TestMethod]
        public void StockPriceHistoryInitTest()
        {
            #region testing data
            //_stockDb.ExecuteCommand.CommandText = string.Format("INSERT INTO Stock(StockNo, StockName) VALUES({0}, {1})",
            //    OleDbStrHelper.getParamStr("1101"),
            //    OleDbStrHelper.getParamStr("台泥"));
            //_stockDb.Execute();
            //_stockDb.ExecuteCommand.CommandText = string.Format("INSERT INTO Stock(StockNo, StockName) VALUES({0}, {1})",
            //    OleDbStrHelper.getParamStr("1102"),
            //    OleDbStrHelper.getParamStr("亞泥"));
            //_stockDb.Execute();
            //_stockDb.ExecuteCommand.CommandText = string.Format("INSERT INTO Stock(StockNo, StockName) VALUES({0}, {1})",
            //    OleDbStrHelper.getParamStr("1103"),
            //    OleDbStrHelper.getParamStr("嘉泥"));
            //_stockDb.Execute();

            //Thread.Sleep(5 * 1000);
            #endregion

            StockPriceHistoryInitJob target = new StockPriceHistoryInitJob();
            IJobExecutionContext context = null;
            target.Execute(context);
        }
        [TestMethod]
        public void TestDownloadFromYahoo()
        {
            try
            {
                HttpWebRequest req = null;
                string s = "https://finance.yahoo.com/quote/2002.TW/history?period1=1339344000&period2=1497110400&interval=1d&filter=history&frequency=1d";
                Cookie c1 = null;
                req = HttpWebRequest.CreateHttp(s);
                req.Method = "GET";
                req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                Uri target = new Uri("https://query1.finance.yahoo.com/");
                string crumb = null;
                using (var res1 = req.GetResponse())
                {
                    string s2 = res1.Headers["Set-Cookie"];
                    c1 = new Cookie("B", s2.Split(';')[0].Substring(2)) { Domain = ".yahoo.com" };
                    string key = "\"CrumbStore\":{\"crumb\":\"";
                    using (var sr = new StreamReader(res1.GetResponseStream(), Encoding.UTF8))
                    {
                        var data = sr.ReadToEnd();
                        int sub_beg = data.IndexOf(key) + key.Length;
                        data = data.Substring(sub_beg);
                        int sub_end = data.IndexOf("\"");
                        crumb = data.Substring(0, sub_end);
                    }
                }

                req = HttpWebRequest.CreateHttp("https://query1.finance.yahoo.com/v7/finance/download/2002.TW?period1=1339344000&period2=1497110400&interval=1d&events=history&crumb=" + crumb);
                req.Method = "GET";
                req.CookieContainer = new CookieContainer();
                req.CookieContainer.Add(c1);
                //req.CookieContainer.Add(new Cookie("B", "877a031cgt2jv&b=3&s=ne") { Domain = target.Host });
                var res = req.GetResponse();
                using (var sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8))
                {
                    var data = sr.ReadToEnd();
                    Debug.WriteLine(data);
                }
                //using (var wc = new WebClient())
                //{
                //    var data = wc.DownloadString("https://query1.finance.yahoo.com/v7/finance/download/2002.TW?period1=1339344000&period2=1497110400&interval=1d&events=history&crumb=mZF39ey4ZER");
                //    Debug.WriteLine(data);
                //}
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
