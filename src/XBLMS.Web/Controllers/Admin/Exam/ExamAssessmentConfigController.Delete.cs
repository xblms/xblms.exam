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
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<BoolResult>> Delete([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Delete))
            {
                return this.NoAuth();
            }

            var item = await _examAssessmentConfigRepository.GetAsync(request.Id);
            if (item != null)
            {
                await _examAssessmentConfigRepository.DeleteAsync(item.Id);
                await _examAssessmentConfigSetRepository.DeleteByConfigIdAsync(item.Id);
                await _authManager.AddAdminLogAsync("删除测评类别", $"{item.Title}");
            }
            return new BoolResult
            {
                Value = true
            };
        }

    }
}
