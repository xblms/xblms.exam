using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;

namespace XBLMS.Web.Controllers.Home.Study
{
    public partial class StudyCourseOfflineInfoController
    {
        [HttpPost, Route(RouteCollection)]
        public async Task<ActionResult<BoolResult>> Collection([FromBody] IdRequest request)
        {
            var usercourse = await _studyCourseUserRepository.GetAsync(request.Id);
            usercourse.Collection = !usercourse.Collection;
            await _studyCourseUserRepository.UpdateAsync(usercourse);

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
