using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Home
{
    public partial class HomeMenusController
    {
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<UserMenusResult>> Delete([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Delete))
            {
                return this.NoAuth();
            }
            await _userMenuRepository.DeleteAsync(request.Id);

            var menu = await _userMenuRepository.GetAsync(request.Id);
            await _authManager.AddAdminLogAsync("删除用户菜单", $"{ menu.Text }");

            return new UserMenusResult
            {
                UserMenus = await _userMenuRepository.GetUserMenusAsync()
            };
        }
    }
}
