using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Settings.Users
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class UsersGroupUserListController : ControllerBase
    {
        private const string Route = "settings/usersGroupUserList";

        private readonly IAuthManager _authManager;
        private readonly IUserRepository _userRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IOrganManager _organManager;

        public UsersGroupUserListController(IAuthManager authManager, IUserRepository userRepository, IUserGroupRepository userGroupRepository, IOrganManager organManager)
        {
            _authManager = authManager;
            _userRepository = userRepository;
            _userGroupRepository = userGroupRepository;
            _organManager = organManager;
        }

        public class GetRequest
        {
            public int OrganId { get; set; }
            public string OrganType { get; set; }
            public int GroupId { get; set; }
            public string Order { get; set; }
            public int LastActivityDate { get; set; }
            public string Keyword { get; set; }
            public int Offset { get; set; }
            public int Limit { get; set; }
        }

        public class GetResults
        {
            public List<User> Users { get; set; }
            public int Count { get; set; }
        }

    }
}
