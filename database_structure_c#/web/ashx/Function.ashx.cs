using System;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using web.model;
using web.bll;

namespace web.ashx
{
    /// <summary>
    /// Function 的摘要说明
    /// </summary>
    public class Function : IHttpHandler
    {
        /// <summary>
        /// 后台统一处理入口
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            response.ContentEncoding = Encoding.GetEncoding("utf-8");
            request.ContentEncoding = Encoding.GetEncoding("utf-8");
            response.ContentType = "text/html;charset=utf-8";
            try
            {
                //数据库类型
                string dbmsMysql = "mysql";
                string dbmsOracle = "oracle";
                string dbmsSqlserver = "sqlserver";
                string dbmsPostgresql = "postgresql";
                //封装请求参数
                PostParamModel postParam = GetPostParam(request);
                if ("test_connect".Equals(postParam.Method))
                {
                    //测试连接
                    if (dbmsMysql.Equals(postParam.Dbms))
                    {
                        //mysql
                        if (MySqlBll.TestConnect(postParam))
                        {
                            string json = JsonConvert.SerializeObject(JsonModel.Ok("", null));
                            response.Write(json);
                        }
                    }
                    else if (dbmsPostgresql.Equals(postParam.Dbms))
                    {
                        //pgsql
                        if (PostgreSqlBll.TestConnect(postParam))
                        {
                            string json = JsonConvert.SerializeObject(JsonModel.Ok("", null));
                            response.Write(json);
                        }
                    }
                    else if (dbmsSqlserver.Equals(postParam.Dbms))
                    {
                        //sqlserver
                        if (SqlserverBll.TestConnect(postParam))
                        {
                            string json = JsonConvert.SerializeObject(JsonModel.Ok("", null));
                            response.Write(json);
                        }
                    }
                    else if (dbmsOracle.Equals(postParam.Dbms))
                    {
                        //oracle
                        if (OracleBll.TestConnect(postParam))
                        {
                            string json = JsonConvert.SerializeObject(JsonModel.Ok("", null));
                            response.Write(json);
                        }
                    }
                }
                else if ("tables".Equals(postParam.Method))
                {
                    //显示所有表
                    List<TableModel> dataList = null;
                    if (dbmsMysql.Equals(postParam.Dbms))
                    {
                        //mysql
                        dataList = MySqlBll.GetTables(postParam);
                    }
                    else if (dbmsPostgresql.Equals(postParam.Dbms))
                    {
                        //pgsql
                        dataList = PostgreSqlBll.GetTables(postParam);
                    }
                    else if (dbmsSqlserver.Equals(postParam.Dbms))
                    {
                        //sqlserver
                        dataList = SqlserverBll.GetTables(postParam);
                    }
                    else if (dbmsOracle.Equals(postParam.Dbms))
                    {
                        //oracle
                        dataList = OracleBll.GetTables(postParam);
                    }
                    string json = JsonConvert.SerializeObject(JsonModel.Ok("", dataList));
                    response.Write(json);
                }
                else if ("generation".Equals(postParam.Method))
                {
                    //反向生成表结构
                    List<TableModel> dataList = null;
                    if (dbmsMysql.Equals(postParam.Dbms))
                    {
                        //mysql
                        dataList = MySqlBll.Generation(postParam);
                    }
                    else if (dbmsPostgresql.Equals(postParam.Dbms))
                    {
                        //pgsql
                        dataList = PostgreSqlBll.Generation(postParam);
                    }
                    else if (dbmsSqlserver.Equals(postParam.Dbms))
                    {
                        //sqlserver
                        dataList = SqlserverBll.Generation(postParam);
                    }
                    else if (dbmsOracle.Equals(postParam.Dbms))
                    {
                        //oracle
                        dataList = OracleBll.Generation(postParam);
                    }
                    string json = JsonConvert.SerializeObject(JsonModel.Ok("", dataList));
                    response.Write(json);
                }
                else if ("save_html".Equals(postParam.Method))
                {
                    //模版路径
                    string templateFilePath = null;
                    //当前上下文
                    string ctx = context.Server.MapPath(request.ApplicationPath);
                    //数据集
                    List<TableModel> dataList = null;
                    if (dbmsMysql.Equals(postParam.Dbms))
                    {
                        //mysql
                        dataList = MySqlBll.Generation(postParam);
                        templateFilePath = string.Format("{0}/template/mysql/mysql_save.html", ctx);
                    }
                    else if (dbmsPostgresql.Equals(postParam.Dbms))
                    {
                        //pgsql
                        dataList = PostgreSqlBll.Generation(postParam);
                        templateFilePath = string.Format("{0}/template/postgresql/postgresql_save.html", ctx);
                    }
                    else if (dbmsSqlserver.Equals(postParam.Dbms))
                    {
                        //sqlserver
                        dataList = SqlserverBll.Generation(postParam);
                        templateFilePath = string.Format("{0}/template/sqlserver/sqlserver_save.html", ctx);
                    }
                    else if (dbmsOracle.Equals(postParam.Dbms))
                    {
                        //oracle
                        dataList = OracleBll.Generation(postParam);
                        templateFilePath = string.Format("{0}/template/oracle/oracle_save.html", ctx);
                    }
                    //需要替换的json结果集
                    string json = JsonConvert.SerializeObject(dataList);
                    //读取文件内容
                    string content = File.ReadAllText(templateFilePath);
                    //替换文件
                    content = content.Replace("#tableDataJson#", json);
                    content = content.Replace("#dbms#", postParam.Dbms);
                    content = content.Replace("#dbname#", postParam.Dbname);
                    content = content.Replace("#tableCols#", postParam.Cols);
                    content = content.Replace("#search_input_placeholder#", postParam.SearchInputPlaceholder);
                    //下载文件
                    string downFileName = string.Format("{0}_{1}_{2}.html", postParam.Dbms, postParam.Dbname, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"));
                    DownFile(downFileName, content, response);
                }
            }
            catch (Exception ex)
            {
                string json = JsonConvert.SerializeObject(JsonModel.Fail(ex.Message, null));
                response.Write(json);
            }
        }

        /// <summary>
        /// 获得请求的参数
        /// </summary>
        /// <param name="request">request</param>
        /// <returns></returns>
        private PostParamModel GetPostParam(HttpRequest request)
        {
            PostParamModel param = new PostParamModel();
            param.Method = request.Form.Get("method");
            param.Dbms = request.Form.Get("dbms");
            param.Host = request.Form.Get("host");
            param.Port = request.Form.Get("port");
            param.Dbname = request.Form.Get("dbname");
            param.User = request.Form.Get("user");
            param.Pass = request.Form.Get("pass");
            param.Tb = request.Form.Get("tb");
            param.Cols = request.Form.Get("cols");
            param.SearchInputPlaceholder = request.Form.Get("search_input_placeholder");
            return param;
        }

        /// <summary>
        ///  文件流下载文件
        /// </summary>
        /// <param name="downFileName">下载的文件名</param>
        /// <param name="content">输出内容</param>
        /// <param name="response">响应</param>
        private void DownFile(string downFileName, string content, HttpResponse response)
        {
            response.ContentType = "application/octet-stream";
            response.AddHeader("Accept-Ranges", "bytes");
            response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(downFileName, System.Text.Encoding.UTF8));
            response.BinaryWrite(Encoding.UTF8.GetBytes(content));
            response.Flush();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}