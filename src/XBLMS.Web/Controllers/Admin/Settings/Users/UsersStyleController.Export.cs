using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Users
{
    public partial class UsersStyleController
    {
        [HttpGet, Route(RouteExport)]
        public async Task<ActionResult> Export()
        {
            var admin = await _authManager.GetAdminAsync();


            var fileName = "";

            var filePath = _pathManager.GetTemporaryFilesPath(fileName);
            return this.Download(filePath);
        }
    }
}
