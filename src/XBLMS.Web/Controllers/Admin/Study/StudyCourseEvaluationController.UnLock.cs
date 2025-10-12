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
        [HttpPost, Route(RouteUnLock)]
        public async Task<ActionResult<BoolResult>> UnLocked([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }

            var item = await _studyCourseEvaluationRepository.GetAsync(request.Id);
            if (item == null) return NotFound();
            item.Locked = false;

            await _studyCourseEvaluationRepository.UpdateAsync(item);

            await _authManager.AddAdminLogAsync("解锁课程评价", item.Title);
            await _authManager.AddStatLogAsync(StatType.StudyEvaluationUpdate, "启用课程评价", item.Id, item.Title);

            return new BoolResult
            {
                Value = true
            };
        }

    }
}
