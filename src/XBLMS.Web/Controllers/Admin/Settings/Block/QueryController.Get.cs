using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Block
{
    public partial class QueryController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<StringResult>> Get()
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }
            var admin = await _authManager.GetAdminAsync();
            return new StringResult
            {
                Value = PageUtils.GetIpAddress(Request)
            };
        }
    }
}
