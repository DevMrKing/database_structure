package service;

import java.sql.*;
import java.util.*;

import db.*;
import vo.*;

/**
 * mysql数据库服务类
 */
public class MySqlService{
	
	/**
	 * 测试mysql数据库连接
	 * @param postParam
	 * @throws SQLException 
	 */
	public static boolean testConnect(PostParamVo postParam) throws Exception{
		Connection connection=MySqlDb.getConnection(postParam);
		String sql="select count(1)";
		long ret=(long)MySqlDb.executeQueryOneColumn(sql, null, connection);
		connection.close();
		return ret>0;
	}
	
	/**
	 * 获取mysql所有表名
	 * @param postParam
	 * @return
	 * @throws Exception 
	 */
	public static List<TableVo> getTables(PostParamVo postParam)throws Exception{
		Connection connection=MySqlDb.getConnection(postParam);
		StringBuffer sql=new StringBuffer("SELECT ");
		sql.append("table_name AS tableName,table_comment AS tableComment ");
		sql.append("FROM information_schema.tables "); 
		sql.append("WHERE table_schema=? ");
		String[] parameters=new String[]{postParam.getDbname()};
		List<TableVo> list=MySqlDb.executeQuery(sql.toString(), parameters, connection,TableVo.class);
		connection.close();
		return list;
	}
	
	/**
	 * 获得mysql数据库的所有表结构
	 * @param postParam
	 * @return
	 * @throws Exception 
	 */
	public static List<TableVo> generation(PostParamVo postParam) throws Exception{
		Connection connection=MySqlDb.getConnection(postParam);
		//获取表名的sql
		StringBuffer sql=new StringBuffer("SELECT ");
		sql.append("table_name AS tableName,table_comment AS tableComment ");
		sql.append("FROM information_schema.tables "); 
		sql.append("WHERE table_schema=? AND table_name IN ("+postParam.getTb()+")");
		String[] tablParameters=new String[]{postParam.getDbname()};
		List<TableVo> tabList=MySqlDb.executeQuery(sql.toString(), tablParameters, connection,TableVo.class);
		//表的描述
		StringBuffer colField=new StringBuffer();
		colField.append("column_name AS columnName,");
		colField.append("column_type AS columnType,");
		colField.append("column_default AS columnDefault,");
		colField.append("is_nullable AS isNullable,");
		colField.append("extra,column_key AS columnKey,");
		colField.append("column_comment AS columnComment");
		//表名
		String colTab="information_schema.columns";
		for(int i=0;i<tabList.size();i++){
			//查询sql
			String sqlCol=String.format("SELECT %s FROM %s WHERE table_name=?",colField.toString(),colTab);
			//获取table对象
			TableVo table=tabList.get(i);
			String[] colParameters=new String[]{table.getTableName()};
			List<ColumnVo> colList=MySqlDb.executeQuery(sqlCol, colParameters, connection,ColumnVo.class);
			table.setColums(colList);
		}
		connection.close();
		return tabList;
	}

}
