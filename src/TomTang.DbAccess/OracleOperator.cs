using System;
using System.Data;
using System.Data.OracleClient;

namespace TomTang.DbAccess {
	/// <summary>
	/// OracleOperator ªººK­n´y­z¡C
	/// </summary>
	public class OracleOperator : DBOperatorBase {
		internal OracleOperator(string ConnectionString) {
            connection = new OracleConnection(ConnectionString);
			CreateCommands();
		}

		protected override IDbDataAdapter GetDBAdapter() {
			return new OracleDataAdapter();
		}

		public override void Fill(DataTable dt) {
			OracleDataAdapter oAdapter = (OracleDataAdapter)GetPreparedDbAdapter();
			oAdapter.Fill(dt);
		}

		public override void Update(DataTable dt) {
			OracleDataAdapter oAdapter = (OracleDataAdapter)GetPreparedDbAdapter();
			oAdapter.Update(dt);
		}
	}
}
