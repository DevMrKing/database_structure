package db;

import java.sql.*;

import vo.*;

/**
 * pgsql帮助类
 */
public class PostgreSqlDb extends JdbcDb{
	static{
		try{
			String driver = "org.postgresql.Driver";
			Class.forName(driver);
		}catch(Exception ex){
			ex.printStackTrace();
		}
	}
	
	/**
	 * 获取pgsql连接
	 * @param postParam 参数
	 * @return 数据库连接
	 * @throws SQLException 
	 */
	public static Connection getConnection(PostParamVo postParam) throws SQLException {
		String url=String.format("jdbc:postgresql://%s:%s/%s?useUnicode=true&characterEncoding=utf8&useSSL=true",postParam.getHost(),postParam.getPort(),postParam.getDbname());
		Connection connection = DriverManager.getConnection(url, postParam.getUser(), postParam.getPass());
		return connection;
	}
}
