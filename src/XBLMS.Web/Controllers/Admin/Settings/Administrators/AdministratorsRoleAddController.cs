using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Settings.Administrators
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class AdministratorsRoleAddController : ControllerBase
    {
        private const string Route = "settings/administratorsRoleAdd";
        private const string RouteUpdate = "settings/administratorsRoleAdd/actions/update";
        private const string RouteSetRole = "settings/administratorsRoleAdd/actions/setRole";

        private readonly ICacheManager _cacheManager;
        private readonly ISettingsManager _settingsManager;
        private readonly IAuthManager _authManager;
        private readonly IRoleRepository _roleRepository;
        private readonly IAdministratorsInRolesRepository _administratorsInRolesRepository;
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IConfigRepository _configRepository;

        public AdministratorsRoleAddController(ICacheManager cacheManager, ISettingsManager settingsManager, IAuthManager authManager, IRoleRepository roleRepository, IAdministratorsInRolesRepository administratorsInRolesRepository, IAdministratorRepository administratorRepository, IConfigRepository configRepository)
        {
            _cacheManager = cacheManager;
            _settingsManager = settingsManager;
            _authManager = authManager;
            _administratorsInRolesRepository = administratorsInRolesRepository;
            _roleRepository = roleRepository;
            _administratorRepository = administratorRepository;
            _configRepository = configRepository;
        }

        public class GetRequest
        {
            public int RoleId { get; set; }
        }

        public class GetResult
        {
            public Role Role { get; set; }
            public List<Menu> Menus { get; set; }
        }

        public class SubmitRequest
        {
            public int RoleId { get; set; }
            public string RoleName { get; set; }
            public string Description { get; set; }
            public List<Menu> Menus { get; set; }
            public List<string> SelectIds { get; set; }
        }

        public class SetRoleRequest
        {
            public List<int> RoleIds { get; set; }
            public List<int> AdminIds { get; set; }
        }
        public class GetPermissionsResultRole
        {
            public int Key { get; set; }
            public string label { get; set; }
        }
        public class GetPermissionsResult
        {
            public List<GetPermissionsResultRole> Roles { get; set; }
            public List<int> CheckedRoles { get; set; }
        }
    }
}
