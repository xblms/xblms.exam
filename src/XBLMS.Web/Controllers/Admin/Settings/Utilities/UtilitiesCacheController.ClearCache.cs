using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Utilities
{
    public partial class UtilitiesCacheController
    {
        [HttpPost, Route(RouteClearCache)]
        public async Task<ActionResult<BoolResult>> ClearCache()
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.SystemClearCache))
            {
                return this.NoAuth();
            }
            await _dbCacheRepository.ClearAllExceptAdminSessionsAsync();

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
