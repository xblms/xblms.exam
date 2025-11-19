using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPaperInfoController
    {
        [HttpGet, Route(RouteCheck)]
        public async Task<ActionResult<GetCheckResult>> Check([FromQuery] GetRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            var (success, msg) = await _examManager.CheckExam(request.Id, user.Id, request.PlanId, request.CourseId);
            return new GetCheckResult
            {
                Success = success,
                Msg = msg
            };
        }
    }
}
