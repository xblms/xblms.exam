using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Core.Utils.Office;
using XBLMS.Dto;
using XBLMS.Core.Utils;
using System.Collections.Generic;
using XBLMS.Enums;
using XBLMS.Utils;

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
