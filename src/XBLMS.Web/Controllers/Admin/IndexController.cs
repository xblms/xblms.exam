using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin
{
    [OpenApiIgnore]
    [Route(Constants.ApiAdminPrefix)]
    public partial class IndexController : ControllerBase
    {
        private const string Route = "index";
        private const string RouteChangeAuthDataShowAll = Route + "/changeShowall";
        private const string RouteChangeOrgan = Route + "/changeOrgan";

        private const string RouteSetLanguage = "index/actions/setLanguage";

        private readonly ISettingsManager _settingsManager;
        private readonly IAuthManager _authManager;
        private readonly IConfigRepository _configRepository;
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IDbCacheRepository _dbCacheRepository;
        private readonly IOrganCompanyRepository _organCompanyRepository;

        public IndexController(ISettingsManager settingsManager,
            IAuthManager authManager,
            IConfigRepository configRepository,
            IAdministratorRepository administratorRepository,
            IDbCacheRepository dbCacheRepository,
            IOrganCompanyRepository organCompanyRepository)
        {
            _settingsManager = settingsManager;
            _authManager = authManager;
            _configRepository = configRepository;
            _administratorRepository = administratorRepository;
            _dbCacheRepository = dbCacheRepository;
            _organCompanyRepository = organCompanyRepository;
        }

        public class Local
        {
            public int UserId { get; set; }
            public string Guid { get; set; }
            public string UserName { get; set; }
            public string DisplayName { get; set; }
            public string AvatarUrl { get; set; }
            public string Auth { get; set; }
            public string AuthCurrentOrganName { get; set; }
            public bool AuthDataShowAll { get; set; }
            public bool AuthOrganChange { get; set; }
        }

        public class GetRequest
        {
            public string SessionId { get; set; }
        }
        public class GetResult
        {
            public InstallCheckResult InstallCheck { get; set; }
            public string Version { get; set; }
            public string VersionName { get; set; }
            public bool IsSafeMode { get; set; }
            public bool Value { get; set; }
            public string RedirectUrl { get; set; }
            public IList<Menu> Menus { get; set; }
            public Local Local { get; set; }
            public int AdminEnforceLogoutMinutes { get; set; }
            public bool IsEnforcePasswordChange { get; set; }
            public string SystemCodeName { get; set; }
        }

        public class GetChangeAuthDataShowAllRequest
        {
            public bool AuthDataShowAll { get; set; }
        }

        public class ChangeOrganRequest
        {
            public int OrganId { get; set; }
        }
        public class SetLanguageRequest
        {
            public string Culture { get; set; }
        }
    }
}
