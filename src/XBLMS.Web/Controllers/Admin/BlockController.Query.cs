using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin
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
            var (isBlocked, rule) = await _blockManager.IsBlockedAsync(ipAddress, request.SessionId, 2);

            var blockMethod = BlockMethod.RedirectUrl;
            var redirectUrl = string.Empty;
            var warning = string.Empty;
            if (isBlocked)
            {
                blockMethod = rule.BlockMethod;
                redirectUrl = rule.RedirectUrl;
                warning = rule.Warning;

                try
                {
                    var admin = await _administratorRepository.GetByUserIdAsync(1);
                    await _logRepository.AddAdminLogAsync(admin, PageUtils.GetIpAddress(Request), Constants.ActionsLoginFailure, "登录被拦截");
                }
                catch { }
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
