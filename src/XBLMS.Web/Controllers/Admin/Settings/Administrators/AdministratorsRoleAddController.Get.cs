using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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

            var admin = await _authManager.GetAdminAsync();

            var role = await _roleRepository.GetRoleAsync(request.RoleId);

            var allMenus = _settingsManager.GetMenus(true);

            return new GetResult
            {
                Role = role,
                Menus = allMenus
            };
        }
    }
}
