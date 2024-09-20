using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Datory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Configuration;
using XBLMS.Core.Services;
using XBLMS.Core.Utils;
using XBLMS.Models;
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
            if (_settingsManager.Containerized)
            {
                var envSecurityKey = SettingsManager.GetEnvironmentVariable(SettingsManager.EnvSecurityKey);
                var envDatabaseType = SettingsManager.GetEnvironmentVariable(SettingsManager.EnvDatabaseType);
                var envDatabaseHost = SettingsManager.GetEnvironmentVariable(SettingsManager.EnvDatabaseHost);
                var envDatabasePort = SettingsManager.GetEnvironmentVariable(SettingsManager.EnvDatabasePort);
                var envDatabaseUser = SettingsManager.GetEnvironmentVariable(SettingsManager.EnvDatabaseUser);
                var envDatabasePassword = SettingsManager.GetEnvironmentVariable(SettingsManager.EnvDatabasePassword);
                var envDatabaseName = SettingsManager.GetEnvironmentVariable(SettingsManager.EnvDatabaseName);
                var envDatabaseConnectionString = SettingsManager.GetEnvironmentVariable(SettingsManager.EnvDatabaseConnectionString);
                var envRedisConnectionString = SettingsManager.GetEnvironmentVariable(SettingsManager.EnvRedisConnectionString);

                var isEnvironment = SettingsManager.IsEnvironment(envSecurityKey, envDatabaseType, envDatabaseConnectionString,
                    envDatabaseHost, envDatabaseUser, envDatabasePassword, envDatabaseName);
                if (!isEnvironment)
                {
                    return this.Error("系统启动失败，请检查 XBLMS 容器运行环境变量设置");
                }
            }

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
            var cacheKey = Constants.GetSessionIdCacheKey(admin.Id);
            var sessionId = await _dbCacheRepository.GetValueAsync(cacheKey);
            if (string.IsNullOrEmpty(request.SessionId) || sessionId != request.SessionId)
            {
                return Unauthorized();
            }

            var allMenus = await _authManager.GetMenus();

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


            return new GetResult
            {
                Value = true,
                Menus = allMenus,
                IsEnforcePasswordChange = isEnforcePasswordChange,
                Local = new Local
                {
                    UserId = admin.Id,
                    Guid = admin.Guid,
                    UserName = admin.UserName,
                    DisplayName = admin.DisplayName,
                    AvatarUrl = admin.AvatarUrl,
                    Auth = admin.Auth.GetDisplayName()
                }

            };
        }
    }
}
