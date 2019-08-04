using System;
using System.Collections.Generic;

namespace web.model
{
    /// <summary>
    /// 表的封装vo
    /// </summary>
    [Serializable]
    public class TableModel
    {

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 表描述
        /// </summary>
        public string TableComment { get; set; }

        /// <summary>
        /// 列的集合
        /// </summary>
        public List<ColumnModel> Colums { get; set; }
    }
}