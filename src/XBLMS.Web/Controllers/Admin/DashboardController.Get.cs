using System.Threading.Tasks;
using Datory;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Configuration;
using XBLMS.Core.Utils;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin
{
    public partial class DashboardController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get()
        {
            var admin = await _organManager.GetAdministrator(_authManager.AdminId);

            return new GetResult
            {
                Administrator = admin,
            };
        }
    }
}
