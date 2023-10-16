using datatablegenerator.Models;
using datatablegenerator.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datatablegenerator.Common.Ado.Providers
{
    public class SqlServerProvider : AdoProviderBase
    {
        public SqlServerProvider(string connectString) : base(connectString) { }

        protected override DbProviderFactory DbProvider => SqlClientFactory.Instance;

        /// <summary>
        /// 建表sql
        /// </summary>
        /// <param name="db">数据库名</param>
        /// <param name="dt">数据表名</param>
        /// <param name="models">数据model</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override FuncResult<string> CreateTableSQL(string db, string dt, IEnumerable<TableModel> models)
        {
            string content = string.Empty;
            string remarkContent = string.Empty;
            foreach (var item in models)
            {
                try
                {
                    //TODO: 待优化
                    // 欢迎大家指点 ^-^
                    string name = item.Name;
                    string type = item.Type;
                    string required = item.Required == true ? "not null " : " ";

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
                    string defaults = string.IsNullOrWhiteSpace(item.Default) ? "" : DataTypeHelper.IsCharType(item.Type.Trim()) ? $" default('{item.Default}')" : $" default({item.Default})";

                    content += name + " " + type + " " + required + constraint + defaults + ",\r\n";

                    remarkContent += $"EXECUTE SP_ADDEXTENDEDPROPERTY 'MS_Description','{item.Remacks}','USER','DBO','TABLE','{dt}','COLUMN','{item.Name}';\r\n";
                }
                catch (Exception ex)
                {
                    Log<MainWindowModel>.Error($"创建数据表SQL,错误消息：{ex.Message};堆栈：{ex.StackTrace}");
                    return FuncResult.Fail<string>(ex.Message);
                }
            }

            return FuncResult.Success<string>($"\r\n USE {db}\r\n CREATE TABLE {dt} \r\n ({content})\r\n {remarkContent}");
        }

        /// <summary>
        /// 是否存在该表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public override async Task<FuncResult> ExistTable(string tableName)
        {
            string sql = $"SELECT OBJECT_ID(N'{tableName}',N'U')";
            var result = await IsTableExist(sql);
            if (!result)
            {
                return FuncResult.Success();
            }
            return FuncResult.Fail(result.Message);
        }

        /// <summary>
        /// 生成表
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dt"></param>
        /// <param name="models"></param>
        /// <returns></returns>
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

            if (isok.Data != -1)
            {
                return FuncResult.Fail("创建数据表失败！");
            }
            //await CreateTable(result.Data);
            return FuncResult.Success();
        }


    }
}
