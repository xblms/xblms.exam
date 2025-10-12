using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using XBLMS.Utils;
namespace XBLMS.Web.Controllers.Admin.Study
{
    public partial class StudyCourseController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var auth = await _authManager.GetAdminAuth();

            var (total, list) = await _studyCourseRepository.GetListAsync(auth, request.Keyword, request.Type, request.TreeId, request.TreeIsChildren, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    var useCount = await _studyPlanCourseRepository.GetCourseUseCount(item.Id);
                    item.Set("UseCount", useCount);
                }
            }
            return new GetResult
            {
                Total = total,
                List = list,
            };
        }

    }
}
