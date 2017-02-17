using System;

namespace TomTang.DbAccess
{
    /// <summary>
    /// OleDbStringHelper ªººK­n´y­z¡C
    /// </summary>
    public class OleDbStrHelper
    {
        public static string getParamStr(bool oValue)
        {
            return (oValue) ? "true" : "false";
        }

        public static string getParamStr(DateTime oValue)
        {
            if (oValue == DateTime.MinValue) return "null";
            return ("#" + oValue.ToString("yyyy/MM/dd HH:mm:ss") + "#");
        }

        public static string getParamStr(decimal oValue)
        {
            return oValue.ToString();
        }

        public static string getParamStr(short oValue)
        {
            return oValue.ToString();
        }

        public static string getParamStr(int oValue)
        {
            return oValue.ToString();
        }

        public static string getParamStr(string oValue)
        {
            if (oValue == null) return "null";
            return ("'" + oValue.Replace("'", "''") + "'");
        }

        public static string getParamStr(byte[] oValue)
        {
            if (oValue == null || 0 == oValue.Length) return "null";
            return "0x" + HexEncoding.ToString(oValue);
        }

        public static string getParamStrLike(string oValue)
        {
            if (oValue == null) return "null";
            return ("'%" + oValue.Replace("'", "''") + "%'");
        }

        public const string pC = "@";
        public const string pComma = ",";
    }
}
