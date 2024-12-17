using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin
{
    [OpenApiIgnore]
    [Route(Constants.ApiAdminPrefix)]
    public partial class IndexController : ControllerBase
    {
        private const string Route = "index";
        private const string RouteSetLanguage = "index/actions/setLanguage";

        private readonly ISettingsManager _settingsManager;
        private readonly IAuthManager _authManager;
        private readonly IPathManager _pathManager;
        private readonly IConfigRepository _configRepository;
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IDbCacheRepository _dbCacheRepository;

        public IndexController(ISettingsManager settingsManager, IAuthManager authManager, IPathManager pathManager, IConfigRepository configRepository, IAdministratorRepository administratorRepository, IDbCacheRepository dbCacheRepository)
        {
            _settingsManager = settingsManager;
            _authManager = authManager;
            _pathManager = pathManager;
            _configRepository = configRepository;
            _administratorRepository = administratorRepository;
            _dbCacheRepository = dbCacheRepository;
        }

        public class Local
        {
            public int UserId { get; set; }
            public string Guid { get; set; }
            public string UserName { get; set; }
            public string DisplayName { get; set; }
            public string AvatarUrl { get; set; }
            public string Auth { get; set; }
        }

        public class GetRequest
        {
            public string SessionId { get; set; }
        }


        public class GetResult
        {
            public string Version { get; set; }
            public bool IsSafeMode { get; set; }
            public bool Value { get; set; }
            public string RedirectUrl { get; set; }
            public IList<Menu> Menus { get; set; }
            public Local Local { get; set; }
            public bool IsEnforcePasswordChange { get; set; }
        }

        public class SetLanguageRequest
        {
            public string Culture { get; set; }
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
            else if (config.Initialized &&
                     config.DatabaseVersion != _settingsManager.Version)
            {
                redirect = true;
                redirectUrl = _pathManager.GetAdminUrl(SyncDatabaseController.Route);
            }

            return (redirect, redirectUrl);
        }
    }
}
