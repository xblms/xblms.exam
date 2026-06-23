using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace XBLMS.Web.Controllers.Admin
{
    public partial class LoginController
    {
        [HttpGet, Route(Route)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetResult>> Get()
        {
            var config = await _configRepository.GetAsync();
            var installCheck = await _authManager.InstallRedirectCheckAsync();
            return new GetResult
            {
                InstallCheck = installCheck,
                Success = true,
                Version = _settingsManager.Version,
                VersionName = _settingsManager.VersionName,
                IsAdminCaptchaDisabled = config.IsAdminCaptchaDisabled,
                SystemCodeName = config.SystemCodeName
            };
        }
    }
}
