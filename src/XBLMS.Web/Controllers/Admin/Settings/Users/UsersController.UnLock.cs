using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Dto;
using XBLMS.Core.Utils;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Users
{
    public partial class UsersController
    {
        [HttpPost, Route(RouteUnLock)]
        public async Task<ActionResult<BoolResult>> UnLock([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }
            var user = await _userRepository.GetByUserIdAsync(request.Id);

            await _userRepository.UnLockAsync(new List<int>
            {
                request.Id
            });

            await _authManager.AddAdminLogAsync("解锁用户", $"用户:{user.UserName}");

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
