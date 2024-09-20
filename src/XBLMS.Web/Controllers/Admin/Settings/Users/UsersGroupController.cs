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
    public partial class UsersGroupController : ControllerBase
    {
        private const string Route = "settings/usersGroup";
        private const string RouteDelete = "settings/usersGroup/actions/delete";

        private const string RouteEditGet = "settings/usersGroup/actions/editGet";
        private const string RouteEditPost = "settings/usersGroup/actions/editPost";

        private readonly IAuthManager _authManager;
        private readonly ICacheManager _cacheManager;
        private readonly IConfigRepository _configRepository;
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IOrganManager _organManager;
        private readonly IUserRepository _userRepository;

        public UsersGroupController(IAuthManager authManager, ICacheManager cacheManager, IConfigRepository configRepository, IAdministratorRepository administratorRepository, IUserGroupRepository userGroupRepository, IOrganManager organManager, IUserRepository userRepository)
        {
            _authManager = authManager;
            _cacheManager = cacheManager;
            _configRepository = configRepository;
            _administratorRepository = administratorRepository;
            _userGroupRepository = userGroupRepository;
            _organManager = organManager;
            _userRepository = userRepository;
        }
        public class GetRequest
        {
            public string Search { get; set; }
        }
        public class GetResult
        {
            public IEnumerable<UserGroup> Groups { get; set; }
        }


        public class GetEditResult
        {
            public UserGroup Group { get; set; }
            public List<OrganTree> Organs { get; set; }
            public List<Select<string>> GroupTypeSelects { get; set; }
            public List<string> SelectOrganIds { get; set; }
        }

        public class GetEditRequest
        {
            public UserGroup Group { get; set; }
            public List<SelectOrgans> SelectOrgans { get; set; }
        }

        public class SelectOrgans
        {
            public int Id { get; set; }
            public string Type { get; set; }
        }
    }
}
