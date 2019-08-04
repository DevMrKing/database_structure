package service;

import java.sql.*;
import java.util.List;

import db.*;
import vo.*;

/**
 * oracle数据库服务类
 */
public class OracleService {
	/**
	 * 测试oracle数据库连接
	 * @param postParam
	 * @throws SQLException 
	 */
	public static boolean testConnect(PostParamVo postParam) throws Exception{
		Connection connection=OracleDb.getConnection(postParam);
		String sql="select count(1) from dual";
		java.math.BigDecimal ret=(java.math.BigDecimal)OracleDb.executeQueryOneColumn(sql, null, connection);
		connection.close();
		return ret.intValue()>0;
	}
	
	/**
	 * 获取oracle所有表名
	 * @param postParam
	 * @return
	 * @throws Exception 
	 */
	public static List<TableVo> getTables(PostParamVo postParam)throws Exception{
		Connection connection=OracleDb.getConnection(postParam);
		StringBuffer sql=new StringBuffer("SELECT ");
		sql.append("t.table_name AS \"tableName\",f.comments AS \"tableComment\" ");
		sql.append("FROM user_tables t ");
		sql.append("INNER JOIN user_tab_comments f ON t.table_name = f.table_name "); 
		List<TableVo> list=OracleDb.executeQuery(sql.toString(), null, connection,TableVo.class);
		connection.close();
		return list;
	}
	
	/**
	 * 获得oracle数据库的所有表结构
	 * @param postParam
	 * @return
	 * @throws Exception 
	 */
	public static List<TableVo> generation(PostParamVo postParam) throws Exception{
		Connection connection=OracleDb.getConnection(postParam);
		//获取表名的sql
		StringBuffer sql=new StringBuffer("SELECT ");
		sql.append("t.table_name AS \"tableName\",f.comments AS \"tableComment\" ");
		sql.append("FROM user_tables t ");
		sql.append("INNER JOIN user_tab_comments f ON t.table_name=f.table_name "); 
		sql.append("WHERE t.table_name IN ("+postParam.getTb()+")");
		List<TableVo> tabList=OracleDb.executeQuery(sql.toString(), null, connection,TableVo.class);
		//表的描述
		StringBuffer colField=new StringBuffer();
		colField.append("col.column_name AS \"columnName\",");
		colField.append("col.data_type AS \"columnType\",");
		colField.append("col.data_default AS \"columnDefault\",");
		colField.append("col.nullable AS \"isNullable\",");
		colField.append("cns.constraint_type AS \"columnKey\",");
		colField.append("ucc.comments AS \"columnComment\" ");
		//表的连接
		StringBuffer joinTab=new StringBuffer();
		joinTab.append("LEFT JOIN user_col_comments ucc ON ucc.table_name=col.table_name ");
		joinTab.append("AND ucc.column_name=col.column_name ");
		joinTab.append("LEFT JOIN user_cons_columns ccs ON ccs.table_name=col.table_name ");
		joinTab.append("AND ccs.column_name=col.column_name AND ccs.position=col.column_id ");
		joinTab.append("LEFT JOIN user_constraints cns ON col.table_name=cns.table_name ");
		joinTab.append("AND cns.constraint_type='P' AND ccs.constraint_name=cns.constraint_name ");
		//表名
		String tab=" user_tab_columns col ";
		String order=" ORDER BY col.column_id ASC ";
		for(int i=0;i<tabList.size();i++){
			//查询sql
			String sqlCol=String.format("SELECT %s FROM %s %s WHERE col.table_name =? %s",colField.toString(),tab,joinTab.toString(),order);
			//获取table对象
			TableVo table=tabList.get(i);
			String[] colParameters=new String[]{table.getTableName()};
			List<ColumnVo> colList=OracleDb.executeQuery(sqlCol, colParameters, connection,ColumnVo.class);
			table.setColums(colList);
		}
		connection.close();
		return tabList;
	}
}
