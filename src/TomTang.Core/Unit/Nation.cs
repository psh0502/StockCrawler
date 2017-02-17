using System;

namespace TomTang.Unit
{
	/// <summary>
	/// Nation ªººK­n´y­z¡C
	/// </summary>
	public class Nation
	{
		private const string _CONST_DefaultNationId = "zh-tw";
		private readonly string _nationId;
		public Nation(string NationID)
		{
			_nationId = NationID;
		}
		public Nation()
		{
			_nationId = _CONST_DefaultNationId;
		}
	}
}
