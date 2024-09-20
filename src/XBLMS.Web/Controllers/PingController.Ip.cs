using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers
{
    public partial class PingController
    {
        [HttpGet, Route(RouteIp)]
        public async Task<string> Ip()
        {
            return await RestUtils.GetIpAddressAsync();
        }
    }
}
