using datatablegenerator.DataAccess.Ado.Providers;
using datatablegenerator.Enums;

namespace datatablegenerator.DataAccess.Ado
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
