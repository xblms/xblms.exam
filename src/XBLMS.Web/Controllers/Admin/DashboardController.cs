using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class DashboardController : ControllerBase
    {
        private const string Route = "dashboard";

        private readonly IAuthManager _authManager;
        private readonly IOrganManager _organManager;

        public DashboardController(IAuthManager authManager, IOrganManager organManager)
        {
            _authManager = authManager;
            _organManager = organManager;
        }

        public class GetResult
        {
            public Administrator Administrator { get; set; }
        }
    }
}
