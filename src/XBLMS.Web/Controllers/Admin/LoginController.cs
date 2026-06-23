using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin
{
    [OpenApiIgnore]
    [Route(Constants.ApiAdminPrefix)]
    public partial class LoginController : ControllerBase
    {
        public const string Route = "login";
        private const string RouteCaptcha = "login/captcha";

        private readonly ISettingsManager _settingsManager;
        private readonly IAuthManager _authManager;
        private readonly ICacheManager _cacheManager;
        private readonly IConfigRepository _configRepository;
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IDbCacheRepository _dbCacheRepository;
        private readonly ILogRepository _logRepository;

        public LoginController(ISettingsManager settingsManager,
            IAuthManager authManager,
            ICacheManager cacheManager,
            IConfigRepository configRepository,
            IAdministratorRepository administratorRepository,
            IDbCacheRepository dbCacheRepository,
            ILogRepository logRepository)
        {
            _settingsManager = settingsManager;
            _authManager = authManager;
            _cacheManager = cacheManager;
            _configRepository = configRepository;
            _administratorRepository = administratorRepository;
            _dbCacheRepository = dbCacheRepository;
            _logRepository = logRepository;
        }

        public class GetResult
        {
            public InstallCheckResult InstallCheck { get; set; }
            public bool Success { get; set; }
            public string Version { get; set; }
            public string VersionName { get; set; }
            public string RedirectUrl { get; set; }
            public bool IsAdminCaptchaDisabled { get; set; }
            public string SystemCodeName { get; set; }
        }

        public class SubmitRequest
        {
            public string Account { get; set; }
            public string Password { get; set; }
            public bool IsPersistent { get; set; }
            public bool IsForceLogoutAndLogin {get;set;}
            public string Token { get; set; }
            public string Value { get; set; }
        }

        public class SubmitResult
        {
            public bool IsLoginExists { get; set; }
            public Administrator Administrator { get; set; }
            public string SessionId { get; set; }
            public string Token { get; set; }
        }
    }
}
