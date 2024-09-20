using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace XBLMS.Web.Controllers
{
    public partial class PingController
    {
        [OpenApiOperation("Ping 可用性 API", "Ping用于确定 API 是否可以访问，使用GET发起请求，请求地址为/api/ping，此接口可以直接访问，无需身份验证。")]
        [HttpGet, Route(Route)]
        public ActionResult<string> Get()
        {
            return "xblms";
        }
    }
}
