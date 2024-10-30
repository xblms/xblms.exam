using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPaperInfoController
    {
        [HttpGet, Route(RouteCheck)]
        public async Task<ActionResult<GetCheckResult>> Check([FromQuery] IdRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            var (success, msg) = await _examManager.CheckExam(request.Id, user.Id);
            return new GetCheckResult
            {
                Success = success,
                Msg = msg
            };
        }
    }
}
