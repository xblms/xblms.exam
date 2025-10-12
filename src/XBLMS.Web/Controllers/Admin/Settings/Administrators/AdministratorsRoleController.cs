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
    public partial class AdministratorsRoleController : ControllerBase
    {
        private const string Route = "settings/administratorsRole";
        private const string RouteDelete = "settings/administratorsRole/actions/delete";

        private readonly IAuthManager _authManager;
        private readonly IRoleRepository _roleRepository;
        private readonly IAdministratorsInRolesRepository _administratorsInRolesRepository;
        private readonly IAdministratorRepository _administratorRepository;

        public AdministratorsRoleController(IAuthManager authManager, IRoleRepository roleRepository, IAdministratorRepository administratorRepository, IAdministratorsInRolesRepository administratorsInRolesRepository)
        {
            _authManager = authManager;
            _roleRepository = roleRepository;
            _administratorRepository = administratorRepository;
            _administratorsInRolesRepository = administratorsInRolesRepository;
        }
        public class GetRequest
        {
            public string KeyWords { get; set; }
        }
        public class ListRequest
        {
            public List<Role> Roles { get; set; }
        }
    }
}
