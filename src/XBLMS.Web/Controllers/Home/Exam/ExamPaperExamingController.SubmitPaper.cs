using Microsoft.AspNetCore.Mvc;
using XBLMS.Dto;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPaperExamingController
    {
        [HttpPost, Route(RouteSubmitPaper)]
        public void SubmitPaper([FromBody] IdRequest request)
        {
            _createManager.CreateSubmitPaperAsync(request.Id);
        }
    }
}
