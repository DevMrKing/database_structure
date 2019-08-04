package db;

import java.sql.*;

import vo.*;

/**
 * sqlserver数据库帮助类
 */
public class SqlserverDb extends JdbcDb{
	static{
		try{
			String driver = "com.microsoft.sqlserver.jdbc.SQLServerDriver";
			Class.forName(driver);
		}catch(Exception ex){
			ex.printStackTrace();
		}
	}
	
	/**
	 * 获取sqlserver连接
	 * @param postParam 参数
	 * @return 数据库连接
	 * @throws SQLException 
	 */
	public static Connection getConnection(PostParamVo postParam) throws SQLException {
		String url=String.format("jdbc:sqlserver://%s:%s;databaseName=%s",postParam.getHost(),postParam.getPort(),postParam.getDbname());
		Connection connection = DriverManager.getConnection(url, postParam.getUser(), postParam.getPass());
		return connection;
	}
}
