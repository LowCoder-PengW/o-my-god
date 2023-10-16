using datatablegenerator.Models;
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
            throw new NotImplementedException();
        }

        public override Task<FuncResult> ExistTable(string tableName)
        {
            throw new NotImplementedException();
        }

        public override Task<FuncResult> GeneraterTable(string db, string dt, IEnumerable<TableModel> models)
        {
            throw new NotImplementedException();
        }


    }
}
