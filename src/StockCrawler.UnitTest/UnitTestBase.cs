using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using TomTang.DbAccess;

namespace StockCrawler.UnitTest
{
    [TestClass]
    public class UnitTestBase
    {
        protected const string CONST_APP_STOCK_CONNECTION_KEY = "StockCrawler.Dao.Properties.Settings.StockConnectionString";
        protected static readonly string _currentAssemblyLocation = null;
        protected static readonly DBOperatorBase _stockDb = null;

        static UnitTestBase()
        {
            FileInfo file = new FileInfo(Assembly.GetExecutingAssembly().Location);
            _currentAssemblyLocation = file.DirectoryName;
            Trace.WriteLine(string.Format("_currentAssemblyCodebase={0}", _currentAssemblyLocation));
            _stockDb = DBOperatorBase.GetDBOperatorObject(DBOperatorType.OleDBOperator,
                string.Format(ConfigurationManager.ConnectionStrings[CONST_APP_STOCK_CONNECTION_KEY].ConnectionString, _currentAssemblyLocation + @"\..\..\..\IRONMAN.UnitTest\database\"));
        }
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        /// <summary>
        /// Use TestInitialize to run code before running each test
        /// </summary>
        [TestInitialize()]
        public virtual void MyTestInitialize()
        {
            // Clean up all tables
            _stockDb.ExecuteCommand.CommandText = "DELETE FROM StockPriceHistory";
            _stockDb.Execute();
            _stockDb.ExecuteCommand.CommandText = "DELETE FROM Stock";
            _stockDb.Execute();
        }
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion
    }
}
