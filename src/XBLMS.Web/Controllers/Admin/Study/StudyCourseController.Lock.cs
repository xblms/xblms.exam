using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;
namespace XBLMS.Web.Controllers.Admin.Study
{
    public partial class StudyCourseController
    {
        [HttpPost, Route(RouteLock)]
        public async Task<ActionResult<BoolResult>> Lock([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }

            var course = await _studyCourseRepository.GetAsync(request.Id);
            if (course == null) return NotFound();
            course.Locked = true;

            await _studyCourseRepository.UpdateAsync(course);
            await _studyCourseUserRepository.UpdateByCourseAsync(course);

            await _authManager.AddAdminLogAsync("锁定课程", course.Name);
            await _authManager.AddStatLogAsync(StatType.StudyCourseUpdate, "禁用课程", course.Id, course.Name);

            return new BoolResult
            {
                Value = true
            };
        }

    }
}
