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
            var userId = _authManager.UserId;
            var (success, msg) = await _examManager.CheckExam(request.Id, userId);
            return new GetCheckResult
            {
                Success = success,
                Msg = msg
            };
        }
    }
}
