using System.Collections.Generic;
using System.Threading.Tasks;
using Datory;
using SqlKata;

namespace XBLMS.Services
{
    public partial interface IDatabaseManager
    {
        Task<List<KeyValuePair<int, IDictionary<string, object>>>> ParserGetSqlDataSourceAsync(
            DatabaseType databaseType, string connectionString, string queryString);

        Task<List<KeyValuePair<int, IDictionary<string, object>>>> ParserGetSqlDataSourceAsync(
            DatabaseType databaseType, string connectionString, Query query);
    }
}
