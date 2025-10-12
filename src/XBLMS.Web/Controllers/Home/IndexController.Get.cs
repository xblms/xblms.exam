using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home
{
    public partial class IndexController
    {
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var config = await _configRepository.GetAsync();
            if (config.IsHomeClosed) return this.Error("对不起，用户中心已被禁用！");

            var (redirect, redirectUrl) = await AdminRedirectCheckAsync();
            if (redirect)
            {
                return new GetResult
                {
                    Value = false,
                    RedirectUrl = redirectUrl
                };
            }

            var user = await _authManager.GetUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            var cacheKey = Constants.GetUserSessionIdCacheKey(user.Id);
            var sessionId = await _dbCacheRepository.GetValueAsync(cacheKey);
            if (string.IsNullOrEmpty(request.SessionId) || sessionId != request.SessionId)
            {
                return Unauthorized();
            }

            var pointNotice = await _authManager.PointNotice(user.Id);
            return new GetResult
            {
                SystemCodeName = config.SystemCodeName,
                SystemCode = config.SystemCode,
                Value = true,
                PointNotice = pointNotice,
                DisplayName = user.DisplayName,
                AvatarUrl = user.AvatarUrl
            };
        }
    }
}
