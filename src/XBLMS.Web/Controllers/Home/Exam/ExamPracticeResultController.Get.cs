using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPracticeResultController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] IdRequest request)
        {
            var practice = await _examPracticeRepository.GetAsync(request.Id);
            return new GetResult
            {
                Total = practice.TmCount,
                AnswerTotal = practice.AnswerCount,
                RightTotal = practice.RightCount,
                WrongTotal = practice.AnswerCount - practice.RightCount
            };
        }
    }
}
