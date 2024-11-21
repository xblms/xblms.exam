using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Common
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class AdminLayerViewController : ControllerBase
    {
        private const string Route = "common/adminLayerView";

        private readonly IHttpContextAccessor _context;
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IOrganManager _organManager;

        public AdminLayerViewController(IHttpContextAccessor context, IAdministratorRepository administratorRepository, IOrganManager organManager)
        {
            _context = context;
            _administratorRepository = administratorRepository;
            _organManager = organManager;
        }

        public class GetResult
        {
            public Administrator Administrator { get; set; }
        }
    }
}
