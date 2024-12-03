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
        [HttpPost, Route(RouteLock)]
        public async Task<ActionResult<BoolResult>> Lock([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }

            var item = await _examAssessmentConfigRepository.GetAsync(request.Id);
            if (item == null) return NotFound();
            item.Locked = true;

            await _examAssessmentConfigRepository.UpdateAsync(item);

            await _authManager.AddAdminLogAsync("锁定测评类别", $"{ item.Title }");

            return new BoolResult
            {
                Value = true
            };
        }

    }
}
