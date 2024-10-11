using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Core.Utils;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Home
{
    public partial class HomeMenusController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get()
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }
            var menus = await _userMenuRepository.GetUserMenusAsync();
            if (menus == null || menus.Count == 0)
            {
                await _userMenuRepository.ResetAsync();
            }

            return new GetResult
            {
                UserMenus = menus
            };
        }
    }
}
