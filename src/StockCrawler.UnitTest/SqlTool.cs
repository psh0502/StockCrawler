using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace StockCrawler.UnitTest
{
    internal class SqlTool
    {
        public static string ConnectionString { get; set; }
        public static void ExecuteSql(string sql)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        public static void ExecuteSqlFile(string fileName)
        {
            string sql = null;
            FileInfo file = new FileInfo(fileName);
            if (!file.Exists) throw new FileNotFoundException(file.FullName + "找不到!");
            using (var sr = new StreamReader(file.FullName))
                sql = sr.ReadToEnd();

            sql = sql.Replace("GO\r\n", string.Empty);
            if (sql.EndsWith("GO")) sql = sql.Substring(0, sql.Length - 2);

            ExecuteSql(sql);
        }

        public static DataTable QuerySql(string sql)
        {
            DataTable table = new DataTable();
            using (var conn = new SqlConnection(ConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.Fill(table);
            }

            return table;
        }
    }
}
