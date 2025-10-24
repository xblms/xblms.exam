using Datory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin
{
    public partial class IndexController
    {
        [HttpGet, Route(Route)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var allowed = PageUtils.IsVisitAllowed(_settingsManager, Request);
            if (!allowed)
            {
                return this.Error($"访问已被禁止，IP地址：{PageUtils.GetIpAddress(Request)}，请与网站管理员联系开通访问权限");
            }

            var (redirect, redirectUrl) = await AdminRedirectCheckAsync();
            if (redirect)
            {
                return new GetResult
                {
                    Value = false,
                    RedirectUrl = redirectUrl
                };
            }

            var admin = await _authManager.GetAdminAsync();
            if (admin == null)
            {
                return Unauthorized();
            }
            var adminAuth = await _authManager.GetAdminAuth();

            var cacheKey = Constants.GetSessionIdCacheKey(admin.Id);
            var sessionId = await _dbCacheRepository.GetValueAsync(cacheKey);
            if (string.IsNullOrEmpty(request.SessionId) || sessionId != request.SessionId)
            {
                return Unauthorized();
            }

            var allMenus = await _authManager.GetMenus();
            if (allMenus == null || allMenus.Count == 0) { return this.Error($"无权限访问，请与管理员联系开通访问权限"); }

            var config = await _configRepository.GetAsync();

            var isEnforcePasswordChange = false;
            if (config.IsAdminEnforcePasswordChange)
            {
                if (admin.LastChangePasswordDate == null)
                {
                    isEnforcePasswordChange = true;
                }
                else
                {
                    var ts = new TimeSpan(DateTime.Now.Ticks - admin.LastChangePasswordDate.Value.Ticks);
                    if (ts.TotalDays > config.AdminEnforcePasswordChangeDays)
                    {
                        isEnforcePasswordChange = true;
                    }
                }
            }

            var version = _settingsManager.Version;

            var curOrganName = string.Empty;
            if (admin.AuthDataCurrentOrganId > 0)
            {
                var company = await _organCompanyRepository.GetAsync(admin.AuthDataCurrentOrganId);
                if (company != null)
                {
                    curOrganName = company.Name;
                }
            }

            var adminEnforceLogoutMinutes = config.IsAdminEnforceLogout && config.AdminEnforceLogoutMinutes > 0 ? config.AdminEnforceLogoutMinutes : 0;

            await AddPingTask();

            return new GetResult
            {
                Version = version,
                VersionName = _settingsManager.VersionName,
                SystemCodeName = config.SystemCodeName,
                IsSafeMode = _settingsManager.IsSafeMode,
                Value = true,
                Menus = allMenus,
                IsEnforcePasswordChange = isEnforcePasswordChange,
                AdminEnforceLogoutMinutes = adminEnforceLogoutMinutes,
                Local = new Local
                {
                    UserId = admin.Id,
                    Guid = admin.Guid,
                    UserName = admin.UserName,
                    DisplayName = admin.DisplayName,
                    AvatarUrl = admin.AvatarUrl,
                    Auth = admin.Auth.GetDisplayName(),
                    AuthCurrentOrganName = curOrganName,
                    AuthDataShowAll = admin.AuthDataShowAll,
                    AuthOrganChange = adminAuth.AuthDataType != Enums.AuthorityDataType.DataCreator
                }
            };
        }
    }
}
