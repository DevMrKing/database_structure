using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Oracle.ManagedDataAccess.Client;
using web.model;

namespace web.dal
{
    /// <summary>
    /// oracle数据库帮助类
    /// </summary>
    public class OracleDal
    {
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string GetConnectionString(PostParamModel model)
        {
            return string.Format("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1}))(CONNECT_DATA=(SERVICE_NAME={2})));Persist Security Info=True;User ID={3};Password={4};", model.Host, model.Port, model.Dbname, model.User, model.Pass);
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns>数据库连接</returns>
        public static OracleConnection GetConnect(string connectionString)
        {
            OracleConnection connection = new OracleConnection(connectionString);
            return connection;
        }

        /// <summary>
        /// 单值查询
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="sql">查询sql</param>
        /// <returns>查询结果</returns>
        public static object GetSingle(OracleConnection connection, string sql)
        {
            return GetSingle(connection, sql, null);
        }

        /// <summary>
        /// 单值查询
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="sql">查询sql</param>
        /// <param name="cmdParams">查询参数</param>
        /// <returns>查询结果</returns>
        public static object GetSingle(OracleConnection connection, string sql, OracleCommand[] cmdParams)
        {
            using (OracleCommand cmd = new OracleCommand())
            {
                try
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    if (cmdParams != null && cmdParams.Length > 0)
                    {
                        cmd.Parameters.AddRange(cmdParams);
                    }
                    return cmd.ExecuteScalar();
                }
                catch (SqlException e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// 返回结果集
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="sql">查询sql</param>
        /// <returns></returns>
        public static DataSet Query(OracleConnection connection, string sql)
        {
            return Query(connection, sql, null);
        }

        /// <summary>
        /// 查询数据返回结果集
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="sql">查询sql</param>
        /// <param name="cmdParams">查询参数</param>
        /// <returns></returns>
        public static DataSet Query(OracleConnection connection, string sql, OracleParameter[] cmdParams)
        {
            using (OracleCommand cmd = new OracleCommand())
            {
                using (OracleDataAdapter ada = new OracleDataAdapter(cmd))
                {
                    try
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = sql;
                        if (cmdParams != null && cmdParams.Length > 0)
                        {
                            cmd.Parameters.AddRange(cmdParams);
                        }
                        DataSet ds = new DataSet();
                        ada.Fill(ds);
                        return ds;
                    }
                    catch (SqlException e)
                    {
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 单表查询
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="sql">查询sql</param>
        /// <returns></returns>
        public static DataTable GetQueryData(OracleConnection connection, string sql)
        {
            DataSet ds = Query(connection, sql);
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }

        /// <summary>
        /// 单表查询
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="sql">查询sql</param>
        /// <param name="cmdParams">查询参数</param>
        /// <returns></returns>
        public static DataTable GetQueryData(OracleConnection connection, string sql, OracleParameter[] cmdParams)
        {
            DataSet ds = Query(connection, sql, cmdParams);
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }

        /// <summary>
        /// 单表查询生成实体类
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="sql">查询sql</param>
        /// <param name="cmdParams">查询参数</param>
        /// <param name="classType">类的类型</param>
        /// <returns></returns>
        public static List<T> GetQueryData<T>(OracleConnection connection, string sql, OracleParameter[] cmdParams, Type classType)
        {
            DataTable table = GetQueryData(connection, sql, cmdParams);
            List<T> list = new List<T>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                T model = Activator.CreateInstance<T>();
                DataRow dataRow = table.Rows[i];
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    object data = dataRow[j];
                    if (data == null)
                    {
                        continue;
                    }
                    PropertyInfo pi = classType.GetProperty(table.Columns[j].ColumnName);
                    if (pi != null)
                    {
                        pi.SetValue(model, Convert.ChangeType(data, pi.PropertyType), null);
                    }
                }
                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 查询一行数据
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="sql">查询sql</param>
        /// <returns></returns>
        public static DataRow GetQueryRecord(OracleConnection connection, string sql)
        {
            DataTable dt = GetQueryData(connection, sql);
            if (dt != null && dt.Rows.Count > 0)
                return dt.Rows[0];
            return null;
        }

        /// <summary>
        /// 查询一行数据
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="sql">查询sql</param>
        /// <param name="cmdParams">查询参数</param>
        /// <returns></returns>
        public static DataRow GetQueryRecord(OracleConnection connection, string sql, OracleParameter[] cmdParams)
        {
            DataTable dt = GetQueryData(connection, sql, cmdParams);
            if (dt != null && dt.Rows.Count > 0)
                return dt.Rows[0];
            return null;
        }

        /// <summary>
        /// 查询一行数据生成一个对象
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="sql">查询sql</param>
        /// <param name="cmdParams">查询参数</param>
        /// <param name="classType">类的类型</param>
        /// <returns></returns>
        public static T GetQueryRecord<T>(OracleConnection connection, string sql, OracleParameter[] cmdParams, Type classType)
        {
            T model = Activator.CreateInstance<T>();
            DataRow row = GetQueryRecord(connection, sql, cmdParams);
            DataColumnCollection cols = row.Table.Columns;
            for (int j = 0; j < cols.Count; j++)
            {
                object data = row[j];
                if (data == null)
                {
                    continue;
                }
                PropertyInfo pi = classType.GetProperty(cols[j].ColumnName);
                if (pi != null)
                {
                    pi.SetValue(model, Convert.ChangeType(data, pi.PropertyType), null);
                }
            }
            return model;
        }
    }
}