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
    public partial class CrudDemoController : ControllerBase
    {
        private const string Route = "xbl/crudDemo";
        private const string RouteDelete = Route + "/actions/delete";

        private readonly IAuthManager _authManager;
        private readonly ICrudDemoRepository _crudDemoRepository;
        private readonly IAdministratorsInRolesRepository _administratorsInRolesRepository;
        private readonly IAdministratorRepository _administratorRepository;

        public CrudDemoController(IAuthManager authManager, ICrudDemoRepository crudDemoRepository, IAdministratorRepository administratorRepository, IAdministratorsInRolesRepository administratorsInRolesRepository)
        {
            _authManager = authManager;
            _crudDemoRepository = crudDemoRepository;
            _administratorRepository = administratorRepository;
            _administratorsInRolesRepository = administratorsInRolesRepository;
        }

        public class GetRequest
        {
            public string Title { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetResult
        {
            public int Total { get; set; }
            public List<CrudDemo> List { get; set; }
        }
    }
}
