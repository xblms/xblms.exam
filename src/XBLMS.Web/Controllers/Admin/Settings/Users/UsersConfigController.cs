using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Settings.Users
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class UsersConfigController : ControllerBase
    {
        private const string Route = "settings/usersConfig";

        private readonly IAuthManager _authManager;
        private readonly IConfigRepository _configRepository;

        public UsersConfigController(IAuthManager authManager, IConfigRepository configRepository)
        {
            _authManager = authManager;
            _configRepository = configRepository;
        }

        public class GetResult
        {
            public Config Config { get; set; }
        }

        public class SubmitRequest
        {
            public bool IsHomeClosed { get; set; }
            public int UserPasswordMinLength { get; set; }
            public PasswordRestriction UserPasswordRestriction { get; set; }
            public bool IsUserLockLogin { get; set; }
            public int UserLockLoginCount { get; set; }
            public string UserLockLoginType { get; set; }
            public int UserLockLoginHours { get; set; }
            public bool IsUserCaptchaDisabled { get; set; }
        }
    }
}
