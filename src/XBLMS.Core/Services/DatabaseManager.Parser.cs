using Dapper;
using Datory;
using SqlKata;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XBLMS.Core.Services
{
    public partial class DatabaseManager
    {
        public async Task<List<KeyValuePair<int, IDictionary<string, object>>>> ParserGetSqlDataSourceAsync(DatabaseType databaseType, string connectionString, string queryString)
        {
            var rows = new List<KeyValuePair<int, IDictionary<string, object>>>();
            var itemIndex = 0;
            using (var connection = GetConnection(databaseType, connectionString))
            {
                using var reader = await connection.ExecuteReaderAsync(queryString);
                while (reader.Read())
                {
                    var dict = new Dictionary<string, object>();
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        dict[reader.GetName(i)] = reader.GetValue(i);
                    }
                    rows.Add(new KeyValuePair<int, IDictionary<string, object>>(itemIndex, dict));
                }
            }
            return rows;
        }

        public async Task<List<KeyValuePair<int, IDictionary<string, object>>>> ParserGetSqlDataSourceAsync(DatabaseType databaseType, string connectionString, Query query)
        {
            var rows = new List<KeyValuePair<int, IDictionary<string, object>>>();

            var component = query.GetOneComponent<FromClause>("from");
            if (component != null)
            {
                var tableName = component.Table;

                var database = new Database(databaseType, connectionString);
                var columns = await database.GetTableColumnsAsync(tableName);
                var repository = new Repository(database, tableName, columns);

                var list = await repository.GetAllAsync<object>(query);
                var itemIndex = 0;
                foreach (var row in list)
                {
                    var fields = row as IDictionary<string, object>;
                    rows.Add(new KeyValuePair<int, IDictionary<string, object>>(itemIndex, fields));
                }
            }

            return rows;
        }


        private string Quote(string identifier)
        {
            return _settingsManager.Database.GetQuotedIdentifier(identifier);
        }
    }
}
