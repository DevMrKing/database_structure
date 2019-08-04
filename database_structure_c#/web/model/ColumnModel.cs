using System;

namespace web.model
{
    /// <summary>
    /// 列封装的类
    /// </summary>
    [Serializable]
    public class ColumnModel
    {
        /// <summary>
        ///  列名
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 列类型
        /// </summary>
        public string ColumnType { get; set; }

        /// <summary>
        /// 列默认值
        /// </summary>
        public string ColumnDefault { get; set; }

        /// <summary>
        /// 是否为null
        /// </summary>
        public string IsNullable { get; set; }

        /// <summary>
        /// 是否自增
        /// </summary>
        public string Extra { get; set; }

        /// <summary>
        /// 列键类型
        /// </summary>
        public string ColumnKey { get; set; }

        /// <summary>
        /// 列描述
        /// </summary>
        public string ColumnComment { get; set; }

    }
}