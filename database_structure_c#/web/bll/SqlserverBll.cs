using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using web.model;

namespace web.bll
{
    /// <summary>
    /// sqlserver数据库服务类
    /// </summary>
    public class SqlserverBll
    {
        /// <summary>
        /// 测试sqlserver数据库连接
        /// </summary>
        /// <param name="postParam">请求参数体</param>
        /// <returns></returns>
        public static bool TestConnect(PostParamModel postParam)
        {
            //获取连接字符串
            string connectionString = SqlserverDal.GetConnectionString(postParam);
            //获取连接
            SqlConnection connection = SqlserverDal.GetConnect(connectionString);
            //查询sql
            string sql = "select count(1)";
            connection.Open();
            int count = (int)SqlserverDal.GetSingle(connection, sql);
            connection.Close();
            return count > 0;
        }

        /// <summary>
        /// 获取sqlserver所有表名
        /// </summary>
        /// <param name="postParam">请求参数体</param>
        /// <returns></returns>
        public static List<TableModel> GetTables(PostParamModel postParam)
        {
            //获取连接字符串
            string connectionString = SqlserverDal.GetConnectionString(postParam);
            //获取连接
            SqlConnection connection = SqlserverDal.GetConnect(connectionString);
            //查询sql
            StringBuilder sql = new StringBuilder("SELECT ");
            sql.Append("a.name AS TableName,");
            sql.Append("CONVERT(NVARCHAR(100),isnull(g.[value],'-')) AS TableComment ");
            sql.Append("FROM sys.tables a ");
            sql.Append("LEFT JOIN sys.extended_properties g ON (a.object_id = g.major_id AND g.minor_id = 0) ");
            connection.Open();
            List<TableModel> list = SqlserverDal.GetQueryData<TableModel>(connection, sql.ToString(), null, typeof(TableModel));
            connection.Close();
            return list;
        }

        /// <summary>
        /// 获得sqlserver数据库的所有表结构
        /// </summary>
        /// <param name="postParam">请求参数体</param>
        /// <returns></returns>
        public static List<TableModel> Generation(PostParamModel postParam)
        {
            //获取连接字符串
            string connectionString = SqlserverDal.GetConnectionString(postParam);
            //获取连接
            SqlConnection connection = SqlserverDal.GetConnect(connectionString);
            //查询sql
            StringBuilder sql = new StringBuilder("SELECT ");
            sql.Append("a.name AS TableName,");
            sql.Append("CONVERT(NVARCHAR(100),isnull(g.[value],'-')) AS TableComment ");
            sql.Append("FROM sys.tables a ");
            sql.Append("LEFT JOIN sys.extended_properties g ON (a.object_id = g.major_id AND g.minor_id = 0) ");
            sql.Append("WHERE a.name IN (" + postParam.Tb + ")");
            connection.Open();
            List<TableModel> tabList = SqlserverDal.GetQueryData<TableModel>(connection, sql.ToString(), null, typeof(TableModel));
            //循环赋值colList
            for (int i = 0; i < tabList.Count; i++)
            {
                TableModel table = tabList[i];
                //查询sql
                StringBuilder colSql = new StringBuilder("SELECT ");
                colSql.Append("a.name AS ColumnName,");
                colSql.Append("COLUMNPROPERTY(a.id,a.name,'IsIdentity') AS 'Extra',");
                colSql.Append("case when exists(");
                colSql.Append("SELECT xtype FROM sysobjects WHERE xtype='PK'  AND name IN(");
                colSql.Append("SELECT name FROM sysindexes WHERE indid IN(");
                colSql.Append("SELECT indid FROM sysindexkeys WHERE id = a.id AND colid=a.colid");
                colSql.Append("))) then 'true' else '' end AS 'ColumnKey',");
                colSql.Append("b.name AS ColumnType,");
                colSql.Append("a.isnullable AS IsNullable,");
                colSql.Append("isnull(e.text,'') AS ColumnDefault,");
                colSql.Append("convert(varchar,isnull(g.[value],'')) AS ColumnComment ");
                colSql.Append("FROM syscolumns a ");
                colSql.Append("LEFT JOIN systypes b ON a.xusertype=b.xusertype ");
                colSql.Append("INNER JOIN sysobjects d ON a.id=d.id ");
                colSql.Append("LEFT JOIN syscomments e ON a.cdefault=e.id ");
                colSql.Append("LEFT JOIN sys.extended_properties g ON a.id=g.major_id AND a.colid=g.minor_id ");
                colSql.Append("WHERE d.name=@name ");
                colSql.Append("ORDER BY a.id,a.colorder");
                //赋值
                SqlParameter[] cmdParams = { new SqlParameter("@name", table.TableName) };
                List<ColumnModel> colList = SqlserverDal.GetQueryData<ColumnModel>(connection, colSql.ToString(), cmdParams, typeof(ColumnModel));
                table.Colums = colList;
            }
            connection.Close();
            return tabList;
        }
    }
}