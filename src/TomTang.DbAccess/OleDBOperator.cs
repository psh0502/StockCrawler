using JRO;
using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Threading;

namespace TomTang.DbAccess
{
    public class OleDBOperator : DBOperatorBase
    {
        private const string CONST_JET_PROVIDER_STR = "microsoft.jet.oledb.";
        internal OleDBOperator(string ConnectionString)
        {
            connection = new OleDbConnection(ConnectionString);
            CreateCommands();
        }

        public override void Fill(DataTable dt)
        {
            OleDbDataAdapter oAdapter = (OleDbDataAdapter)GetPreparedDbAdapter();
            oAdapter.Fill(dt);
        }

        public override void Update(DataTable dt)
        {
            OleDbDataAdapter oAdapter = (OleDbDataAdapter)GetPreparedDbAdapter();
            oAdapter.Update(dt);
        }

        protected override IDbDataAdapter GetDBAdapter()
        {
            return new OleDbDataAdapter();
        }

        /// <summary>
        /// Compact Database
        /// </summary>
        /// <param name="sErrMsg">Error message if failed</param>
        /// <returns>success or fail</returns>
        public override bool Compact(out string sErrMsg)
        {
            bool bRtnValue = false;
            string sDriver = string.Empty;
            FileInfo oDBFile = null;
            FileInfo oDestDBFile = null;
            string[] sConnStrFlds = connection.ConnectionString.Split(';');
            string sDestConnStr = string.Empty;
            const int nRetryCount = 5;
            foreach (string sFld in sConnStrFlds)
            {
                string[] tmps = sFld.Split('=');
                if (2 != tmps.Length) continue;	// Go for next one.

                #region Identify which oleDb driver it using
                if ("provider" == tmps[0].Trim().ToLower())
                {
                    if (-1 != tmps[1].Trim().ToLower().IndexOf(CONST_JET_PROVIDER_STR))
                    {
                        sDriver = CONST_JET_PROVIDER_STR;
                    }
                    else
                    {
                        sDriver = tmps[1].Trim();
                    }
                }
                #endregion

                #region Identify data source
                if ("data source" == tmps[0].Trim().ToLower())
                {
                    oDBFile = new FileInfo(tmps[1].Replace("'", ""));
                    oDestDBFile = new FileInfo(oDBFile.DirectoryName + "\\" + Guid.NewGuid().ToString() + ".mdb");
                    sDestConnStr += "Data Source='" + oDestDBFile.FullName + "';";
                    continue;
                }
                #endregion

                sDestConnStr += sFld + ";";
            }
            DebugInfo.WriteLine("DestConnStr = " + sDestConnStr);
            try
            {
                switch (sDriver)
                {
                    case CONST_JET_PROVIDER_STR:
                        JetEngine oJRO = new JetEngineClass();
                        int nCount = 0;
                        #region Compact process
                        while (true)
                        {
                            try
                            {
                                oJRO.CompactDatabase(connection.ConnectionString, sDestConnStr);
                                break;
                            }
                            catch (Exception oEX)
                            {
                                DebugInfo.WriteLine("Compacting failed with reason \"" + oEX.Message + "\" and retry counter = " + nCount + ".");
                                oDestDBFile.Delete();
                                nCount++;
                                if (nRetryCount == nCount)
                                {
                                    throw;
                                }
                                Thread.Sleep(10);
                                continue;
                            }
                        }
                        #endregion

                        nCount = 0;

                        #region Complete replacing
                        while (true)
                        {
                            try
                            {
                                if (oDestDBFile.Exists)
                                {
                                    oDBFile.Delete();
                                    oDestDBFile.MoveTo(oDBFile.FullName);
                                    sErrMsg = string.Empty;
                                    bRtnValue = true;
                                }
                                else
                                {
                                    sErrMsg = "The temp DB file[" + oDestDBFile.FullName + "] is not existed!";
                                }
                                break;
                            }
                            catch (Exception oEX)
                            {
                                DebugInfo.WriteLine("Replacing failed with reason \"" + oEX.Message + "\" and retry counter = " + nCount + ".");
                                nCount++;
                                if (nRetryCount == nCount)
                                {
                                    oDestDBFile.Delete();
                                    throw;
                                }
                                Thread.Sleep(10);
                                continue;
                            }
                        }
                        #endregion

                        break;
                    default:
                        sErrMsg = "This driver[" + sDriver + "] is not supported!";
                        bRtnValue = false;
                        break;
                }
            }
            catch (Exception oEX)
            {
                sErrMsg = oEX.Message;
            }
            return bRtnValue;
        }
    }
}