using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Settings.Users
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class UsersCerAvatarUploadController : ControllerBase
    {
        private const string Route = "settings/usersCerAvatarUpload";

        private readonly IAuthManager _authManager;
        private readonly IUserRepository _userRepository;
        private readonly IUploadManager _uploadManager;

        public UsersCerAvatarUploadController(IAuthManager authManager, IUserRepository userRepository, IUploadManager uploadManager)
        {
            _authManager = authManager;
            _userRepository = userRepository;
            _uploadManager = uploadManager;
        }

        public class GetResult
        {
            public bool Success { get; set; }
            public string Msg { get; set; }
        }

    }
}
