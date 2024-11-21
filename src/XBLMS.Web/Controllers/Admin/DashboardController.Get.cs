using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
