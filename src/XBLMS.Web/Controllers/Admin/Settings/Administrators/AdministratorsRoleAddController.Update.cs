using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Administrators
{
    public partial class AdministratorsRoleAddController
    {
        [HttpPost, Route(RouteUpdate)]
        public async Task<ActionResult<BoolResult>> UpdateRole([FromBody] SubmitRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }

            var role = await _roleRepository.GetRoleAsync(request.RoleId);
            if (role.RoleName != request.RoleName)
            {
                if (await _roleRepository.IsRoleExistsAsync(request.RoleName))
                {
                    return this.Error("角色名称已存在，请更换角色名称！");
                }
            }


            role.RoleName = request.RoleName;
            role.Description = request.Description;
            role.SelectIds = request.SelectIds;
            role.MenuIds = new List<string>();
            role.PermissionIds = new List<string>();

            if (request.Menus != null && request.Menus.Count > 0)
            {
                foreach (var item in request.Menus)
                {
                    if (!item.IsPermission)
                    {
                        role.MenuIds.Add(item.Id);
                    }
                    else
                    {
                        role.PermissionIds.Add(item.Id);
                    }
                }
            }


            await _roleRepository.UpdateRoleAsync(role);

            _cacheManager.Clear();

            await _authManager.AddAdminLogAsync("修改管理员角色", $"{request.RoleName}");

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
