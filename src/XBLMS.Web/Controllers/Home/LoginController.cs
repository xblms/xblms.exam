using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Home
{
    [OpenApiIgnore]
    [Route(Constants.ApiHomePrefix)]
    public partial class LoginController : ControllerBase
    {
        private const string Route = "login";
        private const string RouteCaptcha = "login/captcha";

        private readonly ISettingsManager _settingsManager;
        private readonly IAuthManager _authManager;
        private readonly ICacheManager _cacheManager;
        private readonly IConfigRepository _configRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogRepository _logRepository;
        private readonly IDbCacheRepository _dbCacheRepository;

        public LoginController(ISettingsManager settingsManager,
            IAuthManager authManager, ICacheManager cacheManager,
            IConfigRepository configRepository,
            IUserRepository userRepository,
            ILogRepository logRepository,
            IDbCacheRepository dbCacheRepository)
        {
            _settingsManager = settingsManager;
            _authManager = authManager;
            _cacheManager = cacheManager;
            _configRepository = configRepository;
            _userRepository = userRepository;
            _logRepository = logRepository;
            _dbCacheRepository = dbCacheRepository;
        }

        public class GetResult
        {
            public InstallCheckResult InstallCheck { get; set; }
            public string Version { get; set; }
            public string VersionName { get; set; }
            public bool IsUserCaptchaDisabled { get; set; }
            public string SystemCodeName { get; set; }
        }

        public class SubmitRequest
        {
            public string Account { get; set; }
            public string Password { get; set; }
            public bool IsPersistent { get; set; }
            public string Token { get; set; }
            public string Value { get; set; }
        }

        public class SubmitResult
        {
            public string SessionId { get; set; }
            public User User { get; set; }
            public string Token { get; set; }
        }
    }
}
