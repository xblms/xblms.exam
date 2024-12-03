using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;
namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamAssessmentConfigController
    {
        [HttpPost, Route(RouteUnLock)]
        public async Task<ActionResult<BoolResult>> UnLocked([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }

            var item = await _examAssessmentConfigRepository.GetAsync(request.Id);
            if (item == null) return NotFound();
            item.Locked = false;

            await _examAssessmentConfigRepository.UpdateAsync(item);

            await _authManager.AddAdminLogAsync("解锁测评类别", $"{item.Title}");

            return new BoolResult
            {
                Value = true
            };
        }

    }
}
