using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using web.dal;
using web.model;

namespace web.bll
{
    /// <summary>
    /// oracle数据库服务类
    /// </summary>
    public class OracleBll
    {
        /// <summary>
        /// 测试oracle数据库连接
        /// </summary>
        /// <param name="postParam"></param>
        /// <returns></returns>
        public static bool TestConnect(PostParamModel postParam)
        {
            //获取连接字符串
            string connectionString = OracleDal.GetConnectionString(postParam);
            //获取连接
            OracleConnection connection = OracleDal.GetConnect(connectionString);
            //查询sql
            string sql = "select count(1) from dual";
            connection.Open();
            int count = Convert.ToInt32(OracleDal.GetSingle(connection, sql));
            connection.Close();
            return count > 0;
        }

        /// <summary>
        /// 获取oracle所有表名
        /// </summary>
        /// <param name="postParam"></param>
        /// <returns></returns>
        public static List<TableModel> GetTables(PostParamModel postParam)
        {
            //获取连接字符串
            string connectionString = OracleDal.GetConnectionString(postParam);
            //获取连接
            OracleConnection connection = OracleDal.GetConnect(connectionString);
            //查询sql
            StringBuilder sql = new StringBuilder("SELECT ");
            sql.Append("t.table_name AS \"TableName\",f.comments AS \"TableComment\" ");
            sql.Append("FROM user_tables t ");
            sql.Append("INNER JOIN user_tab_comments f ON t.table_name = f.table_name ");
            connection.Open();
            List<TableModel> list = OracleDal.GetQueryData<TableModel>(connection, sql.ToString(), null, typeof(TableModel));
            connection.Close();
            return list;
        }

        /// <summary>
        /// 获得oracle数据库的所有表结构
        /// </summary>
        /// <param name="postParam"></param>
        /// <returns></returns>
        public static List<TableModel> Generation(PostParamModel postParam)
        {
            //获取连接字符串
            string connectionString = OracleDal.GetConnectionString(postParam);
            //获取连接
            OracleConnection connection = OracleDal.GetConnect(connectionString);
            //查询sql
            StringBuilder sql = new StringBuilder("SELECT ");
            sql.Append("t.table_name AS \"TableName\",f.comments AS \"TableComment\" ");
            sql.Append("FROM user_tables t ");
            sql.Append("INNER JOIN user_tab_comments f ON t.table_name = f.table_name ");
            sql.Append("WHERE t.table_name IN (" + postParam.Tb + ")");
            OracleParameter[] tabParams = { new OracleParameter("@table_schema", postParam.Dbname) };
            connection.Open();
            List<TableModel> tabList = OracleDal.GetQueryData<TableModel>(connection, sql.ToString(), tabParams, typeof(TableModel));
            //表的描述
            StringBuilder colField = new StringBuilder();
            colField.Append("col.column_name AS \"ColumnName\",");
            colField.Append("col.data_type AS \"ColumnType\",");
            colField.Append("col.data_default AS \"ColumnDefault\",");
            colField.Append("col.nullable AS \"IsNullable\",");
            colField.Append("cns.constraint_type AS \"ColumnKey\",");
            colField.Append("ucc.comments AS \"ColumnComment\" ");
            //表的连接
            StringBuilder joinTab = new StringBuilder();
            joinTab.Append("LEFT JOIN user_col_comments ucc ON ucc.table_name=col.table_name ");
            joinTab.Append("AND ucc.column_name=col.column_name ");
            joinTab.Append("LEFT JOIN user_cons_columns ccs ON ccs.table_name=col.table_name ");
            joinTab.Append("AND ccs.column_name=col.column_name AND ccs.position=col.column_id ");
            joinTab.Append("LEFT JOIN user_constraints cns ON col.table_name=cns.table_name ");
            joinTab.Append("AND cns.constraint_type='P' AND ccs.constraint_name=cns.constraint_name ");
            //表名
            string tab = " user_tab_columns col ";
            string order = " ORDER BY col.column_id ASC ";
            for (int i = 0; i < tabList.Count; i++)
            {
                TableModel table = tabList[i];
                //查询sql
                string sqlCol = string.Format("SELECT {0} FROM {1} {2} WHERE col.table_name='{3}' {4}", colField.ToString(), tab, joinTab.ToString(), table.TableName, order);
                List<ColumnModel> colList = OracleDal.GetQueryData<ColumnModel>(connection, sqlCol, null, typeof(ColumnModel));
                table.Colums = colList;
            }
            connection.Close();
            return tabList;
        }
    }
}