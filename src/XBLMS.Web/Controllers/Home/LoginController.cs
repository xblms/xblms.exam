using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
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
        private readonly IStatRepository _statRepository;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamPaperUserRepository _examPaperUserRepository;
        private readonly IExamPaperStartRepository _examPaperStartRepository;
        private readonly IExamQuestionnaireUserRepository _examQuestionnaireUserRepository;
        private readonly IExamAssessmentUserRepository _examAssessmentUserRepository;
        private readonly IExamQuestionnaireRepository _examQuestionnaireRepository;
        private readonly IExamAssessmentRepository _examAssessmentRepository;
        private readonly IDbCacheRepository _dbCacheRepository;

        public LoginController(ISettingsManager settingsManager,
            IAuthManager authManager, ICacheManager cacheManager,
            IConfigRepository configRepository,
            IUserRepository userRepository,
            ILogRepository logRepository,
            IStatRepository statRepository,
            IExamPaperRepository examPaperRepository,
            IExamPaperUserRepository examPaperUserRepository,
            IExamPaperStartRepository examPaperStartRepository,
            IExamQuestionnaireUserRepository examQuestionnaireUserRepository,
            IExamAssessmentUserRepository examAssessmentUserRepository,
            IExamQuestionnaireRepository examQuestionnaireRepository,
            IExamAssessmentRepository examAssessmentRepository, IDbCacheRepository dbCacheRepository)
        {
            _settingsManager = settingsManager;
            _authManager = authManager;
            _cacheManager = cacheManager;
            _configRepository = configRepository;
            _userRepository = userRepository;
            _logRepository = logRepository;
            _statRepository = statRepository;
            _examPaperRepository = examPaperRepository;
            _examPaperUserRepository = examPaperUserRepository;
            _examPaperStartRepository = examPaperStartRepository;
            _examQuestionnaireUserRepository = examQuestionnaireUserRepository;
            _examAssessmentUserRepository = examAssessmentUserRepository;
            _examQuestionnaireRepository = examQuestionnaireRepository;
            _examAssessmentRepository = examAssessmentRepository;
            _dbCacheRepository = dbCacheRepository;
        }

        public class GetResult
        {
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
