using datatablegenerator.Common.Ado.Providers;
using datatablegenerator.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datatablegenerator.Common.Ado
{
    public class AdoFactory
    {

        public static AdoProviderBase GetProvider(DataSourceType dataSourceType, string connectionString)
        {
            switch (dataSourceType)
            {
                case DataSourceType.MSSQL:
                    return new SqlServerProvider(connectionString);
                case DataSourceType.MySQL:
                    return new MySqlProvider(connectionString);
                default:
                    return null;
            }

        }

    }
}
