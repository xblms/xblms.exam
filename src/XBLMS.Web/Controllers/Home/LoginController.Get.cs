using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home
{
    public partial class LoginController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get()
        {
            var config = await _configRepository.GetAsync();
            if (config.IsHomeClosed) return this.Error("用户中心已被禁用！");
            var installCheck = await _authManager.InstallRedirectCheckAsync();

            return new GetResult
            {
                InstallCheck = installCheck,
                Version = _settingsManager.Version,
                VersionName = _settingsManager.VersionName,
                IsUserCaptchaDisabled = config.IsUserCaptchaDisabled,
                SystemCodeName = config.SystemCodeName
            };
        }
    }
}
