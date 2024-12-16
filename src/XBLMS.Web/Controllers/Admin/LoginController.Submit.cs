using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Core.Utils;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin
{
    public partial class LoginController
    {
        [HttpPost, Route(Route)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SubmitResult>> Submit([FromBody] SubmitRequest request)
        {
            Administrator administrator;
            var config = await _configRepository.GetAsync();
            if (!request.IsForceLogoutAndLogin && !config.IsAdminCaptchaDisabled)
            {
                var captcha = TranslateUtils.JsonDeserialize<CaptchaUtils.Captcha>(_settingsManager.Decrypt(request.Token));

                if (captcha == null || string.IsNullOrEmpty(captcha.Value) || captcha.ExpireAt < DateTime.Now)
                {
                    return this.Error("验证码已超时，请点击刷新验证码！");
                }

                if (!StringUtils.EqualsIgnoreCase(captcha.Value, request.Value) || CaptchaUtils.IsAlreadyUsed(captcha, _cacheManager))
                {
                    return this.Error("验证码不正确，请重新输入！");
                }
            }

            string userName;
            string errorMessage;
            (administrator, userName, errorMessage) = await _administratorRepository.ValidateAsync(request.Account, request.Password, true);

            if (administrator == null)
            {
                administrator = await _administratorRepository.GetByUserNameAsync(userName);
                if (administrator != null)
                {
                    await _administratorRepository.UpdateLastActivityDateAndCountOfFailedLoginAsync(administrator); // 记录最后登录时间、失败次数+1
                }

                await _statRepository.AddCountAsync(StatType.AdminLoginFailure);
                return this.Error(errorMessage);
            }

            administrator = await _administratorRepository.GetByUserNameAsync(userName);

            await _administratorRepository.UpdateLastActivityDateAndCountOfLoginAsync(administrator); // 记录最后登录时间、失败次数清零

            var token = _authManager.AuthenticateAdministrator(administrator, request.IsPersistent);

            await _statRepository.AddCountAsync(StatType.AdminLoginSuccess);
            await _logRepository.AddAdminLogAsync(administrator, PageUtils.GetIpAddress(Request), Constants.ActionsLoginSuccess);

            var cacheKey = Constants.GetSessionIdCacheKey(administrator.Id);
            if (!request.IsForceLogoutAndLogin)
            {
                var cacheState = await _dbCacheRepository.GetValueAndCreatedDateAsync(cacheKey);
                if (!string.IsNullOrEmpty(cacheState.Item1) && cacheState.Item2 > DateTime.Now.AddMinutes(-30))
                {
                    return new SubmitResult
                    {
                        IsLoginExists = true,
                    };
                }
            }

            var sessionId = StringUtils.Guid();
            await _dbCacheRepository.RemoveAndInsertAsync(cacheKey, sessionId);

            return new SubmitResult
            {
                IsLoginExists = false,
                Administrator = administrator,
                SessionId = sessionId,
                Token = token
            };
        }
    }
}
