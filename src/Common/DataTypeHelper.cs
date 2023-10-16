using datatablegenerator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datatablegenerator.Common
{
    public class DataTypeHelper
    {
        //数据类型
        private static readonly List<string> _TypeList;

        private static readonly Dictionary<string, string> Dic;
        static DataTypeHelper()
        {
            _TypeList = new List<string> { "char", "varchar", "varchar(max)", "nchar", "nvarchar", "nvarchar(max)", "bit", "binary", "varbinary", "varbinary(max)", "image", "int", "bigint", "decimal", "float", "datetime", "datetime2", "date", "time", "text" };

            Dic = new Dictionary<string, string>();
            Dic.Add("PK", "PRIMARY KEY");
            // Dic.Add("UQ", "UNIQUE");
        }

        public static string GetConstraintSQL(string constraintKey)
        {
            return Dic[constraintKey];
        }


        /// <summary>
        /// 数据类型
        /// </summary>
        /// <returns></returns>
        public static List<string> GetListDataType()
        {
            return _TypeList;
        }


        /// <summary>
        /// 判断是否是字符类型
        /// </summary>
        /// <param name="charStr">数据类型</param>
        /// <returns>TRUE: 是字符类型；FALSE:其他类型</returns>
        public static bool IsCharType(string charStr)
        {
            string tempStr = charStr.Split('(').ToList().Take(1).First();

            switch (tempStr)
            {
                case "char":
                    return true;
                case "varchar":
                    return true;
                case "nchar":
                    return true;
                case "nvarchar":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 字段约束状态
        /// </summary>
        /// <param name="constraint"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static FuncResult<string> ConstraintStatus(string constraint, string fieldName)
        {
            var isok = Dic.ContainsKey(constraint.ToUpper());
            if (!isok)
            {
                return FuncResult.Fail<string>($"【{fieldName}】字段名称的约束值不正确！");
            }
            var sql = GetConstraintSQL(constraint.ToUpper());
            return FuncResult.Success<string>(sql);
        }

    }
}
