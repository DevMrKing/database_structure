package vo;

import java.io.Serializable;

/**
 * 列封装的vo
 */
public class ColumnVo implements Serializable{

	private static final long serialVersionUID = 3963812990964927657L;

	/**
	 * 列名
	 */
	private String columnName;
	/**
	 * 列类型
	 */
	private String columnType;
	/**
	 * 列默认值
	 */
	private String columnDefault;
	/**
	 * 是否为null
	 */
	private String isNullable;
	/**
	 * 是否自增
	 */
	private String extra;
	/**
	 * 列键类型
	 */
	private String columnKey;
	/**
	 * 列描述
	 */
	private String columnComment;
	
	public String getColumnName() {
		return columnName;
	}
	public void setColumnName(String columnName) {
		this.columnName = columnName;
	}
	public String getColumnType() {
		return columnType;
	}
	public void setColumnType(String columnType) {
		this.columnType = columnType;
	}
	public String getColumnDefault() {
		return columnDefault;
	}
	public void setColumnDefault(String columnDefault) {
		this.columnDefault = columnDefault;
	}
	public String getIsNullable() {
		return isNullable;
	}
	public void setIsNullable(String isNullable) {
		this.isNullable = isNullable;
	}
	public String getExtra() {
		return extra;
	}
	public void setExtra(String extra) {
		this.extra = extra;
	}
	public String getColumnKey() {
		return columnKey;
	}
	public void setColumnKey(String columnKey) {
		this.columnKey = columnKey;
	}
	public String getColumnComment() {
		return columnComment;
	}
	public void setColumnComment(String columnComment) {
		this.columnComment = columnComment;
	}	
}
