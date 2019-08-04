using System;

namespace web.model
{
    /// <summary>
    /// 传递过程中的参数
    /// </summary>
    public class PostParamModel
    {
        /// <summary>
        /// 请求处理的方法
        /// </summary>
        public String Method { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public String Dbms { get; set; }

        /// <summary>
        /// ip
        /// </summary>
        public String Host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public String Port { get; set; }

        /// <summary>
        /// 数据库名
        /// </summary>
        public String Dbname { get; set; }

        /// <summary>
        /// 数据库用户
        /// </summary>
        public String User { get; set; }

        /// <summary>
        /// 数据库密码
        /// </summary>
        public String Pass { get; set; }

        /// <summary>
        /// 选择的表
        /// </summary>
        public String Tb { get; set; }

        /// <summary>
        /// 显示的列名
        /// </summary>
        public String Cols { get; set; }

        /// <summary>
        /// 搜索框中的提示
        /// </summary>
        public String SearchInputPlaceholder { get; set; }

    }
}