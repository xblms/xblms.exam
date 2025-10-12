using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public partial class UsersLayerProfileController : ControllerBase
    {
        private const string Route = "settings/usersLayerProfile";
        private const string RouteUpload = "settings/usersLayerProfile/actions/upload";

        private readonly IAuthManager _authManager;
        private readonly IPathManager _pathManager;
        private readonly IUserRepository _userRepository;
        private readonly IOrganManager _organManager;
        private readonly IUploadManager _uploadManager;

        public UsersLayerProfileController(IAuthManager authManager, IPathManager pathManager, IOrganManager organManager, IUserRepository userRepository, IUploadManager uploadManager)
        {
            _authManager = authManager;
            _pathManager = pathManager;
            _organManager = organManager;
            _userRepository = userRepository;
            _uploadManager = uploadManager;
        }

        public class GetResult
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string DutyName { get; set; }
            public string DisplayName { get; set; }
            public string AvatarUrl { get; set; }
            public string Mobile { get; set; }
            public string Email { get; set; }
            public int OrganId { get; set; }
            public string OrganType { get; set; }
            public string OrganName { get; set; }
            public bool Locked { get; set; }
        }
        public class SubmitRequest
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string DutyName { get; set; }
            public string DisplayName { get; set; }
            public string Password { get; set; }
            public string AvatarUrl { get; set; }
            public string Mobile { get; set; }
            public string Email { get; set; }
            public int OrganId { get; set; }
            public string OrganType { get; set; }
            public bool Locked { get; set; }
        }
        public class UploadRequest
        {
            public int UserId { get; set; }
            public IFormFile File { set; get; }
        }
    }
}
