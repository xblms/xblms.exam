using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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
            await _organManager.DeleteAdministrator(adminInfo.Id);
       
            await _authManager.AddAdminLogAsync("删除管理员", $"{adminInfo.UserName}");
            await _authManager.AddStatLogAsync(StatType.AdminDelete, "删除管理员", adminInfo.Id, adminInfo.UserName, adminInfo);
            await _authManager.AddStatCount(StatType.AdminDelete);

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
