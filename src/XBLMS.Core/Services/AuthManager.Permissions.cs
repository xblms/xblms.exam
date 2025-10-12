using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            if (admin.Auth == AuthorityType.Admin || admin.Auth == AuthorityType.AdminCompany) return true;

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

        public List<Select<string>> AuthorityDataTypes()
        {
            return ListUtils.GetSelects<AuthorityDataType>();
        }
        public List<Select<string>> AuthorityTypes()
        {
            return ListUtils.GetSelects<AuthorityType>();
        }

        public async Task<AdminAuth> GetAdminAuth()
        {
            var admin = await GetAdminAsync();
            var cpath = admin.CompanyParentPath;
            var dpath = admin.DepartmentParentPath;
            var dId = admin.DepartmentId;
            if (admin.CompanyId != admin.AuthDataCurrentOrganId)
            {
                var company = await _databaseManager.OrganCompanyRepository.GetAsync(admin.AuthDataCurrentOrganId);
                cpath = company.CompanyParentPath;
                dpath = new List<string>();
                dId = 0;
            }

            var authResult = new AdminAuth()
            {
                Admin = admin,
                AuthType = admin.Auth,
                AuthDataType = admin.AuthData,
                AuthDataShowAll = admin.AuthDataShowAll,
                AdminId = admin.Id,
                CompanyId = admin.CompanyId,
                DepartmentId = dId,
                CurCompanyId = admin.AuthDataCurrentOrganId,
                CompanyParentPath = cpath,
                DepartmentParentPath = dpath,
            };
            return authResult;
        }
    }
}
