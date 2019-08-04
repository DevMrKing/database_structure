package vo;

import java.io.Serializable;
import java.util.List;

/**
 * 表的封装vo
 */
public class TableVo implements Serializable{

	private static final long serialVersionUID = -8775992976135283989L;
	
	/**
	 * 表名
	 */
	private String tableName;
	/**
	 * 表描述
	 */
	private String tableComment;
	
	/**
	 * 列的集合
	 */
	private List<ColumnVo> colums;
	
	public String getTableName() {
		return tableName;
	}
	public void setTableName(String tableName) {
		this.tableName = tableName;
	}
	public String getTableComment() {
		return tableComment;
	}
	public void setTableComment(String tableComment) {
		this.tableComment = tableComment;
	}
	public List<ColumnVo> getColums() {
		return colums;
	}
	public void setColums(List<ColumnVo> colums) {
		this.colums = colums;
	}
}
