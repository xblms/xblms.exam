using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Web.Controllers.Admin;

namespace XBLMS.Web.Controllers.Home
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.User)]
    [Route(Constants.ApiHomePrefix)]
    public partial class IndexController : ControllerBase
    {
        private const string Route = "index";
        private const string RouteSession = Route + "/resses";

        private readonly ISettingsManager _settingsManager;
        private readonly IAuthManager _authManager;
        private readonly IConfigRepository _configRepository;
        private readonly IDbCacheRepository _dbCacheRepository;
        private readonly IPathManager _pathManager;

        public IndexController(IAuthManager authManager,
            ISettingsManager settingsManager,
            IConfigRepository configRepository,
            IDbCacheRepository dbCacheRepository,
            IPathManager pathManager)
        {
            _settingsManager = settingsManager;
            _authManager = authManager;
            _configRepository = configRepository;
            _dbCacheRepository = dbCacheRepository;
            _pathManager = pathManager;
        }
        public class GetRequest
        {
            public string SessionId { get; set; }
        }
        public class GetResult
        {
            public string SystemCodeName { get; set; }
            public SystemCode SystemCode { get; set; }
            public PointNotice PointNotice { get; set; }
            public string DisplayName { get; set; }
            public string AvatarUrl { get; set; }
            public bool Value { get; set; }
            public string RedirectUrl { get; set; }
        }

        private async Task<(bool redirect, string redirectUrl)> AdminRedirectCheckAsync()
        {
            var redirect = false;
            var redirectUrl = string.Empty;

            var config = await _configRepository.GetAsync();

            if (string.IsNullOrEmpty(_settingsManager.Database.ConnectionString) || await _configRepository.IsNeedInstallAsync())
            {
                redirect = true;
                redirectUrl = _pathManager.GetAdminUrl(InstallController.Route);
            }
            else if (config.Initialized && config.DatabaseVersion != _settingsManager.Version)
            {
                redirect = true;
                redirectUrl = _pathManager.GetAdminUrl(SyncDatabaseController.Route);
            }

            return (redirect, redirectUrl);
        }
    }
}
