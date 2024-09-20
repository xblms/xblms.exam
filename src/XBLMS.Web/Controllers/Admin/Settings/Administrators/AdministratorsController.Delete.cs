using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Administrators
{
    public partial class AdministratorsController
    {
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<BoolResult>> Delete([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Delete))
            {
                return this.NoAuth();
            }
            if (request.Id == 1) { return this.Error("你想干啥？"); }

            if (request.Id == _authManager.AdminId) { return this.Error("不能删除自己"); }

            var adminInfo = await _administratorRepository.GetByUserIdAsync(request.Id);
            await _administratorRepository.DeleteAsync(adminInfo.Id);

            await _authManager.AddAdminLogAsync("删除管理员", $"管理员:{adminInfo.UserName}");

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
