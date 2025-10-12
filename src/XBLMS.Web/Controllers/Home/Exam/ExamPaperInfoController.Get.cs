using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Enums;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPaperInfoController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            var paper = await _examPaperRepository.GetAsync(request.Id);
            await _examManager.GetPaperInfo(paper, user, request.PlanId, request.CourseId, true);

            var pointNotice = await _authManager.PointNotice(PointType.PointExam, user.Id);

            return new GetResult
            {
                pointNotice = pointNotice,
                Item = paper
            };
        }
    }
}
