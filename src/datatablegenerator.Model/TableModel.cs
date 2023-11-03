using MiniExcelLibs.Attributes;

namespace datatablegenerator.Models
{
    public class TableModel
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        [ExcelColumnName("字段名称")]
        public string Name { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        [ExcelColumnName("数据类型")]
        public string Type { get; set; }
        /// <summary>
        /// 必填
        /// </summary>
        [ExcelColumnName("必填")]
        public bool Required { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        [ExcelColumnName("默认值")]
        public string Default { get; set; }
        /// <summary>
        /// 约束
        /// </summary>
        [ExcelColumnName("约束")]
        public string Constraint { get; set; }
        /// <summary>
        /// 字段说明
        /// </summary>
        [ExcelColumnName("字段说明")]
        public string Remacks { get; set; }

    }
}
