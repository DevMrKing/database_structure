package vo;

/**
 * 传递过程中的参数
 */
public class PostParamVo {
	
	/**
	 * 请求处理的方法
	 */
	private String method;

	/**
	 * 数据库类型
	 */
	private String dbms;
	
	/**
	 * ip
	 */
	private String host;
	
	/**
	 * 端口
	 */
	private String port;
	
	/**
	 * 数据库名
	 */
	private String dbname;
	
	/**
	 * 数据库用户
	 */
	private String user;
	
	/**
	 * 数据库密码
	 */
	private String pass;
	
	/**
	 * 选择的表
	 */
	private String tb;
	
	/**
	 * 显示的列名
	 */
	private String cols;
	
	/**
	 * 搜索框中的提示
	 */
	private String searchInputPlaceholder;
	
	public String getMethod() {
		return method;
	}

	public void setMethod(String method) {
		this.method = method;
	}

	public String getDbms() {
		return dbms;
	}

	public void setDbms(String dbms) {
		this.dbms = dbms;
	}

	public String getHost() {
		return host;
	}

	public void setHost(String host) {
		this.host = host;
	}

	public String getPort() {
		return port;
	}

	public void setPort(String port) {
		this.port = port;
	}

	public String getDbname() {
		return dbname;
	}

	public void setDbname(String dbname) {
		this.dbname = dbname;
	}

	public String getUser() {
		return user;
	}

	public void setUser(String user) {
		this.user = user;
	}

	public String getPass() {
		return pass;
	}

	public void setPass(String pass) {
		this.pass = pass;
	}

	public String getTb() {
		return tb;
	}

	public void setTb(String tb) {
		this.tb = tb;
	}

	public String getCols() {
		return cols;
	}

	public void setCols(String cols) {
		this.cols = cols;
	}

	public String getSearchInputPlaceholder() {
		return searchInputPlaceholder;
	}

	public void setSearchInputPlaceholder(String searchInputPlaceholder) {
		this.searchInputPlaceholder = searchInputPlaceholder;
	}
	
}