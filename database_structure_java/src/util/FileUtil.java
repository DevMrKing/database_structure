package util;

import java.io.*;

import javax.servlet.http.HttpServletResponse;

/**
 * 文件操作封装工具类
 */
public class FileUtil {

	/**
	 * 从路径中读取文件内容
	 * @param path 文件路径
	 * @return
	 */
	public static String readContentFromPath(String path) {
		StringBuffer content = new StringBuffer();
		File file = new File(path);
		try {
			BufferedReader in = new BufferedReader(new InputStreamReader(new FileInputStream(file), "utf-8"));  
            String str;
            while ((str = in.readLine()) != null) {
            	content.append(str);
            }
            in.close();
		} catch (Exception e) {
			e.printStackTrace();
		}
		return content.toString();
	}
	
	/**
	 * 下载文件
	 * @param fileName 文件名
	 * @param content 文件内容
	 * @param response
	 * @return
	 */
	public static void downFile(String fileName,String content,HttpServletResponse response){
		try{
			// 清空response
	        response.reset();
	        // 设置response的Header
	        response.addHeader("Content-Disposition", "attachment;filename=" + new String(fileName.getBytes()));
	        response.setContentType("application/octet-stream");
	        response.setHeader("Accept-Ranges", "bytes");
	        OutputStream toClient = new BufferedOutputStream(response.getOutputStream());
	        toClient.write(content.getBytes("UTF-8"));
	        toClient.flush();
	        toClient.close(); 
		}catch(Exception ex){
			ex.printStackTrace();
		}
	}
}
