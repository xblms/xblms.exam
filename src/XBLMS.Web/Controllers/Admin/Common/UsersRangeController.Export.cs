using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class UsersRangeController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<StringResult>> Export([FromBody] GetRequest request)
        {
            var admin = await _authManager.GetAdminAsync();
            return new StringResult
            {
                Value = ""
            };
        }
    }
}
