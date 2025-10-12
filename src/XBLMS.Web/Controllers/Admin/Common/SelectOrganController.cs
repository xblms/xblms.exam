using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Common
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class SelectOrganController : ControllerBase
    {
        private const string Route = "common/selectOrgan";
        private const string RouteChange = "common/selectOrganChange";

        private readonly IAuthManager _authManager;
        private readonly ICacheManager _cacheManager;
        private readonly IConfigRepository _configRepository;
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IOrganManager _organManager;

        public SelectOrganController(IAuthManager authManager, ICacheManager cacheManager, IConfigRepository configRepository, IAdministratorRepository administratorRepository, IUserGroupRepository userGroupRepository, IOrganManager organManager, IUserRepository userRepository)
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
            public int ParentId { get; set; }
            public string KeyWords { get; set; }
        }
        public class GetResult
        {
            public IEnumerable<OrganTree> Organs { get; set; }
        }
    }
}
