using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datatablegenerator.Models
{
    public class TableModel
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 必填
        /// </summary>
        public bool Required { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public string Default { get; set; }
        /// <summary>
        /// 约束
        /// </summary>
        public string Constraint { get; set; }
        /// <summary>
        /// 字段说明
        /// </summary>
        public string Remacks { get; set; }

    }
}
