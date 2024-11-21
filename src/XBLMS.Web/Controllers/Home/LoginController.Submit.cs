using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Configuration;
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
            var ipAddress = PageUtils.GetIpAddress(Request);
            string userName;
            string errorMessage;
            (user, userName, errorMessage) = await _userRepository.ValidateAsync(request.Account, request.Password, true);

            if (user == null)
            {
                user = await _userRepository.GetByUserNameAsync(userName);
                if (user != null)
                {
                    await _logRepository.AddUserLogAsync(user, ipAddress, Constants.ActionsLoginFailure, "帐号或密码错误");
                }
                return this.Error(errorMessage);
            }

            user = await _userRepository.GetByUserNameAsync(userName);

            await _userRepository.UpdateLastActivityDateAndCountOfLoginAsync(user); // 记录最后登录时间、失败次数清零

            await _statRepository.AddCountAsync(StatType.UserLogin);
            await _logRepository.AddUserLogAsync(user, ipAddress, Constants.ActionsLoginSuccess);
            var token = _authManager.AuthenticateUser(user, request.IsPersistent);


            return new SubmitResult
            {
                User = user,
                Token = token
            };
        }
    }
}
