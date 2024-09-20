using Microsoft.AspNetCore.Mvc;
using XBLMS.Core.Utils;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home
{
    public partial class LoginController
    {
        [HttpGet, Route(RouteCaptcha)]
        public FileResult CaptchaGet([FromQuery] string token)
        {
            var captcha = TranslateUtils.JsonDeserialize<CaptchaUtils.Captcha>(_settingsManager.Decrypt(token));

            var bytes = CaptchaUtils.GetCaptcha(captcha.Value);

            return File(bytes, "image/png");
        }
    }
}
