using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using log4net;
using log4net.Config;

namespace TomTang.DbAccess {
	/// <summary>
	/// DBServiceBase is a common DB service base class for inheritance.
    /// For singleton, it's not thread-safe within singleton
	/// </summary>
	public abstract class DBServiceBase : MarshalByRefObject, IDisposable {
        private readonly ILog _logger = null;
        protected readonly DBOperatorBase op = null;
		/// <summary>
		/// Constructor for DBServiceBase
		/// </summary>
		/// <param name="ConnStr">Connection string</param>
		/// <param name="DBType">DB type</param>
		protected DBServiceBase(string ConnStr, DBOperatorType DBType) {
			op = DBOperatorBase.GetDBOperatorObject(DBType, ConnStr);

            FileInfo fi = new FileInfo("log4net.config");
            if (fi.Exists) XmlConfigurator.ConfigureAndWatch(fi); else Trace.WriteLine(string.Format("{0} is not existing, use itself config to instead of.", fi.FullName));
            _logger = LogManager.GetLogger(this.GetType());
        }
        /// <summary>
        /// Logging facility with log4net
        /// </summary>
        protected ILog Logger { get { return _logger; } }
		/// <summary>
		/// Execute serveral sql statements stored in array list in a transaction control.
		/// </summary>
		/// <param name="oA">Array list which stores sql string</param>
		/// <returns>Affected rows count</returns>
		protected int ExecuteNonQuery(IList<string> commandTextList) {
			int effectedCount = 0;
			op.BeginTrans();
			try {
                foreach (var text in commandTextList)
                {
                    op.ExecuteCommand.CommandText = text;
                    effectedCount += op.Execute();
				}
				op.CommitTrans();
			}
			catch {
				op.RollBack();
				throw;
			}
			finally {
				op.Close();
			}
            return effectedCount;
		}
		/// <summary>
		/// Execute single sql statement in a transaction control.
		/// </summary>
		/// <param name="sqlStr">sql statements</param>
		/// <returns>Affected rows count</returns>
		protected int ExecuteNonQuery(string sqlStr) {
			int num1 = 0;
			op.ExecuteCommand.CommandText = sqlStr;
			op.BeginTrans();
			try {
				num1 = op.Execute();
				op.CommitTrans();
			}
			catch {
				op.RollBack();
				throw;
			}
			finally {
				op.Close();
			}
			return num1;
		}
		/// <summary>
		/// Execute a sql statement which returns a single value
		/// </summary>
		/// <param name="sqlStr">Sql statement</param>
		/// <returns>Return value</returns>
		protected object ExecuteScalar(string sqlStr) {
			op.ExecuteCommand.CommandText = sqlStr;
			object obj1 = op.ExecuteScalar();
			op.Close();
			return obj1;
		}
		/// <summary>
		/// Fill data table with sql select statement
		/// </summary>
		/// <param name="sqlStr">Sql select statement</param>
		/// <param name="dt">Data table object which you want to fill</param>
		protected void FillDt(string sqlStr, DataTable dt) {
			op.SelectCommand.CommandText = sqlStr;
			op.Fill(dt);
		}
        /// <summary>
        /// Easy to customized the exception handle procedure.
        /// </summary>
        /// <param name="Ex">Unhandled exception</param>
        /// <remarks>Now it just write error log simply and rethrow out.</remarks>
        protected virtual void ExceptionHandler(Exception Ex)
        {
            _logger.Error(Ex.Message, Ex);
            throw Ex;
        }
        /// <summary>
        /// Log SPI invocation in DEBUG level.
        /// </summary>
        /// <returns>SPI name</returns>
        protected virtual string SpiLogging()
        {
            int i = 1;
            StackFrame frame = new StackFrame(i, false);
            while ("SpiLogging" == frame.GetMethod().Name ||
                "DoFirst" == frame.GetMethod().Name)
            {
                frame = new StackFrame(++i, false);
            }
            string spiName = frame.GetMethod().Name;
            _logger.Debug(string.Format("[SPI]={0}", spiName));
            return spiName;
        }

        #region IDisposable Members

        public void Dispose()
        {
            op.Dispose();
        }

        #endregion
    }
}
