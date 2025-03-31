using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPracticeController
    {
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<BoolResult>> Delete([FromBody] IdRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            await _examPracticeRepository.DeleteByIdAsync(request.Id);
            await _examPracticeAnswerRepository.DeleteByPracticeIdAsync(request.Id);
            
            return new BoolResult
            {
               Value = true,
            };
        }
    }
}
