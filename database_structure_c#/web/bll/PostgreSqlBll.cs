using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using web.dal;
using web.model;

namespace web.bll
{
    /// <summary>
    /// pgsql数据库服务类
    /// </summary>
    public class PostgreSqlBll
    {
        /// <summary>
        /// 测试pgsql数据库连接
        /// </summary>
        /// <param name="postParam"></param>
        /// <returns></returns>
        public static bool TestConnect(PostParamModel postParam)
        {
            //获取连接字符串
            string connectionString = PostgreSqlDal.GetConnectionString(postParam);
            //获取连接
            NpgsqlConnection connection = PostgreSqlDal.GetConnect(connectionString);
            //查询sql
            string sql = "select count(1)";
            connection.Open();
            int count = Convert.ToInt32(PostgreSqlDal.GetSingle(connection, sql));
            connection.Close();
            return count > 0;
        }

        /// <summary>
        /// 获取pgsql所有表名
        /// </summary>
        /// <param name="postParam"></param>
        /// <returns></returns>
        public static List<TableModel> GetTables(PostParamModel postParam)
        {
            //获取连接字符串
            string connectionString = PostgreSqlDal.GetConnectionString(postParam);
            //获取连接
            NpgsqlConnection connection = PostgreSqlDal.GetConnect(connectionString);
            //查询sql
            StringBuilder sql = new StringBuilder("SELECT ");
            sql.Append("tb.tablename AS \"TableName\",");
            sql.Append("cast(obj_description(c.relfilenode,'pg_class') AS varchar) AS \"TableComment\" ");
            sql.Append("FROM pg_tables tb ");
            sql.Append("LEFT JOIN pg_class c ON tb.tablename=relname ");
            sql.Append("WHERE schemaname = 'public'");
            connection.Open();
            List<TableModel> list = PostgreSqlDal.GetQueryData<TableModel>(connection, sql.ToString(), null, typeof(TableModel));
            connection.Close();
            return list;
        }

        /// <summary>
        /// 获得pgsql数据库的所有表结构
        /// </summary>
        /// <param name="postParam"></param>
        /// <returns></returns>
        public static List<TableModel> Generation(PostParamModel postParam)
        {
            //获取连接字符串
            string connectionString = PostgreSqlDal.GetConnectionString(postParam);
            //获取连接
            NpgsqlConnection connection = PostgreSqlDal.GetConnect(connectionString);
            //查询sql
            StringBuilder sql = new StringBuilder("SELECT ");
            sql.Append("tb.tablename AS \"TableName\",");
            sql.Append("cast(obj_description(c.relfilenode,'pg_class') AS varchar) AS \"TableComment\" ");
            sql.Append("FROM pg_tables tb ");
            sql.Append("LEFT JOIN pg_class c ON tb.tablename=relname ");
            sql.Append("WHERE schemaname = 'public' AND tb.tablename IN (" + postParam.Tb + ")");
            connection.Open();
            List<TableModel> tabList = PostgreSqlDal.GetQueryData<TableModel>(connection, sql.ToString(), null, typeof(TableModel));
            for (int i = 0; i < tabList.Count; i++)
            {
                TableModel table = tabList[i];
                //查询sql
                StringBuilder colSql = new StringBuilder("SELECT DISTINCT ");
                colSql.Append("a.attnum as num,");
                colSql.Append("a.attname as \"ColumnName\",");
                colSql.Append("format_type(a.atttypid, a.atttypmod) as \"ColumnType\",");
                colSql.Append("a.attnotnull as \"IsNullable\",");
                colSql.Append("com.description as \"ColumnComment\",");
                colSql.Append("coalesce(i.indisprimary,false) as \"ColumnKey\",");
                colSql.Append("def.adsrc as \"ColumnDefault\" ");
                colSql.Append("FROM pg_attribute a ");
                colSql.Append("JOIN pg_class pgc ON pgc.oid = a.attrelid ");
                colSql.Append("LEFT JOIN pg_index i ON (pgc.oid = i.indrelid AND i.indkey[0] = a.attnum) ");
                colSql.Append("LEFT JOIN pg_description com ON (pgc.oid = com.objoid AND a.attnum = com.objsubid) ");
                colSql.Append("LEFT JOIN pg_attrdef def ON (a.attrelid = def.adrelid AND a.attnum = def.adnum) ");
                colSql.Append("WHERE a.attnum > 0 AND pgc.oid = a.attrelid ");
                colSql.Append("AND pg_table_is_visible(pgc.oid) ");
                colSql.Append("AND NOT a.attisdropped ");
                colSql.Append("AND pgc.relname = @table_name ");
                colSql.Append("ORDER BY a.attnum");
                //赋值
                NpgsqlParameter[] cmdParams = { new NpgsqlParameter("@table_name", table.TableName) };
                List<ColumnModel> colList = PostgreSqlDal.GetQueryData<ColumnModel>(connection, colSql.ToString(), cmdParams, typeof(ColumnModel));
                table.Colums = colList;
            }
            connection.Close();
            return tabList;
        }
    }
}