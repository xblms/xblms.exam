using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Datory
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DatabaseType
    {
        [DataEnum(DisplayName = "MySql")] MySql,
        [DataEnum(DisplayName = "SqlServer")] SqlServer,
        [DataEnum(DisplayName = "PostgreSql")] PostgreSql,
        [DataEnum(DisplayName = "SQLite")] SQLite,
        [DataEnum(DisplayName = "人大金仓")] KingbaseES,
        [DataEnum(DisplayName = "达梦")] Dm,
        [DataEnum(DisplayName = "OceanBase")] OceanBase,
        [DataEnum(DisplayName = "MariaDB")] MariaDB
    }
}
