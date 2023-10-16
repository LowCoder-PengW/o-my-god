using datatablegenerator.Models;
using datatablegenerator.ViewModels;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datatablegenerator.Common.Ado.Providers
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
                    Log<MainWindowModel>.Error($"创建数据表SQL,错误消息：{ex.Message};堆栈：{ex.StackTrace}");
                    return FuncResult.Fail<string>(ex.Message);
                }
            }

            return FuncResult.Success<string>($"\r\n USE {db};\r\n CREATE TABLE {dt} \r\n ({string.Join(",", content)})");

        }

        /// <summary>
        /// 是否存在该表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public override async Task<FuncResult> ExistTable(string tableName)
        {
            string sql = $"SHOW TABLES LIKE '{tableName}';";
            var result = await IsTableExist(sql);
            if (!result)
            {
                return FuncResult.Success();
            }
            return FuncResult.Fail(result.Message);
        }

        public override async Task<FuncResult> GeneraterTable(string db, string dt, IEnumerable<TableModel> models)
        {
            var result = CreateTableSQL(db, dt, models);
            if (!result)
            {
                return FuncResult.Fail(result.Message);
            }

            var isok = await ExecuteNonQueryAsync(result.Data);
            if (!isok)
            {
                return FuncResult.Fail(isok.Message);
            }

            if (isok.Data != 0)
            {
                return FuncResult.Fail("创建数据表失败！");
            }
            Log<MySqlProvider>.Debug($"创建表SQL：\r\n {isok.Data}");
            return FuncResult.Success();
        }


    }
}
