using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPaperSubmitResultController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] IdRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            var start = await _examPaperStartRepository.GetAsync(request.Id);
            if (start.IsSubmit)
            {
                var paper = await _examPaperRepository.GetAsync(start.ExamPaperId);
                return new GetResult
                {
                    Success = true,
                    IsShowScore = paper.SecrecyScore,
                    Score = paper.SecrecyScore ? start.Score : 0,
                    IsPass = paper.SecrecyScore ? start.Score >= paper.PassScore : false,
                    Title = paper.Title,
                    IsMark = start.IsMark
                };
            }
            else
            {
                var queue = 0;
                var taskStarids = _createManager.GetTaskStartIds();

                if (taskStarids.Count > 0)
                {
                    queue = taskStarids.Count - taskStarids.IndexOf(request.Id);
                }

                return new GetResult
                {
                    Queue = queue
                };

            }

        }
    }
}
