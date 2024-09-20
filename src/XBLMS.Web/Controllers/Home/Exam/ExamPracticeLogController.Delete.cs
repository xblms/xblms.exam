using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPracticeLogController
    {
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<BoolResult>> Delete()
        {
            var user = await _authManager.GetUserAsync();

            await _examPracticeRepository.DeleteAsync(user.Id);

            return new BoolResult
            {
               Value= true,
            };
        }
    }
}
