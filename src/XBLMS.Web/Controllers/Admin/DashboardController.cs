using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class DashboardController : ControllerBase
    {
        private const string Route = "dashboard";

        private readonly IHttpContextAccessor _context;
        private readonly IAntiforgery _antiforgery;
        private readonly ICacheManager _cacheManager;
        private readonly ISettingsManager _settingsManager;
        private readonly IDatabaseManager _databaseManager;
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IAuthManager _authManager;
        private readonly IOrganManager _organManager;

        public DashboardController(IHttpContextAccessor context, IAuthManager authManager, IAntiforgery antiforgery, ICacheManager cacheManager, ISettingsManager settingsManager, IDatabaseManager databaseManager, IAdministratorRepository administratorRepository, IOrganManager organManager)
        {
            _context = context;
            _authManager = authManager;
            _antiforgery = antiforgery;
            _cacheManager = cacheManager;
            _settingsManager = settingsManager;
            _databaseManager = databaseManager;
            _administratorRepository = administratorRepository;
            _organManager = organManager;
        }

        public class GetResult
        {
            public Administrator Administrator { get; set; }
        }
    }
}
