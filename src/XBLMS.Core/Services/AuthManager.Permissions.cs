using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Datory;
using XBLMS.Configuration;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class AuthManager
    {
        public async Task<bool> HasPermissionsAsync(MenuPermissionType menuPermissionType = MenuPermissionType.Select)
        {
            var admin = await GetAdminAsync();
            if (admin.Auth == AuthorityType.Admin) return true;//超级管理员

            var menuId = MenuId;
            if (string.IsNullOrEmpty(menuId))
            {
                return false;
            }
            var myMenuIds = new List<string>();
            var myPermissionIds = new List<string>();
            var roleIds = await _databaseManager.AdministratorsInRolesRepository.GetRoleIdsForAdminAsync(admin.Id);
            if (roleIds == null || roleIds.Count == 0)
            {
                return false;
            }
            foreach (var roleId in roleIds)
            {
                var role = await _databaseManager.RoleRepository.GetRoleAsync(roleId);
                if (role != null)
                {
                    myMenuIds.AddRange(role.MenuIds);
                    myPermissionIds.AddRange(role.PermissionIds);
                }
            }
            if (myMenuIds.Count == 0 || !myMenuIds.Contains(menuId))
            {
                return false;
            }

            var permissionName = _settingsManager.GetPermissionId(menuId, menuPermissionType.GetValue());
            if (menuPermissionType != MenuPermissionType.Select && !myPermissionIds.Contains(permissionName))
            {
                return false;
            }
            return true; ;
        }


        public List<Select<string>> AuthorityTypes()
        {
            return ListUtils.GetSelects<AuthorityType>();
        }
    }
}
