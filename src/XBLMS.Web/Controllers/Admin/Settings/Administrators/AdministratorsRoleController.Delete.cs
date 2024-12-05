using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;

namespace XBLMS.Web.Controllers.Admin.Settings.Administrators
{
    public partial class AdministratorsRoleController
    {
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<BoolResult>> Delete([FromBody] IdRequest request)
        {

            var roleInfo = await _roleRepository.GetRoleAsync(request.Id);

            await _roleRepository.DeleteRoleAsync(roleInfo.Id);
            await _administratorsInRolesRepository.DeleteByRoleIdAsync(roleInfo.Id);

            await _authManager.AddAdminLogAsync("删除管理员角色", $"{roleInfo.RoleName}");
            await _authManager.AddStatLogAsync(StatType.AdminAuthDelete, "删除管理员角色", roleInfo.Id, roleInfo.RoleName, roleInfo);
            await _authManager.AddStatCount(StatType.AdminAuthDelete);

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
