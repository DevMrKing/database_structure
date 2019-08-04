package service;

import java.sql.*;
import java.util.*;

import db.*;
import vo.*;

/**
 * pgsql数据库服务类
 */
public class PostgresqlService {
	
	/**
	 * 测试pgsql数据库连接
	 * @param postParam
	 * @throws SQLException 
	 */
	public static boolean testConnect(PostParamVo postParam) throws Exception{
		Connection connection=PostgreSqlDb.getConnection(postParam);
		String sql="select count(1)";
		long ret=(long)PostgreSqlDb.executeQueryOneColumn(sql, null, connection);
		connection.close();
		return ret>0;
	}
	
	/**
	 * 获取pgsql所有表名
	 * @param postParam
	 * @return
	 * @throws Exception 
	 */
	public static List<TableVo> getTables(PostParamVo postParam)throws Exception{
		Connection connection=PostgreSqlDb.getConnection(postParam);
		StringBuffer sql=new StringBuffer("SELECT ");
		sql.append("tb.tablename AS \"tableName\",");
		sql.append("cast(obj_description(c.relfilenode,'pg_class') AS varchar) AS \"tableComment\" ");
		sql.append("FROM pg_tables tb ");
		sql.append("LEFT JOIN pg_class c ON tb.tablename=relname ");
		sql.append("WHERE schemaname = 'public'");
		List<TableVo> list=PostgreSqlDb.executeQuery(sql.toString(), null, connection,TableVo.class);
		connection.close();
		return list;
	}
	
	/**
	 * 获得pgsql数据库的所有表结构
	 * @param postParam
	 * @return
	 * @throws Exception 
	 */
	public static List<TableVo> generation(PostParamVo postParam) throws Exception{
		Connection connection=PostgreSqlDb.getConnection(postParam);
		StringBuffer sql=new StringBuffer("SELECT ");
		sql.append("tb.tablename AS \"tableName\",");
		sql.append("cast(obj_description(c.relfilenode,'pg_class') AS varchar) AS \"tableComment\" ");
		sql.append("FROM pg_tables tb ");
		sql.append("LEFT JOIN pg_class c ON tb.tablename=relname ");
		sql.append("WHERE schemaname = 'public' AND tb.tablename IN ("+postParam.getTb()+")");
		List<TableVo> tabList=PostgreSqlDb.executeQuery(sql.toString(), null, connection,TableVo.class);
		//表的描述
		for(int i=0;i<tabList.size();i++){
			//查询sql
			StringBuffer colSql=new StringBuffer("SELECT DISTINCT ");
			colSql.append("a.attnum as num,");
			colSql.append("a.attname as \"columnName\",");
			colSql.append("format_type(a.atttypid, a.atttypmod) as \"columnType\",");
			colSql.append("a.attnotnull as \"isNullable\",");
			colSql.append("com.description as \"columnComment\",");
			colSql.append("coalesce(i.indisprimary,false) as \"columnKey\",");
			colSql.append("def.adsrc as \"columnDefault\" ");
			colSql.append("FROM pg_attribute a ");
			colSql.append("JOIN pg_class pgc ON pgc.oid = a.attrelid ");
			colSql.append("LEFT JOIN pg_index i ON (pgc.oid = i.indrelid AND i.indkey[0] = a.attnum) ");
			colSql.append("LEFT JOIN pg_description com ON (pgc.oid = com.objoid AND a.attnum = com.objsubid) ");
			colSql.append("LEFT JOIN pg_attrdef def ON (a.attrelid = def.adrelid AND a.attnum = def.adnum) ");
			colSql.append("WHERE a.attnum > 0 AND pgc.oid = a.attrelid ");
			colSql.append("AND pg_table_is_visible(pgc.oid) ");
			colSql.append("AND NOT a.attisdropped ");
			colSql.append("AND pgc.relname = ? ");
			colSql.append("ORDER BY a.attnum");
			//获取table对象
			TableVo table=tabList.get(i);
			String[] parameters=new String[]{table.getTableName()};
			List<ColumnVo> colList=PostgreSqlDb.executeQuery(colSql.toString(), parameters, connection,ColumnVo.class);
			table.setColums(colList);
		}
		connection.close();
		return tabList;
	}
}
