using Microsoft.AspNetCore.Mvc;
using XBLMS.Dto;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPaperExamingController
    {
        [HttpPost, Route(RouteSubmitTiming)]
        public async void SubmitTiming([FromBody] IdRequest request)
        {
            await _examPaperStartRepository.IncrementAsync(request.Id);
        }
    }
}
