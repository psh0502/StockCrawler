using System;
using System.Data;

namespace TomTang.DbAccess {
	/// <summary>
	/// IDBOperator ªººK­n´y­z¡C
	/// </summary>
	internal interface IDBOperator {
		void BeginTrans();
		void Close();
		void CommitTrans();
		int Execute();
		object ExecuteScalar();
		void Fill(DataSet ds);
		void Fill(DataTable dt);
		string FillInXML();
		void Open();
		void RollBack();
		void Update(DataSet ds);
		void Update(DataTable dt);
		void Update(string strXML);
		/// <summary>
		/// Compact Database
		/// </summary>
		/// <param name="sErrMsg">Error message if failed</param>
		/// <returns>success or fail</returns>
		bool Compact(out string sErrMsg);
		#region Commands
		IDbCommand SelectCommand {
			get;
		}

		IDbCommand InsertCommand {
			get;
		}
		
		IDbCommand UpdateCommand {
			get;
		}
		
		IDbCommand DeleteCommand {
			get;
		}

		IDbCommand ExecuteCommand {
			get;
		}
		#endregion

		bool InTransaction {
			get;
		}

		ConnectionState State {
			get;
		}
	}
}
