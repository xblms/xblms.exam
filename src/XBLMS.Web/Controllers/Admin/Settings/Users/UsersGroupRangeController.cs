using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Settings.Users
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class UsersGroupRangeController : ControllerBase
    {
        private const string Route = "settings/usersGroupRange";
        private const string RouteOtherData = Route+"/actions/otherData";
        private const string RouteRange = Route + "/actions/range";
        private const string RouteExport = Route + "/actions/export";

        private readonly IAuthManager _authManager;
        private readonly IPathManager _pathManager;
        private readonly IDatabaseManager _databaseManager;
        private readonly IUserRepository _userRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IOrganManager _organManager;
        private readonly IExamManager _examManager;

        public UsersGroupRangeController(IAuthManager authManager, IPathManager pathManager, IDatabaseManager databaseManager, IUserRepository userRepository, IUserGroupRepository userGroupRepository, IOrganManager organManager, IExamManager examManager)
        {
            _authManager = authManager;
            _pathManager = pathManager;
            _databaseManager = databaseManager;
            _userRepository = userRepository;
            _userGroupRepository = userGroupRepository;
            _organManager = organManager;
            _examManager = examManager;
        }

        public class GetRequest
        {
            public int GroupId { get; set; }
            public int OrganId { get; set; }
            public string OrganType { get; set; }
            public int Range { get; set; }
            public string Order { get; set; }
            public int LastActivityDate { get; set; }
            public string Keyword { get; set; }
            public int Offset { get; set; }
            public int Limit { get; set; }
            public List<int> RangeUserIds { get; set; }
            public bool RangeAll { get; set; }
        }

        public class GetResults
        {
            public List<OrganTree> Organs { get; set; }
            public List<User> Users { get; set; }
            public int Count { get; set; }
            public string GroupName { get; set; }
        }

    }
}
