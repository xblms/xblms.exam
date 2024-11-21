using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Users
{
    public partial class UsersLayerPasswordController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<BoolResult>> Submit([FromBody] SubmitRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }

            var user = await _userRepository.GetByUserIdAsync(request.UserId);
            if (user == null) return this.Error(Constants.ErrorNotFound);

            var (success, errorMessage) = await _userRepository.ChangePasswordAsync(user.Id, request.Password);
            if (!success)
            {
                return this.Error($"更改密码失败：{errorMessage}");
            }

            await _authManager.AddAdminLogAsync("重设用户密码", $"{user.UserName}");

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
