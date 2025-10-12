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
    public partial class PointsLogController : ControllerBase
    {
        private const string Route = "pointsLog";

        private readonly IConfigRepository _configRepository;
        private readonly IAuthManager _authManager;
        private readonly IPointLogRepository _pointLogRepository;

        public PointsLogController(IConfigRepository configRepository,
            IAuthManager authManager, IPointLogRepository pointLogRepository)
        {
            _configRepository = configRepository;
            _authManager = authManager;
            _pointLogRepository = pointLogRepository;
        }
        public class GetRequest
        {
            public string DateFrom { get; set; }
            public string DateTo { get; set; }
            public string keyWords { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetResult
        {
            public List<PointLog> List { get; set; }
            public int Total { get; set; }
        }
    }
}
