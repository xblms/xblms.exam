using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Settings.Administrators
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class CachDemoController : ControllerBase
    {
        private const string Route = "xbl/cacheDemo";

        private readonly IAuthManager _authManager;
        private readonly ICacheManager _cacheManager;

        public CachDemoController(IAuthManager authManager, ICacheManager cacheManager)
        {
            _authManager = authManager;
            _cacheManager = cacheManager;
        }

        public class GetRequest
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }
        public class GetResult
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var admin = await _authManager.GetAdminAsync();
            var cache = _cacheManager.Get<string>(request.Key);

            return new GetResult
            {
                Value = cache
            };
        }
        [HttpPost, Route(Route)]
        public async Task<ActionResult<BoolResult>> Set([FromBody] GetRequest request)
        {
            var admin = await _authManager.GetAdminAsync();
            _cacheManager.AddOrUpdate(request.Key, request.Value);

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
