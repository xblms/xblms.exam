using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using XBLMS.Utils;
namespace XBLMS.Web.Controllers.Admin.Study
{
    public partial class StudyCourseEvaluationSelectController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var auth = await _authManager.GetAdminAuth();

            var (total, list) = await _studyCourseEvaluationRepository.GetListAsync(auth, request.Keyword, request.PageIndex, request.PageSize);

            return new GetResult
            {
                Total = total,
                List = list,
            };
        }

    }
}
