using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Dto;

namespace XBLMS.Web.Controllers.Home
{
    public partial class IndexController
    {
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet, Route(RouteSession)]
        public async Task<ActionResult<BoolResult>> RefreSession([FromQuery] GetRequest request)
        {
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

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
