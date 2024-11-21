using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPracticeLogController
    {
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<BoolResult>> Delete()
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            await _examPracticeRepository.DeleteAsync(user.Id);

            return new BoolResult
            {
               Value= true,
            };
        }
    }
}
