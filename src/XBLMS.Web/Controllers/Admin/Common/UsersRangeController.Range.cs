using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class UsersRangeController
    {
        [HttpPost, Route(RouteRange)]
        public async Task<ActionResult<BoolResult>> RangeUser([FromBody] GetRangeRequest request)
        {
            var admin = await _authManager.GetAdminAsync();

            if (request.Ids != null && request.Ids.Count > 0)
            {
                foreach (var id in request.Ids)
                {
                    if(request.RangeType== RangeType.Exam)
                    {
                        await _examManager.Arrange(request.Id, id);
                    }
                }
            }
            return new BoolResult
            {
                Value = true
            };
        }
    }
}
