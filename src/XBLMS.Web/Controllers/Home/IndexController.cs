using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Home
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.User)]
    [Route(Constants.ApiHomePrefix)]
    public partial class IndexController : ControllerBase
    {
        private const string Route = "index";
        private const string RouteSession = Route + "/resses";

        private readonly IAuthManager _authManager;
        private readonly IConfigRepository _configRepository;
        private readonly IDbCacheRepository _dbCacheRepository;

        public IndexController(IAuthManager authManager,
            IConfigRepository configRepository,
            IDbCacheRepository dbCacheRepository)
        {
            _authManager = authManager;
            _configRepository = configRepository;
            _dbCacheRepository = dbCacheRepository;
        }
        public class GetRequest
        {
            public string SessionId { get; set; }
        }
        public class GetResult
        {
            public InstallCheckResult InstallCheck { get; set; }
            public string SystemCodeName { get; set; }
            public SystemCode SystemCode { get; set; }
            public PointNotice PointNotice { get; set; }
            public string DisplayName { get; set; }
            public string AvatarUrl { get; set; }
            public bool Value { get; set; }
            public string RedirectUrl { get; set; }
        }
    }
}
