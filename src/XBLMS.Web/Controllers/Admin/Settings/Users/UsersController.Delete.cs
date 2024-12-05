using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Users
{
    public partial class UsersController
    {
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<BoolResult>> Delete([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Delete))
            {
                return this.NoAuth();
            }
            var user = await _userRepository.DeleteAsync(request.Id);

            await _authManager.AddAdminLogAsync("删除用户账号", $"{user.UserName}");

            await _authManager.AddStatLogAsync(StatType.UserDelete, "删除用户账号", user.Id, user.DisplayName, user);
            await _authManager.AddStatCount(StatType.UserDelete);

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
