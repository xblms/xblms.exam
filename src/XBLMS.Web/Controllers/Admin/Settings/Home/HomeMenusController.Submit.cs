using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Models;
using XBLMS.Core.Utils;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Home
{
    public partial class HomeMenusController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<UserMenusResult>> Submit([FromBody] SubmitRequest request)
        {
            if (request.Id > 0)
            {
                if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
                {
                    return this.NoAuth();
                }
            }
            else
            {
                if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Add))
                {
                    return this.NoAuth();
                }
            }

            if (request.Id == 0)
            {
                await _userMenuRepository.InsertAsync(new UserMenu
                {
                    Disabled = request.Disabled,
                    ParentId = request.ParentId,
                    Taxis = request.Taxis,
                    Text = request.Text,
                    IconClass = request.IconClass,
                    Link = request.Link,
                    Target = request.Target
                });

                await _authManager.AddAdminLogAsync("新增用户菜单", $"用户菜单:{request.Text}");
            }
            else if (request.Id > 0)
            {
                var userMenu = await _userMenuRepository.GetAsync(request.Id);
                userMenu.Disabled = request.Disabled;
                userMenu.ParentId = request.ParentId;
                userMenu.Taxis = request.Taxis;
                userMenu.Text = request.Text;
                userMenu.IconClass = request.IconClass;
                userMenu.Link = request.Link;
                userMenu.Target = request.Target;
                await _userMenuRepository.UpdateAsync(userMenu);

                await _authManager.AddAdminLogAsync("修改用户菜单", $"用户菜单:{request.Text}");
            }

            return new UserMenusResult
            {
                UserMenus = await _userMenuRepository.GetUserMenusAsync()
            };
        }
    }
}
