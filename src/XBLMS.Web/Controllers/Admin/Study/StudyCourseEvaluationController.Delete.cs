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
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<BoolResult>> Delete([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Delete))
            {
                return this.NoAuth();
            }

            var item = await _studyCourseEvaluationRepository.GetAsync(request.Id);
            if (item != null)
            {
                await _studyCourseEvaluationRepository.DeleteAsync(item.Id);
                await _studyCourseEvaluationItemRepository.DeleteByEvaluationIdAsync(item.Id);

                await _authManager.AddAdminLogAsync("删除课程评价", item.Title);
                await _authManager.AddStatLogAsync(StatType.StudyEvaluationDelete, "删除课程评价", item.Id, item.Title);
                await _authManager.AddStatCount(StatType.StudyEvaluationDelete);
            }
            return new BoolResult
            {
                Value = true
            };
        }

    }
}
