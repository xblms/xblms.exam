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

            var user = await _userRepository.GetByUserIdAsync(request.Id);

            await _organManager.DeleteUser(request.Id);

            await _authManager.AddAdminLogAsync("删除用户账号", $"{user.UserName}");

            await _authManager.AddStatLogAsync(StatType.UserDelete, "删除用户账号", user.Id, user.DisplayName, user);
            await _authManager.AddStatCount(StatType.UserDelete);

            return new BoolResult
            {
                Value = true
            };
        }

        [HttpPost, Route(RouteDeletes)]
        public async Task<ActionResult<BoolResult>> Deletes([FromBody] GetDeletesRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Delete))
            {
                return this.NoAuth();
            }
            if (request.Ids != null && request.Ids.Count > 0)
            {
                foreach (var id in request.Ids)
                {
                    var user = await _userRepository.GetByUserIdAsync(id);

                    await _organManager.DeleteUser(id);

                    await _authManager.AddAdminLogAsync("删除用户账号", $"{user.UserName}");
                    await _authManager.AddStatLogAsync(StatType.UserDelete, "删除用户账号", user.Id, user.DisplayName, user);
                    await _authManager.AddStatCount(StatType.UserDelete);

                }
            }

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
