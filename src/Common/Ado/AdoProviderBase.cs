using datatablegenerator.Models;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace datatablegenerator.Common.Ado
{
    public abstract class AdoProviderBase
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        private string ConnectionString { get; set; }
        /// <summary>
        /// 数据库类型工厂
        /// </summary>
        protected abstract DbProviderFactory DbProvider { get; }
        public AdoProviderBase(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// 是否存在表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public abstract Task<FuncResult> ExistTable(string tableName);
        /// <summary>
        /// 生成表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public abstract Task<FuncResult> GeneraterTable(string db, string dt, IEnumerable<TableModel> models);

        /// <summary>
        /// 建表sql
        /// </summary>
        /// <param name="db">数据库名</param>
        /// <param name="dt">数据表名</param>
        /// <param name="models">数据</param>
        /// <returns></returns>
        protected abstract FuncResult<string> CreateTableSQL(string db, string dt, IEnumerable<TableModel> models);


        /// <summary>
        /// 测试连接
        /// </summary>
        /// <returns></returns>
        public bool Connection()
        {
            try
            {
                using (var connection = DbProvider.CreateConnection())
                {
                    connection.ConnectionString = ConnectionString;
                    connection.Open();
                }
                return true;

            }
            catch (Exception ex)
            {
                Log<AdoProviderBase>.Error($"测试连接报错！{ex.Message};堆栈：{ex.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// 检查数据表是否存在
        /// </summary>
        /// <param name="sql"></param>
        protected async Task<FuncResult> IsTableExist(string sql)
        {
            try
            {
                using (var connection = DbProvider.CreateConnection())
                {
                    connection.ConnectionString = ConnectionString;
                    connection.Open();
                    using (var dbCommand = connection.CreateCommand())
                    {
                        dbCommand.CommandText = sql;
                        dbCommand.CommandType = CommandType.Text;
                        var result = await dbCommand.ExecuteScalarAsync();

                        if (!string.IsNullOrWhiteSpace(result?.ToString()))
                        {
                            return FuncResult.Fail("该表名已存在！");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log<AdoProviderBase>.Error($"检查数据表是否存在报错！{ex.Message};堆栈：{ex.StackTrace}");
                return FuncResult.Fail($"{ex.Message}");
            }
            return FuncResult.Success();
        }


        protected async Task<FuncResult<int>> ExecuteNonQueryAsync(string sql)
        {
            try
            {
                using (var connection = DbProvider.CreateConnection())
                {
                    connection.ConnectionString = ConnectionString;
                    connection.Open();
                    using (var dbCommand = connection.CreateCommand())
                    {
                        dbCommand.CommandText = sql;
                        dbCommand.CommandType = CommandType.Text;
                        var result = await dbCommand.ExecuteNonQueryAsync();

                        return FuncResult.Success(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log<AdoProviderBase>.Error($"创建数据表报错！SQL :{sql};{ex.Message};堆栈：{ex.StackTrace}");
                return FuncResult.Fail<int>($"{ex.Message}");
            }
        }

        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        protected async Task<FuncResult> CreateTable(string sql)
        {
            try
            {
                using (var connection = DbProvider.CreateConnection())
                {
                    connection.ConnectionString = ConnectionString;
                    connection.Open();
                    using (var dbCommand = connection.CreateCommand())
                    {
                        dbCommand.CommandText = sql;
                        dbCommand.CommandType = CommandType.Text;
                        var result = await dbCommand.ExecuteNonQueryAsync();

                        if (result != -1)
                        {
                            return FuncResult.Fail("创建数据表失败！");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log<AdoProviderBase>.Error($"创建数据表报错！{ex.Message};堆栈：{ex.StackTrace}");
                return FuncResult.Fail($"{ex.Message}");
            }
            return FuncResult.Success();

        }


    }
}
