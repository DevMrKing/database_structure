using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using web.model;

namespace web.bll
{
    /// <summary>
    /// sqlserver数据库操作基本类
    /// </summary>
    public class SqlserverDal
    {
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string GetConnectionString(PostParamModel model)
        {
            return string.Format("Data Source={0},{1};Initial Catalog={2};User Id={3};Password={4}", model.Host, model.Port, model.Dbname, model.User, model.Pass);
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns>数据库连接</returns>
        public static SqlConnection GetConnect(string connectionString)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }

        /// <summary>
        /// 单值查询
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="sql">查询sql</param>
        /// <returns>查询结果</returns>
        public static object GetSingle(SqlConnection connection, string sql)
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
        public static object GetSingle(SqlConnection connection, string sql, SqlParameter[] cmdParams)
        {
            using (SqlCommand cmd = new SqlCommand())
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
        public static DataSet Query(SqlConnection connection, string sql)
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
        public static DataSet Query(SqlConnection connection, string sql, SqlParameter[] cmdParams)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter(cmd))
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
        public static DataTable GetQueryData(SqlConnection connection, string sql)
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
        public static DataTable GetQueryData(SqlConnection connection, string sql, SqlParameter[] cmdParams)
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
        public static List<T> GetQueryData<T>(SqlConnection connection, string sql, SqlParameter[] cmdParams, Type classType)
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
        public static DataRow GetQueryRecord(SqlConnection connection, string sql)
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
        public static DataRow GetQueryRecord(SqlConnection connection, string sql, SqlParameter[] cmdParams)
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
        public static T GetQueryRecord<T>(SqlConnection connection, string sql, SqlParameter[] cmdParams, Type classType)
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