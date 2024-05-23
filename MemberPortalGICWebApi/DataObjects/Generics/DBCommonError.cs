using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace MemberPortalGICWebApi.DataObjects.Generics
{
    public class DBCommonError
    {
        private static Common instance = null;
        private static string key1 = "alert";

        private readonly string _connectionString;
        public DBCommonError(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DBCommonError()
        {
            // TODO: Complete member initialization
        }
        public bool SessionChk()
        {
            return HttpContext.Current.Session.Keys.Count > 0 ? true : false;
        }




        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        }

        public void SaveXML(Logs Errologs)
        {
            lock (this)
            {

                var path = String.Format("{0}ErrorXmls\\{1}.xml", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("yyyyMMddHHmmssffff"));

                XmlSerializer xs = new XmlSerializer(typeof(Logs));
                TextWriter tw = new StreamWriter(path);
                xs.Serialize(tw, Errologs);

            }

        }

        private OracleConnection GetConnection()
        {
            return new OracleConnection(GetConnectionString());
        }
        public DataTable GetDataTable(string query)
        {
            OracleConnection connection = GetConnection();
            // var connection = new OracleConnection(_connectionString);
            DataTable dt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(query, connection);
            adapter.Fill(dt);
            CloseConnection(connection);
            return dt;
        }

        private void CloseConnection(OracleConnection con)
        {
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public DataRow GetDataRow(string query)
        {
            OracleConnection connection = GetConnection();
            DataTable dt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(query, connection);
            adapter.Fill(dt);
            CloseConnection(connection);
            return dt.Rows[0];
        }

        public OracleDataReader GetDataReader(string query)
        {
            OracleConnection connection = GetConnection();

            connection.Open();
            OracleDataReader dr = null;
            OracleCommand cmd = new OracleCommand(query, connection);
            dr = cmd.ExecuteReader();
            // connection.Close();
            return dr;
        }
        public OracleDataReader GetDataReader(OracleCommand cmd)
        {
            OracleConnection connection = GetConnection();
            connection.Open();
            OracleDataReader dr = null;
            cmd.Connection = connection;
            dr = cmd.ExecuteReader();
            // connection.Close();
            return dr;
        }

        public int UpdateRecord(string query)
        {
            OracleConnection connection = GetConnection();
            connection.Open();
            OracleCommand cmd = new OracleCommand(query, connection);
            cmd.CommandType = CommandType.Text;
            int count = cmd.ExecuteNonQuery();
            CloseConnection(connection);
            return count;
        }

        public int InsertRecord(OracleCommand cmd)
        {
            try
            {
                OracleConnection connection = GetConnection();
                connection.Open();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                int count = cmd.ExecuteNonQuery();
                CloseConnection(connection);
                return count;
            }
            catch (Exception ex)
            {
                Logs Errorlogs = new Logs();
                Errorlogs.ErorDesc = "Error During DB Record insertion || " + cmd.CommandText;
                Errorlogs.ErrorCode = "ErroRInsertRecordLine142Common";
                Errorlogs.ErrorExp = ex.Message.ToString() + "  ||  " + ex.StackTrace.ToString();
                Errorlogs.F1 = "E";
                SaveXML(Errorlogs);
                return 0;
            }
        }

        public object GetScalarValue(string query)
        {
            OracleConnection connection = GetConnection();
            connection.Open();
            OracleCommand cmd = new OracleCommand(query, connection);
            cmd.CommandType = CommandType.Text;
            object result = cmd.ExecuteOracleScalar().ToString();
            CloseConnection(connection);
            return result;
        }


        public bool InsertResponseLogs(string authNo, string incdNo, string status, string key, int count)
        {
            string query = "INSERT INTO RESPONSE_LOG (AUTH_NO, INCD_NO, CREATED_DATE, STATUS, KEY, COUNT) VALUES (:AUTH_NO ,:INCD_NO ,sysdate, :STATUS, :KEY , :COUNT)";
            OracleCommand cmd = new OracleCommand();
            cmd.Parameters.Add(":AUTH_NO", authNo);
            cmd.Parameters.Add(":INCD_NO", incdNo);
            cmd.Parameters.Add(":STATUS", status);
            cmd.Parameters.Add(":KEY", key);
            cmd.Parameters.Add(":COUNT", count);
            cmd.CommandText = query;
            int recpds = InsertRecord(cmd);
            if (recpds == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool InsertErrorLogs(ErrorLogs_DB logs)
        {
            //logs.ErrorExp = logs.ErrorExp.Replace("'", "''");
            //logs.ErrorCode = logs.ErrorCode.Replace("'", "''");
            //logs.ErorDesc = logs.ErorDesc.Replace("'", "''");
            string query = "INSERT INTO errorlogs_memberportal ( ECODE, EDESC,CREATIONDATE, EXPEC,TYPE_E) VALUES (:Ecode,:Edesc ,sysdate ,:Expec, :typeE)";
            OracleCommand cmd = new OracleCommand();
            cmd.Parameters.Add(":Ecode", logs.ErrorCode);
            cmd.Parameters.Add(":Edesc", logs.ErorDesc);
            cmd.Parameters.Add(":Expec", logs.ErrorExp);
            cmd.Parameters.Add(":typeE", logs.TypeError);
            cmd.CommandText = query;
            int recpds = InsertRecord(cmd);
            if (recpds == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public bool InsertSucessLogs(Logs logs)
        {
            logs.ErrorExp = logs.ErrorExp.Replace("'", "''");
            logs.ErrorCode = logs.ErrorCode.Replace("'", "''");
            logs.ErorDesc = logs.ErorDesc.Replace("'", "''");
            string query = "INSERT INTO ERRORLOGS ( ECODE, EDESC,  EDT, EXPEC,F1) VALUES (:ECODESds ,:EDESCVsd ,sysdate ,:ESCPECTs, 'S'   )";
            OracleCommand cmd = new OracleCommand();
            cmd.Parameters.Add(":ECODESds", logs.ErrorCode);
            cmd.Parameters.Add(":EDESCVsd", logs.ErorDesc);
            cmd.Parameters.Add(":ESCPECTs", logs.ErrorExp);
            cmd.CommandText = query;
            int recpds = InsertRecord(cmd);
            if (recpds == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool InsertRecordWithQuery(string query)
        {

            if (query != null)
            {
                OracleCommand cmd = new OracleCommand();
                cmd.CommandText = query;
                int recpds = InsertRecord(cmd);
                if (recpds == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }



        public OracleDataReader GetDataReader2(OracleCommand cmd, ref OracleConnection connection)
        {
            connection.Open();
            OracleDataReader dr = null;
            dr = cmd.ExecuteReader();
            return dr;
        }
    }
}