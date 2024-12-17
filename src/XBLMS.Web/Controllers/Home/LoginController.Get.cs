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


            return new GetResult
            {
                Version = _settingsManager.Version,
                IsUserCaptchaDisabled = config.IsUserCaptchaDisabled
            };
        }
    }
}
