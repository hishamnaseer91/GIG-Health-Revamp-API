
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi
{
    public class DBGenerics
    {
        CommandType CommandType = CommandType.Text;
        string ConnectionString = DBHelper.ConnectionString;
        int CommandTimeout = 30;

        public static readonly DbProviderFactory Factory = DbProviderFactories.GetFactory("System.Data.OracleClient");

        #region retun integer,long
        //scaller for insert and non query for update
        public int ExecuteScalarInt32(string query, params DbParameter[] prms)
        {
            int id = -1;

            object o = ExecuteScalar(query, prms);

            if (o != null && o != DBNull.Value)
            {
                id = int.Parse(o.ToString());
            }

            return id;
        }

        public int ExecuteScalarInt32Sp(string query, params DbParameter[] prms)
        {
            int id = -1;

            object o = ExecuteScalar(query, CommandType.StoredProcedure, prms);

            if (o != null && o != DBNull.Value)
            {
                id = int.Parse(o.ToString());
            }

            return id;
        }

        public int ExecuteScalarInt32(string query)
        {
            int id = -1;

            object o = ExecuteScalar(query, null);

            if (o != null && o != DBNull.Value)
            {
                id = int.Parse(o.ToString());
            }

            return id;
        }


        public decimal ExecuteScalarDecimal(string query)
        {
            decimal id = Decimal.MinValue;

            object o = ExecuteScalar(query, null);

            if (o != null && o != DBNull.Value)
            {
                id = decimal.Parse(o.ToString());
            }

            return id;
        }
        public string ExecuteScalarString(string query)
        {
            string id = "-1";

            object o = ExecuteScalar(query, null);

            if (o != null && o != DBNull.Value)
            {
                id = o.ToString();
            }

            return id;
        }

        public string ExecuteScalarNvarchar(string query, params DbParameter[] prms)
        {
            string id = "-1";

            object o = ExecuteScalar(query, prms);

            if (o != null && o != DBNull.Value)
            {
                id = o.ToString();
            }

            return id;
        }


        public string ExecuteScalarNvarcharwithSP(string query, params DbParameter[] prms)
        {
            string id = "-1";

            object o = ExecuteScalarwithSP(query, prms);

            if (o != null && o != DBNull.Value)
            {
                id = o.ToString();
            }

            return id;
        }
        public object ExecuteScalarwithSP(string sprocName, params DbParameter[] prms)
        {
            object o = null;

            try
            {
                using (DbConnection connection = Factory.CreateConnection())
                {
                    connection.ConnectionString = ConnectionString;

                    using (DbCommand command = Factory.CreateCommand())
                    {
                        command.CommandTimeout = CommandTimeout;
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = sprocName;

                        if (prms != null)
                        {
                            command.Parameters.AddRange(prms);
                        }

                        connection.Open();

                        o = command.ExecuteScalar();

                        connection.Close();


                    }
                }
            }
            finally
            {

            }

            return o;
        }

        public int ExecuteNonQuery(string query, params DbParameter[] prms)
        {
            int effRows = -1;

            try
            {
                using (DbConnection connection = Factory.CreateConnection())
                {
                    connection.ConnectionString = ConnectionString;

                    using (DbCommand command = Factory.CreateCommand())
                    {
                        command.CommandTimeout = CommandTimeout;
                        command.Connection = connection;
                        command.CommandType = CommandType;
                        command.CommandText = query;

                        if (prms != null)
                        {
                            command.Parameters.AddRange(prms);
                        }

                        connection.Open();
                        effRows = command.ExecuteNonQuery();//no of effected rows
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {

            }

            return effRows;
        }

        public int ExecuteNonQueryOracle(string query, string Parametervalue, params DbParameter[] prms)
        {
            int effRows = -1;

            try
            {
                using (DbConnection connection = Factory.CreateConnection())
                {
                    connection.ConnectionString = ConnectionString;

                    using (DbCommand command = Factory.CreateCommand())
                    {
                        command.CommandTimeout = CommandTimeout;
                        command.Connection = connection;
                        command.CommandType = CommandType;
                        command.CommandText = query;

                        if (prms != null)
                        {
                            command.Parameters.AddRange(prms);
                        }

                        connection.Open();
                        effRows = command.ExecuteNonQuery();//no of effected rows
                        effRows =Convert.ToInt32( command.Parameters[Parametervalue].Value.ToString());
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {

            }

            return effRows;
        }
        public long ExecuteScalarInt64(string query, params DbParameter[] prms)
        {
            long id = -1;

            object o = ExecuteScalar(query, prms);

            if (o != null && o != DBNull.Value)
            {
                id = long.Parse(o.ToString());
            }
            //first column of first rown for insert
            return id;
        }

        public long ExecuteScalarInt64(string query)
        {
            long id = -1;

            object o = ExecuteScalar(query, null);

            if (o != null && o != DBNull.Value)
            {
                id = long.Parse(o.ToString());
            }

            return id;
        }

        #endregion

        #region return List ds,dt and obj

        public DataTable ExecuteDataTable(string query, params DbParameter[] prms)
        {
            DataTable dtResult = new DataTable();
            try
            {
                using (DbConnection connection = Factory.CreateConnection())
                {
                    connection.ConnectionString = ConnectionString;

                    using (DbCommand command = Factory.CreateCommand())
                    {
                        command.Parameters.Clear();
                        command.CommandTimeout = CommandTimeout;
                        command.Connection = connection;
                        command.CommandType = CommandType;
                        command.CommandText = query;

                        if (prms != null)
                        {
                            command.Parameters.AddRange(prms);
                        }

                        //connection.Open();
                        DbDataAdapter adapter = Factory.CreateDataAdapter();
                        adapter.SelectCommand = command;

                        // Fill the DataTable.
                        DataTable table = new DataTable();
                        adapter.Fill(dtResult);

                        //connection.Close();


                    }
                }
            }
            catch (Exception exp)
            { }
            finally
            {

            }

            return dtResult;
        }

        public DataSet ExecuteDataSet(string query, params DbParameter[] prms)
        {
            DataSet dsResult = new DataSet();
            try
            {
                using (DbConnection connection = Factory.CreateConnection())
                {
                    connection.ConnectionString = ConnectionString;

                    using (DbCommand command = Factory.CreateCommand())
                    {
                        command.CommandTimeout = CommandTimeout;
                        command.Connection = connection;
                        command.CommandType = CommandType;
                        command.CommandText = query;

                        if (prms != null)
                        {
                            command.Parameters.AddRange(prms);
                        }

                        //connection.Open();
                        DbDataAdapter adapter = Factory.CreateDataAdapter();
                        adapter.SelectCommand = command;

                        // Fill the DataTable.

                        adapter.Fill(dsResult);

                        //connection.Close();
                    }
                }
            }
            finally
            {

            }

            return dsResult;
        }

        public object ExecuteScalar(string query, params DbParameter[] prms)
        {
            object o = null;

            try
            {
                using (DbConnection connection = Factory.CreateConnection())
                {
                    connection.ConnectionString = ConnectionString;

                    using (DbCommand command = Factory.CreateCommand())
                    {
                        command.CommandTimeout = CommandTimeout;
                        command.Connection = connection;
                        command.CommandType = CommandType;
                        command.CommandText = query;

                        if (prms != null)
                        {
                            command.Parameters.AddRange(prms);
                        }

                        connection.Open();

                        o = command.ExecuteScalar();

                        connection.Close();


                    }
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {

            }

            return o;
        }

        public object ExecuteScalar(string query, CommandType commandType, params DbParameter[] prms)
        {
            object o = null;

            try
            {
                using (DbConnection connection = Factory.CreateConnection())
                {
                    connection.ConnectionString = ConnectionString;

                    using (DbCommand command = Factory.CreateCommand())
                    {
                        command.CommandTimeout = CommandTimeout;
                        command.Connection = connection;
                        command.CommandType = commandType;
                        command.CommandText = query;

                        if (prms != null)
                        {
                            command.Parameters.AddRange(prms);
                        }
                        //command.Parameters.Add("@status");
                        command.Parameters["@Result"].Direction = ParameterDirection.Output;
                        connection.Open();

                        o = command.ExecuteScalar();
                        o = Convert.ToInt32(command.Parameters["@Result"].Value);
                        connection.Close();


                    }
                }
            }
            finally
            {

            }

            return o;
        }

        #endregion

        #region return generic model

        public T ExecuteSingle<T>(string query) where T : new()
        {
            return ExecuteSingle<T>(query, null);
        }

        public T ExecuteSingle<T>(string query, params DbParameter[] prms) where T : new()
        {
            IList<T> list = ExecuteList<T>(query, prms);

            if (list.Count > 0)
                return list[0];

            T obj = default(T);

            return obj;
        }

        #endregion

        #region return list

        public IList<T> ExecuteList<T>(string query) where T : new()
        {
            return ExecuteList<T>(query, null);
        }

        public IList<T> ExecuteList<T>(string query, params DbParameter[] prms) where T : new()
        {
            IList<T> objectList = new List<T>();
            T obj = default(T);

            try
            {
                using (DbConnection connection = Factory.CreateConnection())
                {
                    connection.ConnectionString = ConnectionString;

                    using (DbCommand command = Factory.CreateCommand())
                    {
                        command.CommandTimeout = CommandTimeout;
                        command.Connection = connection;
                        command.CommandType = CommandType;
                        command.CommandText = query;

                        if (prms != null)
                        {
                            command.Parameters.AddRange(prms);
                        }

                        connection.Open();

                        using (DbDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    obj = new T();

                                    IDataMapper mapper = obj as IDataMapper;
                                    mapper.MapProperties(dataReader);

                                    objectList.Add(obj);
                                }
                            }

                            dataReader.Close();
                        }
                    }
                }
            }
            finally
            {

            }

            return objectList;
        }

        public IList<T> ExecuteListSp<T>(string query, CommandType Type, params DbParameter[] prms) where T : new()
        {
            IList<T> objectList = new List<T>();
            T obj = default(T);

            try
            {
                using (DbConnection connection = Factory.CreateConnection())
                {
                    connection.ConnectionString = ConnectionString;

                    using (DbCommand command = Factory.CreateCommand())
                    {
                        command.CommandTimeout = CommandTimeout;
                        command.Connection = connection;
                        command.CommandType = Type;
                        command.CommandText = query;

                        if (prms != null)
                        {
                            command.Parameters.AddRange(prms);
                        }

                        connection.Open();

                        using (DbDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    obj = new T();

                                    IDataMapper mapper = obj as IDataMapper;
                                    mapper.MapProperties(dataReader);

                                    objectList.Add(obj);
                                }
                            }

                            dataReader.Close();
                        }
                    }
                }
            }
            finally
            {

            }

            return objectList;
        }

        public IList<T> ExecuteLists<T>(string query, params DbParameter[] prms) where T : new()
        {
            IList<T> objectList = new List<T>();
            T obj = default(T);

            try
            {
                using (DbConnection connection = Factory.CreateConnection())
                {
                    connection.ConnectionString = ConnectionString;

                    using (DbCommand command = Factory.CreateCommand())
                    {
                        command.CommandTimeout = CommandTimeout;
                        command.Connection = connection;
                        command.CommandType = CommandType;
                        command.CommandText = query;

                        if (prms != null)
                        {
                            command.Parameters.AddRange(prms);
                        }

                        connection.Open();

                        using (DbDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    obj = new T();
                                    objectList.Add(obj);
                                }
                            }

                            dataReader.Close();
                        }
                    }
                }
            }
            finally
            {

            }

            return objectList;
        }

        public IList<string> ExecuteStringList(string query, string resultColumnName, params DbParameter[] prms)
        {

            IList<string> objectList = new List<string>();

            try
            {
                using (DbConnection connection = Factory.CreateConnection())
                {
                    connection.ConnectionString = ConnectionString;

                    using (DbCommand command = Factory.CreateCommand())
                    {
                        command.CommandTimeout = CommandTimeout;
                        command.Connection = connection;
                        command.CommandType = CommandType;
                        command.CommandText = query;

                        if (prms != null)
                        {
                            command.Parameters.AddRange(prms);
                        }

                        connection.Open();

                        using (DbDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    string obj = dataReader.GetString(resultColumnName);
                                    objectList.Add(obj);
                                }
                            }

                            dataReader.Close();
                        }
                    }
                }
            }
            finally
            {

            }

            return objectList;
        }

        public IList<int> ExecuteInt32List(string query, string resultColumnName, params DbParameter[] prms)
        {


            //int recordCount = 0;

            IList<int> objectList = new List<int>();



            try
            {


                using (DbConnection connection = Factory.CreateConnection())
                {


                    connection.ConnectionString = ConnectionString;

                    using (DbCommand command = Factory.CreateCommand())
                    {


                        command.CommandTimeout = CommandTimeout;

                        command.Connection = connection;

                        command.CommandType = CommandType;

                        command.CommandText = query;

                        if (prms != null)
                        {
                            command.Parameters.AddRange(prms);
                        }

                        connection.Open();



                        using (DbDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {


                            if (dataReader.HasRows)
                            {


                                while (dataReader.Read())
                                {

                                    int obj = dataReader.GetInt32(resultColumnName);

                                    objectList.Add(obj);
                                }
                            }

                            dataReader.Close();


                        }
                    }
                }
            }
            finally
            {

            }

            return objectList;
        }

        #endregion

        private IParamBuilder _paramBuilder;

        protected IParamBuilder ParamBuilder
        {
            get
            {
                return _paramBuilder ?? new OracleParameterbuilder();
            }
        }
    }
}