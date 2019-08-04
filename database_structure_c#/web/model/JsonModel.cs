using System;

namespace web.model
{
    /// <summary>
    /// json处理返回的封装类
    /// </summary>
    [Serializable]
    public class JsonModel
    {

        /// <summary>
        /// 返回处理结果
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 返回处理信息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 返回数据结果体
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 默认初始化
        /// </summary>
        public JsonModel() { }

        /// <summary>
        /// 带参数初始化
        /// </summary>
        /// <param name="success"></param>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        public JsonModel(bool success, string msg, object data)
        {
            this.Success = success;
            this.Msg = msg;
            this.Data = data;
        }

        /// <summary>
        /// 成功的处理
        /// </summary>
        public static JsonModel Ok(string msg, object data)
        {
            return new JsonModel(true, msg, data);
        }

        /// <summary>
        /// 失败的处理
        /// </summary>
        public static JsonModel Fail(string msg, object data)
        {
            return new JsonModel(false, msg, data);
        }
    }
}
