using Datory;
using Datory.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin
{
    public partial class InstallController
    {
        [HttpPost, Route(RouteDatabaseConnect)]
        public async Task<ActionResult<DatabaseConnectResult>> DatabaseConnect([FromBody] DatabaseConnectRequest request)
        {
            if (!await _configRepository.IsNeedInstallAsync()) return Unauthorized();

            var databaseType = _settingsManager.Containerized ? _settingsManager.DatabaseType : request.DatabaseType;
            var databaseName = (databaseType == DatabaseType.Dm || databaseType == DatabaseType.KingbaseES) ? request.DatabaseName : string.Empty;
            var connectionString = _settingsManager.Containerized
                ? _settingsManager.DatabaseConnectionString
                : DbUtils.GetConnectionString(request.DatabaseType, request.DatabaseHost,
                    request.IsDatabaseDefaultPort, TranslateUtils.ToInt(request.DatabasePort), request.DatabaseUserName,
                    request.DatabasePassword, databaseName);

            var db = new Database(databaseType, connectionString);

            var (isConnectionWorks, message) = await db.IsConnectionWorksAsync();
            if (!isConnectionWorks)
            {
                return this.Error(message);
            }

            var databaseNames = new List<string>();

            if (string.IsNullOrEmpty(databaseName))
            {
                databaseNames = await db.GetDatabaseNamesAsync();
            }

            return new DatabaseConnectResult
            {
                DatabaseNames = databaseNames
            };
        }
    }
}
