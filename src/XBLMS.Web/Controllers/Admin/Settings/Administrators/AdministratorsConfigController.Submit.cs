using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Dto;
using XBLMS.Core.Utils;
using XBLMS.Utils;
using XBLMS.Enums;

namespace XBLMS.Web.Controllers.Admin.Settings.Administrators
{
    public partial class AdministratorsConfigController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<BoolResult>> Submit([FromBody] SubmitRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }

            var config = await _configRepository.GetAsync();

            config.AdminUserNameMinLength = request.AdminUserNameMinLength;
            config.AdminPasswordMinLength = request.AdminPasswordMinLength;
            config.AdminPasswordRestriction = request.AdminPasswordRestriction;

            config.IsAdminLockLogin = request.IsAdminLockLogin;
            config.AdminLockLoginCount = request.AdminLockLoginCount;
            config.AdminLockLoginType = request.AdminLockLoginType;
            config.AdminLockLoginHours = request.AdminLockLoginHours;

            config.IsAdminEnforcePasswordChange = request.IsAdminEnforcePasswordChange;
            config.AdminEnforcePasswordChangeDays = request.AdminEnforcePasswordChangeDays;

            config.IsAdminEnforceLogout = request.IsAdminEnforceLogout;
            config.AdminEnforceLogoutMinutes = request.AdminEnforceLogoutMinutes;

            config.IsAdminCaptchaDisabled = request.IsAdminCaptchaDisabled;

            await _configRepository.UpdateAsync(config);

            await _authManager.AddAdminLogAsync("修改管理员设置");

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
