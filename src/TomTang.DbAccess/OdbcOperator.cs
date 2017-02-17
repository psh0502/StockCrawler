using System;
using System.Data;
using System.Data.Odbc;

namespace TomTang.DbAccess
{
    /// <summary>
    /// OdbcOperator ªººK­n´y­z¡C
    /// </summary>
    public class OdbcOperator : DBOperatorBase
    {
        internal OdbcOperator(string ConnectionString)
        {
            connection = new OdbcConnection(ConnectionString);
            CreateCommands();
        }

        protected override IDbDataAdapter GetDBAdapter()
        {
            return new OdbcDataAdapter();
        }

        public override void Fill(DataTable dt)
        {
            OdbcDataAdapter oAdapter = (OdbcDataAdapter)GetPreparedDbAdapter();
            oAdapter.Fill(dt);
        }

        public override void Update(DataTable dt)
        {
            OdbcDataAdapter oAdapter = (OdbcDataAdapter)GetPreparedDbAdapter();
            oAdapter.Update(dt);
        }
    }
}