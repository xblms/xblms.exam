using Microsoft.AspNetCore.Mvc;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPaperExamingController
    {
        [HttpPost, Route(RouteSubmitAnswer)]
        public void SubmitAnswer([FromBody] GetSubmitAnswerRequest request)
        {
            _createManager.CreateSubmitAnswerAsync(request.Answer);
        }

        [HttpPost, Route(RouteSubmitAnswerSmall)]
        public void SubmitAnswerSmall([FromBody] GetSubmitAnswerSmallRequest request)
        {
            _createManager.CreateSubmitAnswerSmallAsync(request.Answer);
        }
    }
}
