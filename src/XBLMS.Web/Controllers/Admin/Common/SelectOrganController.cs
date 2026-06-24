using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Dto;
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
        private readonly IOrganManager _organManager;

        public SelectOrganController(IAuthManager authManager,IOrganManager organManager)
        {
            _authManager = authManager;
            _organManager = organManager;
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
