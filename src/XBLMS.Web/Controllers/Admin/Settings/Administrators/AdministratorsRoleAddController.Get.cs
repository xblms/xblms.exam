using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Administrators
{
    public partial class AdministratorsRoleAddController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> GetList([FromQuery] GetRequest request)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var config = await _configRepository.GetAsync();
            var adminAuth = await _authManager.GetAdminAuth();

            var admin = await _authManager.GetAdminAsync();

            var role = await _roleRepository.GetRoleAsync(request.RoleId);

            List<Menu> allMenus = null;

            if (adminAuth.AuthType == Enums.AuthorityType.AdminNormal)
            {
                var roles = await _administratorsInRolesRepository.GetRoleIdsForAdminAsync(admin.Id);
                var menuIds = new List<string>();
                foreach (var roleId in roles)
                {
                    var rolet = await _roleRepository.GetRoleAsync(roleId);
                    if (rolet != null)
                    {
                        menuIds.AddRange(rolet.MenuIds);
                    }
                }
                menuIds = menuIds.Distinct().ToList();
                if (menuIds.Count > 0)
                {
                    allMenus = _settingsManager.GetMenus(config.SystemCode, admin.Auth, true, menuIds);
                }
            }
            else
            {
                allMenus = _settingsManager.GetMenus(config.SystemCode, admin.Auth, true);
            }



            return new GetResult
            {
                Role = role,
                Menus = allMenus
            };
        }
    }
}
