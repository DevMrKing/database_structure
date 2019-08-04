package vo;

import java.io.Serializable;

/**
 * json处理返回的封装类
 */
public class JsonVo implements Serializable{

	/**
	 * 
	 */
	private static final long serialVersionUID = 1376289374507755734L;
	
	/**
	 * 返回处理结果
	 */
	private boolean success;
	
	/**
	 * 返回处理信息
	 */
	private String msg;
	
	/**
	 * 返回数据结果体
	 */
	private Object data;
	
	public JsonVo(){}
	
	public JsonVo(boolean success,String msg,Object data){
		this.success=success;
		this.msg=msg;
		this.data=data;
	}

	public boolean isSuccess() {
		return success;
	}

	public void setSuccess(boolean success) {
		this.success = success;
	}

	public String getMsg() {
		return msg;
	}

	public void setMsg(String msg) {
		this.msg = msg;
	}

	public Object getData() {
		return data;
	}

	public void setData(Object data) {
		this.data = data;
	}
	
	/**
	 * 成功的处理
	 * @param success
	 * @param msg
	 * @param data
	 * @return
	 */
	public static JsonVo ok(String msg,Object data){
		return new JsonVo(true,msg,data);
	}
	
	/**
	 * 失败的处理
	 * @param success
	 * @param msg
	 * @param data
	 * @return
	 */
	public static JsonVo fail(String msg,Object data){
		return new JsonVo(false,msg,data);
	}
}
