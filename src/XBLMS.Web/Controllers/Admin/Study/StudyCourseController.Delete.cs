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
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<BoolResult>> Delete([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Delete))
            {
                return this.NoAuth();
            }

            var course = await _studyCourseRepository.GetAsync(request.Id);
            if (course != null)
            {
                await _studyCourseRepository.DeleteAsync(course.Id);
                await _studyCourseUserRepository.DeleteByCourseAsync(course.Id);
                await _studyCourseWareRepository.DeleteByCourseIdAsync(course.Id);

                await _authManager.AddAdminLogAsync("删除课程",course.Name);
                await _authManager.AddStatLogAsync(StatType.StudyCourseDelete, "删除课程", course.Id, course.Name);
                await _authManager.AddStatCount(StatType.StudyCourseDelete);
            }
            return new BoolResult
            {
                Value = true
            };
        }

    }
}
