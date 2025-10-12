using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Home.Gift
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.User)]
    [Route(Constants.ApiHomePrefix)]
    public partial class PointShopLogController : ControllerBase
    {
        private const string Route = "pointShopLog";

        private readonly IConfigRepository _configRepository;
        private readonly IAuthManager _authManager;
        private readonly IPointShopUserRepository _pointShopUserRepository;

        public PointShopLogController(IConfigRepository configRepository,
            IAuthManager authManager,
            IPointShopUserRepository pointShopUserRepository)
        {
            _configRepository = configRepository;
            _authManager = authManager;
            _pointShopUserRepository = pointShopUserRepository;
        }
        public class GetRequest
        {
            public string DateFrom { get; set; }
            public string DateTo { get; set; }
            public string Keywords { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetResult
        {
            public List<PointShopUser> List { get; set; }
            public int Total { get; set; }
        }
    }
}
