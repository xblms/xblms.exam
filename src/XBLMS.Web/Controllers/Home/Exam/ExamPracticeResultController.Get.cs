using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Dto;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPracticeResultController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] IdRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            var practice = await _examPracticeRepository.GetAsync(request.Id);
            return new GetResult
            {
                Title = $"{practice.Title}（{practice.CreatedDate.Value.ToString(DateUtils.FormatStringDateTimeCN)}）",
                Total = practice.ParentId == 0 ? practice.MineTmCount = practice.TmCount : practice.MineTmCount,
                AnswerTotal = practice.AnswerCount,
                RightTotal = practice.RightCount,
                WrongTotal = practice.AnswerCount - practice.RightCount
            };
        }
    }
}
