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
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<BoolResult>> Delete([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Delete))
            {
                return this.NoAuth();
            }

            var assInfo = await _examAssessmentRepository.GetAsync(request.Id);
            if (assInfo != null)
            {
                await _examAssessmentRepository.DeleteAsync(assInfo.Id);
                await _examManager.ClearExamAssessment(assInfo.Id);
                await _authManager.AddAdminLogAsync("删除测评", $"{assInfo.Title}");

            }
            return new BoolResult
            {
                Value = true
            };
        }

    }
}
