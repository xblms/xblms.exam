using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;
namespace XBLMS.Web.Controllers.Admin.Study
{
    public partial class StudyPlanController
    {
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<BoolResult>> Delete([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Delete))
            {
                return this.NoAuth();
            }

            var plan = await _studyPlanRepository.GetAsync(request.Id);
            if (plan != null)
            {
                await _studyPlanRepository.DeleteAsync(plan.Id);
                await _studyPlanUserRepository.DeleteByPlanAsync(plan.Id);
                await _studyPlanCourseRepository.DeleteByPlanAsync(plan.Id);

                await _authManager.AddAdminLogAsync("删除学习任务", plan.PlanName);
                await _authManager.AddStatLogAsync(StatType.StudyPlanDelete, "删除计划", plan.Id, plan.PlanName);
                await _authManager.AddStatCount(StatType.StudyPlanDelete);
            }
            return new BoolResult
            {
                Value = true
            };
        }

    }
}
