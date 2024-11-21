using Datory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Home
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.User)]
    [Route(Constants.ApiHomePrefix)]
    public partial class ProfileController : ControllerBase
    {
        private const string Route = "profile";
        private const string RouteUpload = "profile/actions/upload";
        private const string RouteSendSms = "profile/actions/sendSms";
        private const string RouteVerifyMobile = "profile/actions/verifyMobile";

        private readonly IAuthManager _authManager;
        private readonly IPathManager _pathManager;
        private readonly ICacheManager _cacheManager;
        private readonly IConfigRepository _configRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUploadManager _uploadManager;

        public ProfileController(IAuthManager authManager, IPathManager pathManager, ICacheManager cacheManager, IConfigRepository configRepository, IUserRepository userRepository, IUploadManager uploadManager)
        {
            _authManager = authManager;
            _pathManager = pathManager;
            _cacheManager = cacheManager;
            _configRepository = configRepository;
            _userRepository = userRepository;
            _uploadManager = uploadManager;
        }


        public class GetResult
        {
            public Entity Entity { get; set; }
        }
    }
}
