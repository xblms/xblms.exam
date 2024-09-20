using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home
{
    public partial class BlockController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<QueryResult>> Query([FromQuery] QueryRequest request)
        {
            if (await _configRepository.IsNeedInstallAsync())
            {
                return new QueryResult
                {
                    IsAllowed = true
                };
            }
            var ipAddress = PageUtils.GetIpAddress(Request);
            var (isBlocked, rule) = await _blockManager.IsBlockedAsync(ipAddress, request.SessionId);

            var blockMethod = BlockMethod.RedirectUrl;
            var redirectUrl = string.Empty;
            var warning = string.Empty;
            if (isBlocked)
            {
                blockMethod = rule.BlockMethod;
                redirectUrl = rule.RedirectUrl;
                warning = rule.Warning;
            }

            return new QueryResult
            {
                IsAllowed = !isBlocked,
                BlockMethod = blockMethod,
                RedirectUrl = redirectUrl,
                Warning = warning
            };
        }
    }
}
