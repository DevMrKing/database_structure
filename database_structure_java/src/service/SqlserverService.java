package service;

import java.sql.*;
import java.util.*;

import db.*;
import vo.*;

/**
 * sqlserver数据库服务类
 */
public class SqlserverService {
	/**
	 * 测试sqlserver数据库连接
	 * @param postParam
	 * @throws SQLException 
	 */
	public static boolean testConnect(PostParamVo postParam) throws Exception{
		Connection connection=SqlserverDb.getConnection(postParam);
		String sql="select count(1)";
		long ret=(long)SqlserverDb.executeQueryOneColumn(sql, null, connection);
		connection.close();
		return ret>0;
	}
	
	/**
	 * 获取sqlserver所有表名
	 * @param postParam
	 * @return
	 * @throws Exception 
	 */
	public static List<TableVo> getTables(PostParamVo postParam)throws Exception{
		Connection connection=SqlserverDb.getConnection(postParam);
		StringBuffer sql=new StringBuffer("SELECT ");
		sql.append("a.name AS tableName,");
		sql.append("CONVERT(NVARCHAR(100),");
		sql.append("isnull(g.[value],'-')) AS tableComment ");
		sql.append("FROM sys.tables a ");
		sql.append("LEFT JOIN sys.extended_properties g ON (a.object_id = g.major_id AND g.minor_id = 0) ");
		List<TableVo> list=SqlserverDb.executeQuery(sql.toString(), null, connection,TableVo.class);
		connection.close();
		return list;
	}
	
	/**
	 * 获得sqlserver数据库的所有表结构
	 * @param postParam
	 * @return
	 * @throws Exception 
	 */
	public static List<TableVo> generation(PostParamVo postParam) throws Exception{
		Connection connection=SqlserverDb.getConnection(postParam);
		StringBuffer sql=new StringBuffer("SELECT ");
		sql.append("a.name AS tableName,");
		sql.append("CONVERT(NVARCHAR(100),isnull(g.[value],'-')) AS tableComment ");
		sql.append("FROM sys.tables a ");
		sql.append("LEFT JOIN sys.extended_properties g ON (a.object_id = g.major_id AND g.minor_id = 0) ");
		sql.append("WHERE a.name IN ("+postParam.getTb()+")");
		List<TableVo> tabList=SqlserverDb.executeQuery(sql.toString(), null, connection,TableVo.class);
		//表的描述
		for(int i=0;i<tabList.size();i++){
			//查询sql
			StringBuffer colSql=new StringBuffer("SELECT ");
			colSql.append("a.name AS columnName,");
			colSql.append("COLUMNPROPERTY(a.id,a.name,'IsIdentity') AS 'extra',");
			colSql.append("case when exists(");
			colSql.append("SELECT xtype FROM sysobjects WHERE xtype='PK'  AND name IN(");
			colSql.append("SELECT name FROM sysindexes WHERE indid IN(");
			colSql.append("SELECT indid FROM sysindexkeys WHERE id = a.id AND colid=a.colid");
			colSql.append("))) then 'true' else '' end AS 'columnKey',");
			colSql.append("b.name AS columnType,");
			colSql.append("a.isnullable AS isNullable,");
			colSql.append("isnull(e.text,'') AS columnDefault,");
			colSql.append("convert(varchar,isnull(g.[value],'')) AS columnComment ");
			colSql.append("FROM syscolumns a ");
			colSql.append("LEFT JOIN systypes b ON a.xusertype=b.xusertype ");
			colSql.append("INNER JOIN sysobjects d ON a.id=d.id ");
			colSql.append("LEFT JOIN syscomments e ON a.cdefault=e.id ");
			colSql.append("LEFT JOIN sys.extended_properties g ON a.id=g.major_id AND a.colid=g.minor_id ");
			colSql.append("WHERE d.name=? ");
			colSql.append("ORDER BY a.id,a.colorder");
			//获取table对象
			TableVo table=tabList.get(i);
			String[] parameters=new String[]{table.getTableName()};
			List<ColumnVo> colList=SqlserverDb.executeQuery(colSql.toString(), parameters, connection,ColumnVo.class);
			table.setColums(colList);
		}
		connection.close();
		return tabList;
	}
}
