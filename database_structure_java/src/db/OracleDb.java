package db;

import java.sql.*;

import vo.*;

/**
 * oracle数据库帮助类
 */
public class OracleDb extends JdbcDb{
	static{
		try{
			String driver = "oracle.jdbc.OracleDriver";
			Class.forName(driver);
		}catch(Exception ex){
			ex.printStackTrace();
		}
	}

	/**
	 * 获取oracle连接
	 * @param postParam 参数
	 * @return 数据库连接
	 * @throws SQLException 
	 */
	public static Connection getConnection(PostParamVo postParam) throws SQLException {
		String url=String.format("jdbc:oracle:thin:@//%s:%s/%s",postParam.getHost(),postParam.getPort(),postParam.getDbname());
		Connection connection = DriverManager.getConnection(url, postParam.getUser(), postParam.getPass());
		return connection;
	}
}
