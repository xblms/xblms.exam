using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamQuestionnaireSelectController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var auth = await _authManager.GetAdminAuth();

            var (total, list) = await _examQuestionnaireRepository.Select_GetListAsync(auth, request.Keyword, request.PageIndex, request.PageSize);

            return new GetResult
            {
                Total = total,
                Items = list,
            };
        }
    }
}
