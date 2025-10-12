using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;
namespace XBLMS.Web.Controllers.Admin.Study
{
    public partial class StudyCourseEvaluationController
    {
        [HttpPost, Route(RouteLock)]
        public async Task<ActionResult<BoolResult>> Lock([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }

            var item = await _studyCourseEvaluationRepository.GetAsync(request.Id);
            if (item == null) return NotFound();
            item.Locked = true;

            await _studyCourseEvaluationRepository.UpdateAsync(item);

            await _authManager.AddAdminLogAsync("锁定课程评价", item.Title);
            await _authManager.AddStatLogAsync(StatType.StudyEvaluationUpdate, "禁用课程评价", item.Id, item.Title);

            return new BoolResult
            {
                Value = true
            };
        }

    }
}
