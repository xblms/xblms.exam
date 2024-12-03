using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;
namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamAssessmentController
    {
        [HttpPost, Route(RouteLock)]
        public async Task<ActionResult<BoolResult>> Lock([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }

            var assInfo = await _examAssessmentRepository.GetAsync(request.Id);
            if (assInfo == null) return NotFound();
            assInfo.Locked = true;

            await _examAssessmentRepository.UpdateAsync(assInfo);
            await _examAssessmentUserRepository.UpdateLockedAsync(assInfo.Id, assInfo.Locked);

            await _authManager.AddAdminLogAsync("锁定测评", $"{assInfo.Title}");

            return new BoolResult
            {
                Value = true
            };
        }

    }
}
