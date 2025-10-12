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
        [HttpPost, Route(RouteLock)]
        public async Task<ActionResult<BoolResult>> Lock([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }

            var plan = await _studyPlanRepository.GetAsync(request.Id);
            if (plan == null) return NotFound();
            plan.Locked = true;

            await _studyPlanRepository.UpdateAsync(plan);
            await _studyPlanUserRepository.UpdateByPlanAsync(plan);

            await _authManager.AddAdminLogAsync("锁定学习任务", plan.PlanName);
            await _authManager.AddStatLogAsync(StatType.StudyPlanUpdate, "锁定学习任务", plan.Id, plan.PlanName);

            return new BoolResult
            {
                Value = true
            };
        }

    }
}
