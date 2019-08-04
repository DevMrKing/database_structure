package servlet;

import java.util.List;

import javax.servlet.ServletContext;
import javax.servlet.http.*;

import com.alibaba.fastjson.JSON;

import service.*;
import util.*;
import vo.*;

/**
 * 后台处理统一入口
 */
public class FunctionServlet extends HttpServlet {
	private static final long serialVersionUID = 1L;
  
    public FunctionServlet() {
        super();
    }

    /**
     * 处理post请求模式
     */
	protected void doPost(HttpServletRequest request, HttpServletResponse response){
		try{
			response.setContentType("text/html;charset=utf-8"); 
			//设置编码
			request.setCharacterEncoding("utf-8");
			response.setCharacterEncoding("utf-8");
			//数据库类型
			String dbmsMysql="mysql";
			String dbmsOracle="oracle";
			String dbmsSqlserver="sqlserver";
			String dbmsPostgresql="postgresql";
			//封装参数
			PostParamVo postParam=getPostParam(request);
			if("test_connect".equals(postParam.getMethod())){
				//测试连接
				if(dbmsMysql.equals(postParam.getDbms())){
					//mysql
					if(MySqlService.testConnect(postParam)){
						String json=JSON.toJSONString(JsonVo.ok("",null));
						response.getWriter().write(json);
					}
				}else if(dbmsPostgresql.equals(postParam.getDbms())){
					//pgsql
					if(PostgresqlService.testConnect(postParam)){
						String json=JSON.toJSONString(JsonVo.ok("",null));
						response.getWriter().write(json);
					}
				}else if(dbmsSqlserver.equals(postParam.getDbms())){
					//sqlserver
					if(SqlserverService.testConnect(postParam)){
						String json=JSON.toJSONString(JsonVo.ok("",null));
						response.getWriter().write(json);
					}
				}else if(dbmsOracle.equals(postParam.getDbms())){
					//oracle
					if(OracleService.testConnect(postParam)){
						String json=JSON.toJSONString(JsonVo.ok("",null));
						response.getWriter().write(json);
					}
				}
			}else if("tables".equals(postParam.getMethod())){
				//显示所有表
				List<TableVo> dataList=null;
				if(dbmsMysql.equals(postParam.getDbms())){
					//mysql
					dataList=MySqlService.getTables(postParam);
				}else if(dbmsPostgresql.equals(postParam.getDbms())){
					//pgsql
					dataList=PostgresqlService.getTables(postParam);
				}else if(dbmsSqlserver.equals(postParam.getDbms())){
					//sqlserver
					dataList=SqlserverService.getTables(postParam);
				}else if(dbmsOracle.equals(postParam.getDbms())){
					//oracle
					dataList=OracleService.getTables(postParam);
				}
				String json=JSON.toJSONString(JsonVo.ok("",dataList));
				response.getWriter().write(json);
			}else if("generation".equals(postParam.getMethod())){
				//反向生成表结构
				List<TableVo> dataList=null;
				if(dbmsMysql.equals(postParam.getDbms())){
					//mysql
					dataList=MySqlService.generation(postParam);
				}else if(dbmsPostgresql.equals(postParam.getDbms())){
					//pgsql
					dataList=PostgresqlService.generation(postParam);
				}else if(dbmsSqlserver.equals(postParam.getDbms())){
					//sqlserver
					dataList=SqlserverService.generation(postParam);
				}else if(dbmsOracle.equals(postParam.getDbms())){
					//oracle
					dataList=OracleService.generation(postParam);
				}
				String json=JSON.toJSONString(JsonVo.ok("",dataList));
				response.getWriter().write(json);
			}else if("save_html".equals(postParam.getMethod())){
				ServletContext svltCtx=request.getSession().getServletContext();
				//模版文件位置
				String templateFilePath=null;
				//数据对象
				List<TableVo> dataList=null;
				if(dbmsMysql.equals(postParam.getDbms())){
					//mysql
					templateFilePath="/template/mysql/mysql_save.html";
					dataList=MySqlService.generation(postParam);
				}else if(dbmsPostgresql.equals(postParam.getDbms())){
					//pgsql
					templateFilePath="/template/postgresql/postgresql_save.html";
					dataList=PostgresqlService.generation(postParam);
				}else if(dbmsSqlserver.equals(postParam.getDbms())){
					//sqlserver
					templateFilePath="/template/sqlserver/sqlserver_save.html";
					dataList=SqlserverService.generation(postParam);
				}else if(dbmsOracle.equals(postParam.getDbms())){
					//oracle
					templateFilePath="/template/oracle/oracle_save.html";
					dataList=OracleService.generation(postParam);
				}
				//获取模版的磁盘位置
				templateFilePath=svltCtx.getRealPath(templateFilePath);
				//数据转换成json
				String dataJson=JSON.toJSONString(dataList);
				//读取文件到对象中
				String fileContent=FileUtil.readContentFromPath(templateFilePath);
				//替换文件里的
				fileContent=fileContent.replace("#tableDataJson#",dataJson);
				fileContent=fileContent.replace("#dbms#",postParam.getDbms());
				fileContent=fileContent.replace("#dbname#",postParam.getDbname());
				fileContent=fileContent.replace("#tableCols#",postParam.getCols());
				fileContent=fileContent.replace("#search_input_placeholder#",postParam.getSearchInputPlaceholder());
			    //下载文件
			    String downFileName=String.format("%s_%s_%d.html", postParam.getDbms(),postParam.getDbname(),System.currentTimeMillis());
			    FileUtil.downFile(downFileName,fileContent,response);
			}
		}catch(Exception ex){
			String msg=ex.getMessage();
			try{
				String json=JSON.toJSONString(JsonVo.fail(msg,null));
				response.getWriter().write(json);
			}catch(Exception ex1){
				ex1.printStackTrace();
			}
		}
	}
	
	/**
	 * 获得请求的参数
	 * @param request
	 * @return
	 */
	private PostParamVo getPostParam(HttpServletRequest request){
		PostParamVo param=new PostParamVo();
		param.setMethod(request.getParameter("method"));
		param.setDbms(request.getParameter("dbms"));
		param.setHost(request.getParameter("host"));
		param.setPort(request.getParameter("port"));
		param.setDbname(request.getParameter("dbname"));
		param.setUser(request.getParameter("user"));
		param.setPass(request.getParameter("pass"));
		param.setTb(request.getParameter("tb"));
		param.setCols(request.getParameter("cols"));
		param.setSearchInputPlaceholder(request.getParameter("search_input_placeholder"));
		return param;
	}

}