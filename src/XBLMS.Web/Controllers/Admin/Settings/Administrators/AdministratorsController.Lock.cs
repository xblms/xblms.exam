using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Administrators
{
    public partial class AdministratorsController
    {
        [HttpPost, Route(RouteLock)]
        public async Task<ActionResult<BoolResult>> Lock([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }

            var adminInfo = await _administratorRepository.GetByUserIdAsync(request.Id);

            await _administratorRepository.LockAsync(new List<string>
            {
                adminInfo.UserName
            });

            await _authManager.AddAdminLogAsync("锁定管理员", $"{adminInfo.UserName}");
            await _authManager.AddStatLogAsync(StatType.AdminUpdate, "禁用管理员账号", adminInfo.Id, adminInfo.UserName);

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
