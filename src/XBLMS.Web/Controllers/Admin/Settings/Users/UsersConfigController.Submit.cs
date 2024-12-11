using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Users
{
    public partial class UsersConfigController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<BoolResult>> Submit([FromBody] SubmitRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }

            var config = await _configRepository.GetAsync();

            config.IsHomeClosed = request.IsHomeClosed;
            config.UserPasswordMinLength = request.UserPasswordMinLength;
            config.UserPasswordRestriction = request.UserPasswordRestriction;
            config.IsUserLockLogin = request.IsUserLockLogin;
            config.UserLockLoginCount = request.UserLockLoginCount;
            config.UserLockLoginType = request.UserLockLoginType;
            config.UserLockLoginHours = request.UserLockLoginHours;
            config.IsUserCaptchaDisabled = request.IsUserCaptchaDisabled;

            await _configRepository.UpdateAsync(config);

            await _authManager.AddAdminLogAsync("修改用户设置");
            await _authManager.AddStatLogAsync(StatType.None, "修改用户设置");
            return new BoolResult
            {
                Value = true
            };
        }
    }
}
