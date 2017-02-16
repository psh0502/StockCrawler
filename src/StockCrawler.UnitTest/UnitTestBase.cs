using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using DOWILL.DBAccess;
using StockCrawler.Core;
using StockCrawler.Web.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StockCrawler.UnitTest
{
    [TestClass]
    public class UnitTestBase
    {
        protected const string CONST_APP_STOCK_CONNECTION_KEY = "IRONMAN.DAO.Properties.Settings.StockConnectionString";
        protected const string CONST_APP_TRANS_CONNECTION_KEY = "IRONMAN.DAO.Properties.Settings.TransactionConnectionString";
        protected const string CONST_APP_CORE_CONNECTION_KEY = "IRONMAN.DAO.Properties.Settings.CoreConnectionString";
        protected static readonly string _currentAssemblyLocation = null;
        protected static DBOperatorBase _coreDb = null;
        protected static readonly DBOperatorBase _stockDb = null;
        protected static DBOperatorBase _transDb = null;

        static UnitTestBase()
        {
            FileInfo file = new FileInfo(Assembly.GetExecutingAssembly().Location);
            _currentAssemblyLocation = file.DirectoryName;
            Trace.WriteLine(string.Format("_currentAssemblyCodebase={0}", _currentAssemblyLocation));
            _stockDb = DBOperatorBase.GetDBOperatorObject(DBOperatorType.OleDBOperator,
                string.Format(ConfigurationManager.ConnectionStrings[CONST_APP_STOCK_CONNECTION_KEY].ConnectionString, _currentAssemblyLocation + @"\..\..\..\IRONMAN.UnitTest\database\"));
            _transDb = DBOperatorBase.GetDBOperatorObject(DBOperatorType.OleDBOperator,
                string.Format(ConfigurationManager.ConnectionStrings[CONST_APP_TRANS_CONNECTION_KEY].ConnectionString, _currentAssemblyLocation + @"\..\..\..\IRONMAN.UnitTest\database\"));
            _coreDb = DBOperatorBase.GetDBOperatorObject(DBOperatorType.OleDBOperator,
                string.Format(ConfigurationManager.ConnectionStrings[CONST_APP_CORE_CONNECTION_KEY].ConnectionString, _currentAssemblyLocation + @"\..\..\..\IRONMAN.UnitTest\database\"));
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
            SessionManager.Clear();
            SessionManager.MemberID = 1;
            SessionManager.MemberCode = "POC";
            SessionManager.IsAdmin = false;
            // Clean up all tables
            _stockDb.ExecuteCommand.CommandText = "DELETE FROM StockPriceHistory";
            _stockDb.Execute();
            _stockDb.ExecuteCommand.CommandText = "DELETE FROM Stock";
            _stockDb.Execute();

            // TODO: Investigate why need to reinitialzie database object. It could be a serious bug.
            _coreDb = DBOperatorBase.GetDBOperatorObject(DBOperatorType.OleDBOperator,
                string.Format(ConfigurationManager.ConnectionStrings[CONST_APP_CORE_CONNECTION_KEY].ConnectionString, _currentAssemblyLocation + @"\..\..\..\IRONMAN.UnitTest\database\"));
            _coreDb.BeginTrans();
            try
            {
                _coreDb.ExecuteCommand.CommandText = "DELETE FROM Arm2ArmRelation";
                _coreDb.Execute();
                _coreDb.ExecuteCommand.CommandText = "DELETE FROM Arm2Filter";
                _coreDb.Execute();
                _coreDb.ExecuteCommand.CommandText = "DELETE FROM Arm";
                _coreDb.Execute();
                _coreDb.ExecuteCommand.CommandText = "DELETE FROM Robot";
                _coreDb.Execute();
                _coreDb.ExecuteCommand.CommandText = "DELETE FROM Filter2Selector";
                _coreDb.Execute();
                _coreDb.ExecuteCommand.CommandText = "DELETE FROM Selector";
                _coreDb.Execute();
                _coreDb.ExecuteCommand.CommandText = "DELETE FROM FilterRelation";
                _coreDb.Execute();
                _coreDb.ExecuteCommand.CommandText = "DELETE FROM Filter";
                _coreDb.Execute();
                _coreDb.ExecuteCommand.CommandText = "INSERT INTO Filter(FilterName, Description, Creator, IsPublic, SystemFullName) VALUES('所有股票', '列出集中市場所有股票，是最基本的資料來源。', 0, True, 'IRONMAN.Core.Filters.AllStockSetFilter')";
                _coreDb.Execute();
                _coreDb.ExecuteCommand.CommandText = "INSERT INTO Filter(FilterName, Description, Creator, IsPublic, SystemFullName) VALUES('已持有之股票', '列出目前手上所持有的股票清單。', 0, True, 'IRONMAN.Core.Filters.OwnStockSetFilter')";
                _coreDb.Execute();
                _coreDb.CommitTrans();
            }
            catch
            {
                _coreDb.RollBack();
                throw;
            }
            finally
            {
                _coreDb.Close();
            }

            // TODO: Investigate why need to reinitialzie database object. It could be a serious bug.
            _transDb = DBOperatorBase.GetDBOperatorObject(DBOperatorType.OleDBOperator,
                string.Format(ConfigurationManager.ConnectionStrings[CONST_APP_TRANS_CONNECTION_KEY].ConnectionString, _currentAssemblyLocation + @"\..\..\..\IRONMAN.UnitTest\database\"));
            _transDb.BeginTrans();
            try
            {
                _transDb.ExecuteCommand.CommandText = "DELETE FROM Traders";
                _transDb.Execute();
                _transDb.ExecuteCommand.CommandText = "DELETE FROM Transactions";
                _transDb.Execute();
                _transDb.ExecuteCommand.CommandText = "DELETE FROM RepoStock";
                _transDb.Execute();
                _transDb.ExecuteCommand.CommandText = "DELETE FROM Repositories";
                _transDb.Execute();
                _transDb.ExecuteCommand.CommandText = "DELETE FROM SuggestionReport";
                _transDb.Execute();
                _transDb.ExecuteCommand.CommandText = "DELETE FROM StockPrice";
                _transDb.Execute();
                _transDb.ExecuteCommand.CommandText = "DELETE FROM Stock";
                _transDb.Execute();
                _transDb.CommitTrans();
            }
            catch
            {
                _transDb.RollBack();
                throw;
            }
            finally
            {
                _transDb.Close();
            }

            TimeMachine.Reset();
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
