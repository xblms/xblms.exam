using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamPaperTreeController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get()
        {
            var adminAuth = await _authManager.GetAdminAuth();

            var trees = await _examManager.GetExamPaperTreeCascadesAsync(adminAuth, true);

            return new GetResult
            {
                Items = trees,
            };
        }
    }
}
