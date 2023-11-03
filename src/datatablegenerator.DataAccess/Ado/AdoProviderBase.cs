using datatablegenerator.Common;
using datatablegenerator.Models;
using System.Data;
using System.Data.Common;


namespace datatablegenerator.DataAccess.Ado
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
        /// 成功要求
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected abstract bool SuccessRequire(int index);

        /// <summary>
        /// 建表sql
        /// </summary>
        /// <param name="db">数据库名</param>
        /// <param name="dt">数据表名</param>
        /// <param name="models">数据</param>
        /// <returns></returns>
        protected abstract FuncResult<string> CreateTableSQL(string db, string dt, IEnumerable<TableModel> models);

        protected abstract string ShowSQL(string tableName);


        /// <summary>
        /// 生成表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<FuncResult> GeneraterTable(string db, string dt, IEnumerable<TableModel> models)
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

            if (!SuccessRequire(isok.Data))
            {
                return FuncResult.Fail("不符合成功要求！");
            }
            Log<AdoProviderBase>.Info(result.Data);
            return FuncResult.Success();
        }


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
        public async Task<FuncResult> IsTableExist(string tableName)
        {
            var sql = ShowSQL(tableName);
            var result = await ExecuteNonQueryAsync(sql);
            if (!result || result.Data == -1)
            {
                return FuncResult.Fail(result.Message);
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

    }
}
