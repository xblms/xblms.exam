using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Enums;

namespace XBLMS.Core.Services
{
    public partial class AuthManager
    {
        public async Task<List<Menu>> GetMenus()
        {
            var config = await _databaseManager.ConfigRepository.GetAsync();
            var admin = await GetAdminAsync();
            if (admin.Auth == AuthorityType.Admin)
            {
                return _settingsManager.GetMenus(config.SystemCode);
            }
            else if (admin.Auth == AuthorityType.AdminCompany)
            {
                return _settingsManager.GetMenus(config.SystemCode, admin.Auth);
            }
            else
            {
                var roles = await _databaseManager.AdministratorsInRolesRepository.GetRoleIdsForAdminAsync(admin.Id);
                var menuIds = new List<string>();
                foreach (var roleId in roles)
                {
                    var role = await _databaseManager.RoleRepository.GetRoleAsync(roleId);
                    if (role != null)
                    {
                        menuIds.AddRange(role.MenuIds);
                    }
                }
                menuIds = menuIds.Distinct().ToList();
                if (menuIds.Count > 0)
                {
                    return _settingsManager.GetMenus(config.SystemCode, admin.Auth, false, menuIds);
                }
                return null;
            }
        }
    }
}
