using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class SelectOrganController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var adminAuth = await _authManager.GetAdminAuth();
            var organs = await _organManager.GetOrganTreeTableDataLazyByChangeAsync(adminAuth, request.ParentId, request.KeyWords);
            return new GetResult
            {
                Organs = organs
            };
        }

        [HttpGet, Route(RouteChange)]
        public async Task<ActionResult<GetResult>> GetChange([FromQuery] GetRequest request)
        {
            var adminAuth = await _authManager.GetAdminAuth();
            var organs = await _organManager.GetOrganTreeTableDataLazyByChangeAsync(adminAuth, request.ParentId, request.KeyWords);
            return new GetResult
            {
                Organs = organs
            };
        }
    }
}
