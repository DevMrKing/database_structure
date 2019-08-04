using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using web.dal;
using web.model;

namespace web.bll
{
    /// <summary>
    /// mysql数据库服务类
    /// </summary>
    public class MySqlBll
    {
        /// <summary>
        /// 测试mysql数据库连接
        /// </summary>
        /// <param name="postParam"></param>
        /// <returns></returns>
        public static bool TestConnect(PostParamModel postParam)
        {
            //获取连接字符串
            string connectionString = MySqlDal.GetConnectionString(postParam);
            //获取连接
            MySqlConnection connection = MySqlDal.GetConnect(connectionString);
            //查询sql
            string sql = "select count(1)";
            connection.Open();
            int count = Convert.ToInt32(MySqlDal.GetSingle(connection, sql));
            connection.Close();
            return count > 0;
        }

        /// <summary>
        /// 获取mysql所有表名
        /// </summary>
        /// <param name="postParam"></param>
        /// <returns></returns>
        public static List<TableModel> GetTables(PostParamModel postParam)
        {
            //获取连接字符串
            string connectionString = MySqlDal.GetConnectionString(postParam);
            //获取连接
            MySqlConnection connection = MySqlDal.GetConnect(connectionString);
            //查询sql
            StringBuilder sql = new StringBuilder("SELECT ");
            sql.Append("table_name AS TableName,table_comment AS TableComment ");
            sql.Append("FROM information_schema.tables WHERE table_schema=@table_schema ");
            connection.Open();
            MySqlParameter[] cmdParams = { new MySqlParameter("@table_schema", postParam.Dbname) };
            List<TableModel> list = MySqlDal.GetQueryData<TableModel>(connection, sql.ToString(), cmdParams, typeof(TableModel));
            connection.Close();
            return list;
        }

        /// <summary>
        /// 获得mysql数据库的所有表结构
        /// </summary>
        /// <param name="postParam"></param>
        /// <returns></returns>
        public static List<TableModel> Generation(PostParamModel postParam)
        {
            //获取连接字符串
            string connectionString = MySqlDal.GetConnectionString(postParam);
            //获取连接
            MySqlConnection connection = MySqlDal.GetConnect(connectionString);
            //查询sql
            StringBuilder sql = new StringBuilder("SELECT ");
            sql.Append("table_name AS TableName,table_comment AS TableComment ");
            sql.Append("FROM information_schema.tables WHERE table_schema=@table_schema ");
            sql.Append("AND table_name IN (" + postParam.Tb + ")");
            MySqlParameter[] tabParams = { new MySqlParameter("@table_schema", postParam.Dbname) };
            connection.Open();
            List<TableModel> tabList = MySqlDal.GetQueryData<TableModel>(connection, sql.ToString(), tabParams, typeof(TableModel));
            //循环赋值colList
            StringBuilder colField = new StringBuilder();
            colField.Append("column_name AS ColumnName,");
            colField.Append("column_type AS ColumnType,");
            colField.Append("column_default AS ColumnDefault,");
            colField.Append("is_nullable AS IsNullable,");
            colField.Append("extra AS Extra,");
            colField.Append("column_key AS ColumnKey,");
            colField.Append("column_comment AS ColumnComment");
            //表名
            string colTab = "information_schema.columns";
            for (int i = 0; i < tabList.Count; i++)
            {
                TableModel table = tabList[i];
                //查询sql
                string colSql = string.Format("SELECT {0} FROM {1} WHERE table_name=@table_name", colField.ToString(), colTab);
                //赋值
                MySqlParameter[] cmdParams = { new MySqlParameter("@table_name", table.TableName) };
                List<ColumnModel> colList = MySqlDal.GetQueryData<ColumnModel>(connection, colSql, cmdParams, typeof(ColumnModel));
                table.Colums = colList;
            }
            connection.Close();
            return tabList;
        }
    }
}