using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Core.Utils;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home
{
    public partial class LoginController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<SubmitResult>> Submit([FromBody] SubmitRequest request)
        {
            User user;
            var config = await _configRepository.GetAsync();
            var ipAddress = PageUtils.GetIpAddress(Request);

            if (!config.IsUserCaptchaDisabled)
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
            (user, userName, errorMessage) = await _userRepository.ValidateAsync(request.Account, request.Password, true);

            if (user == null)
            {
                user = await _userRepository.GetByUserNameAsync(userName);
                if (user != null)
                {
                    await _authManager.AddUserStatCount(StatType.UserLoginFailure, user);
                    await _logRepository.AddUserLogAsync(user, ipAddress, Constants.ActionsLoginFailure, "帐号或密码错误");
                }
                return this.Error(errorMessage);
            }

            user = await _userRepository.GetByUserNameAsync(userName);

            await _userRepository.UpdateLastActivityDateAndCountOfLoginAsync(user); // 记录最后登录时间、失败次数清零


            await _authManager.AddUserStatCount(StatType.UserLogin, user);
            await _logRepository.AddUserLogAsync(user, ipAddress, Constants.ActionsLoginSuccess);
            var token = _authManager.AuthenticateUser(user, request.IsPersistent);

            var cacheKey = Constants.GetUserSessionIdCacheKey(user.Id);
            var sessionId = StringUtils.Guid();
            await _dbCacheRepository.RemoveAndInsertAsync(cacheKey, sessionId);

            await _authManager.AddPointsLogAsync(PointType.PointLogin, user, 0, string.Empty);

            return new SubmitResult
            {
                SessionId = sessionId,
                User = user,
                Token = token
            };
        }
    }
}
