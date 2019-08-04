using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using web.model;
using Npgsql;

namespace web.dal
{
    /// <summary>
    /// pgsql数据库帮助类
    /// </summary>
    public class PostgreSqlDal
    {
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string GetConnectionString(PostParamModel model)
        {
            return string.Format("Server={0};Port={1};User Id={2};Password={3};Database={4}", model.Host, model.Port, model.User, model.Pass, model.Dbname);
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns>数据库连接</returns>
        public static NpgsqlConnection GetConnect(string connectionString)
        {
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            return connection;
        }

        /// <summary>
        /// 单值查询
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="sql">查询sql</param>
        /// <returns>查询结果</returns>
        public static object GetSingle(NpgsqlConnection connection, string sql)
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
        public static object GetSingle(NpgsqlConnection connection, string sql, NpgsqlCommand[] cmdParams)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                try
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sql;
                    if (cmdParams != null && cmdParams.Length > 0)
                    {
                        cmd.Parameters.AddRange(cmdParams);
                    }
                    return cmd.ExecuteScalar();
                }
                catch (Exception e)
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
        public static DataSet Query(NpgsqlConnection connection, string sql)
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
        public static DataSet Query(NpgsqlConnection connection, string sql, NpgsqlParameter[] cmdParams)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                using (NpgsqlDataAdapter ada = new NpgsqlDataAdapter(cmd))
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
                    catch (Exception e)
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
        public static DataTable GetQueryData(NpgsqlConnection connection, string sql)
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
        public static DataTable GetQueryData(NpgsqlConnection connection, string sql, NpgsqlParameter[] cmdParams)
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
        public static List<T> GetQueryData<T>(NpgsqlConnection connection, string sql, NpgsqlParameter[] cmdParams, Type classType)
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
        public static DataRow GetQueryRecord(NpgsqlConnection connection, string sql)
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
        public static DataRow GetQueryRecord(NpgsqlConnection connection, string sql, NpgsqlParameter[] cmdParams)
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
        public static T GetQueryRecord<T>(NpgsqlConnection connection, string sql, NpgsqlParameter[] cmdParams, Type classType)
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