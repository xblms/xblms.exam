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
    public partial class CrudDemoEditController : ControllerBase
    {
        private const string Route = "xbl/crudDemoEdit";
        private const string RouteUpdate = Route+"/actions/update";
        private const string RouteAdd = Route + "/actions/add";

        private readonly ICacheManager _cacheManager;
        private readonly ISettingsManager _settingsManager;
        private readonly IAuthManager _authManager;
        private readonly ICrudDemoRepository _crudDemoRepository;
        private readonly IAdministratorsInRolesRepository _administratorsInRolesRepository;
        private readonly IAdministratorRepository _administratorRepository;

        public CrudDemoEditController(ICacheManager cacheManager, ISettingsManager settingsManager, IAuthManager authManager, ICrudDemoRepository crudDemoRepository, IAdministratorsInRolesRepository administratorsInRolesRepository, IAdministratorRepository administratorRepository)
        {
            _cacheManager = cacheManager;
            _settingsManager = settingsManager;
            _authManager = authManager;
            _administratorsInRolesRepository = administratorsInRolesRepository;
            _crudDemoRepository = crudDemoRepository;
            _administratorRepository = administratorRepository;
        }

        public class Option
        {
            public string Name { get; set; }

            public string Text { get; set; }

            public bool Selected { get; set; }
        }

        public class GetRequest
        {
            public string Title { get; set; }
            public int PageInex { get; set; }
            public int PageSize { get; set; }
        }

        public class GetResult
        {
            public CrudDemo Info { get; set; }
        }

        public class SubmitRequest
        {
            public CrudDemo Info { get; set; }
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
