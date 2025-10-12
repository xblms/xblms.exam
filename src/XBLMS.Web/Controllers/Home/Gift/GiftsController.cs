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
    public partial class GiftsController : ControllerBase
    {
        private const string Route = "gifts";

        private readonly IConfigRepository _configRepository;
        private readonly IAuthManager _authManager;
        private readonly IPointShopRepository _pointShopRepository;

        public GiftsController(IConfigRepository configRepository,
            IAuthManager authManager, IPointShopRepository pointShopRepository)
        {
            _configRepository = configRepository;
            _authManager = authManager;
            _pointShopRepository = pointShopRepository;
        }
        public class GetRequest
        {
            public int Point { get; set; }
            public string Keywords { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetResult
        {
            public List<PointShop> List { get; set; }
            public int Total { get; set; }
        }
    }
}
