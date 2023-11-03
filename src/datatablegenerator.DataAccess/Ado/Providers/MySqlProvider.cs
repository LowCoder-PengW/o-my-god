using datatablegenerator.Common;
using datatablegenerator.Models;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace datatablegenerator.DataAccess.Ado.Providers
{
    public class MySqlProvider : AdoProviderBase
    {
        public MySqlProvider(string connectString) : base(connectString) { }
        protected override DbProviderFactory DbProvider => MySqlClientFactory.Instance;

        protected override FuncResult<string> CreateTableSQL(string db, string dt, IEnumerable<TableModel> models)
        {
            List<string> content = new List<string>();
            foreach (var item in models)
            {
                try
                {
                    //TODO: 待优化
                    // 欢迎大家指点 ^-^
                    string name = item.Name;
                    string type = item.Type;
                    string required = item.Required == true ? "NOT NULL " : "NULL DEFAULT NULL";

                    if (!string.IsNullOrWhiteSpace(item.Constraint))
                    {
                        var result = DataTypeHelper.ConstraintStatus(item.Constraint, item.Name);
                        if (!result)
                        {
                            return FuncResult.Fail<string>(result.Message);
                        }
                        item.Constraint = result.Data;
                    }
                    string constraint = item.Constraint;
                    string defaults = string.IsNullOrWhiteSpace(item.Default) ? "" : DataTypeHelper.IsCharType(item.Type.Trim()) ? $" DEFAULT ('{item.Default}')" : $" DEFAULT CURRENT_TIMESTAMP";
                    string remarks = item.Remacks;
                    content.Add($"{name} {type} {required} {constraint} {defaults} COMMENT '{remarks}'\r\n");

                }
                catch (Exception ex)
                {
                    Log<MySqlProvider>.Error($"创建数据表SQL,错误消息：{ex.Message};堆栈：{ex.StackTrace}");
                    return FuncResult.Fail<string>(ex.Message);
                }
            }

            return FuncResult.Success<string>($"\r\n USE {db};\r\n CREATE TABLE {dt} \r\n ({string.Join(",", content)})");

        }


        protected override bool SuccessRequire(int index)
        {
            return index == 0;
        }

        protected override string ShowSQL(string tableName)
        {
            return $"SHOW TABLES LIKE '{tableName}';";
        }


    }
}
