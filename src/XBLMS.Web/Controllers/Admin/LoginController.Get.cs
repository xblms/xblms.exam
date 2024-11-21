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
            var redirectUrl = await AdminRedirectCheckAsync();
            if (!string.IsNullOrEmpty(redirectUrl))
            {
                return new GetResult
                {
                    Success = false,
                    RedirectUrl = redirectUrl
                };
            }

            var config = await _configRepository.GetAsync();

            return new GetResult
            {
                Success = true,
                IsAdminCaptchaDisabled = config.IsAdminCaptchaDisabled,
            };
        }
    }
}
