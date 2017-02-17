using System;
using System.Data;
using System.Data.Common;
using System.Reflection;
using TomTang.DbAccess;

namespace TomTang.DbAccess
{
    /// <summary>
    /// DBOperatorBase ªººK­n´y­z¡C
    /// </summary>
    public abstract class DBOperatorBase : IDBOperator, IDisposable
    {
        protected abstract IDbDataAdapter GetDBAdapter();

        protected IDbDataAdapter GetPreparedDbAdapter()
        {
            IDbDataAdapter oDBApter = GetDBAdapter();
            oDBApter.InsertCommand = InsertCommand;
            oDBApter.SelectCommand = SelectCommand;
            oDBApter.DeleteCommand = DeleteCommand;
            oDBApter.UpdateCommand = UpdateCommand;
            return oDBApter;
        }

        protected void CreateCommands()
        {
            SelectCommand = connection.CreateCommand();
            InsertCommand = connection.CreateCommand();
            UpdateCommand = connection.CreateCommand();
            DeleteCommand = connection.CreateCommand();
            ExecuteCommand = connection.CreateCommand();
        }

        public static DBOperatorBase GetDBOperatorObject(DBOperatorType DBType, string ConnectionString)
        {
            DBOperatorBase oOperator;
            switch (DBType)
            {
                case DBOperatorType.MSSqlOperator:
                    oOperator = new MSSqlOperator(ConnectionString);
                    break;
                case DBOperatorType.OleDBOperator:
                    oOperator = new OleDBOperator(ConnectionString);
                    break;
                case DBOperatorType.OracleOperator:
                    oOperator = new OracleOperator(ConnectionString);
                    break;
                case DBOperatorType.OdbcOperator:
                    oOperator = new OdbcOperator(ConnectionString);
                    break;
                default:
                    throw new ArgumentException("Unknown DBOperator specified!!", "DBType");
            }
            return oOperator;
        }

        #region IDBOperator Members

        public virtual void BeginTrans()
        {
            Open();
            transaction = connection.BeginTransaction();
            SelectCommand.Transaction = transaction;
            InsertCommand.Transaction = transaction;
            UpdateCommand.Transaction = transaction;
            DeleteCommand.Transaction = transaction;
            ExecuteCommand.Transaction = transaction;
            InTransaction = true;
        }

        public virtual void Close()
        {
            if (InTransaction) RollBack();
            connection.Close();
        }

        public virtual void CommitTrans()
        {
            if (InTransaction) transaction.Commit();
            InTransaction = false;
        }

        public virtual int Execute()
        {
            Open();
            return ExecuteCommand.ExecuteNonQuery();
        }

        public virtual object ExecuteScalar()
        {
            Open();
            return ExecuteCommand.ExecuteScalar();
        }

        public virtual void Fill(DataSet ds)
        {
            IDbDataAdapter oDBApter = GetPreparedDbAdapter();
            oDBApter.Fill(ds);
        }

        public abstract void Fill(DataTable dt);

        public abstract void Update(DataTable dt);

        public virtual string FillInXML()
        {
            DataSet set1 = new DataSet();
            Fill(set1);
            return XmlDbHelper.DataSet2Xml(set1);
        }

        public virtual void Open()
        {
            if (State != ConnectionState.Open) connection.Open();
        }

        public virtual void RollBack()
        {
            if (InTransaction) transaction.Rollback();
            InTransaction = false;
        }

        public virtual void Update(DataSet ds)
        {
            IDbDataAdapter oDBApter = GetPreparedDbAdapter();
            oDBApter.Update(ds);
        }

        public virtual void Update(string Xml)
        {
            DataSet oDs = XmlDbHelper.Xml2DataSet(Xml);
            Update(oDs);
        }

        /// <summary>
        /// Compact Database
        /// </summary>
        /// <param name="sErrMsg">Error message if failed</param>
        /// <returns>success or fail</returns>
        public virtual bool Compact(out string sErrMsg)
        {
            sErrMsg = "Not support for this operator!";
            return false;
        }

        #region Commands
        public virtual IDbCommand SelectCommand { get; private set; }

        public virtual IDbCommand InsertCommand { get; private set; }

        public virtual IDbCommand UpdateCommand { get; private set; }

        public virtual IDbCommand DeleteCommand { get; private set; }

        public virtual IDbCommand ExecuteCommand { get; private set; }
        #endregion

        public virtual bool InTransaction { get; private set; }

        public virtual ConnectionState State
        {
            get { return connection.State; }
        }

        protected DbTransaction transaction { get; set; }

        protected DbConnection connection { get; set; }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                if (null != ExecuteCommand) ExecuteCommand.Dispose();
                if (null != SelectCommand) SelectCommand.Dispose();
                if (null != DeleteCommand) DeleteCommand.Dispose();
                if (null != UpdateCommand) UpdateCommand.Dispose();
                if (null != transaction) transaction.Dispose();
                if (null != connection) connection.Dispose();
            }
            catch (Exception ex)
            {
                DebugInfo.WriteLine(ex.ToString());
            }
        }

        #endregion
    }
}