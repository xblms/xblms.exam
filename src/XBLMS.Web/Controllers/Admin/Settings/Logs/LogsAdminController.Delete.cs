using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Logs
{
    public partial class LogsAdminController
    {
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<BoolResult>> Delete()
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Delete))
            {
                return this.NoAuth();
            }
            var adminAuth = await _authManager.GetAdminAuth();
            await _logRepository.DeleteAllAdminLogsAsync(adminAuth);

            await _authManager.AddAdminLogAsync("清空管理员日志");
            await _authManager.AddStatLogAsync(StatType.None, "清空管理员日志");

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
