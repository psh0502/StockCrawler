using System;

namespace TomTang.DbAccess {
	/// <summary>
	/// Indicate what kind of DB driver you want to use.
	/// </summary>
	public enum DBOperatorType {
		OleDBOperator,
		OdbcOperator,
		MSSqlOperator,
		OracleOperator
	}
}
