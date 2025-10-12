using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Administrators
{
    public partial class AdministratorsRoleAddController
    {
        [HttpPost, Route(RouteSetRole)]
        public async Task<ActionResult<BoolResult>> SetRole([FromBody] SetRoleRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }

            var admin = await _authManager.GetAdminAsync();

            foreach (var adminId in request.AdminIds)
            {
                await _administratorsInRolesRepository.DeleteUserAsync(adminId);
            }

            if (request.RoleIds != null && request.RoleIds.Count > 0 && request.AdminIds != null && request.AdminIds.Count > 0)
            {
                var roleNames = new List<string>();
                foreach (var roleId in request.RoleIds)
                {
                    var role = await _roleRepository.GetRoleAsync(roleId);
                    roleNames.Add(role.RoleName);
                    foreach (var adminId in request.AdminIds)
                    {
                        await _administratorsInRolesRepository.InsertAsync(new AdministratorsInRoles
                        {
                            RoleId = roleId,
                            AdminId = adminId,
                            CompanyId = admin.CompanyId,
                            DepartmentId = admin.DepartmentId,
                            CreatorId = admin.Id,
                        });
                    }
                }
                foreach (var adminId in request.AdminIds)
                {
                    var setAdmin = await _administratorRepository.GetByUserIdAsync(adminId);
                    await _authManager.AddAdminLogAsync("配置管理员角色", $"管理员:{setAdmin.DisplayName}>角色:{ListUtils.ToString(roleNames)}");
                }
            }
            return new BoolResult
            {
                Value = true
            };

        }


        [HttpGet, Route(RouteSetRole)]
        public async Task<ActionResult<GetPermissionsResult>> GetRoleSet([FromQuery] IdRequest request)
        {
            var adminAuth = await _authManager.GetAdminAuth();

            var roleds = await _roleRepository.GetRolesAsync(adminAuth, string.Empty);
            var resultRoles = new List<GetPermissionsResultRole>();
            foreach (var role in roleds)
            {
                resultRoles.Add(new GetPermissionsResultRole
                {
                    Key = role.Id,
                    label = role.RoleName
                });
            }

            var adminRoleIds = new List<int>();
            if (request.Id > 0)
            {
                adminRoleIds = await _administratorsInRolesRepository.GetRoleIdsForAdminAsync(request.Id);
            }

            return new GetPermissionsResult
            {
                Roles = resultRoles,
                CheckedRoles = adminRoleIds
            };
        }
    }
}

