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
        [HttpPost, Route(RouteUnLock)]
        public async Task<ActionResult<BoolResult>> UnLocked([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }

            var plan = await _studyPlanRepository.GetAsync(request.Id);
            if (plan == null) return NotFound();
            plan.Locked = false;

            await _studyPlanRepository.UpdateAsync(plan);
            await _studyPlanUserRepository.UpdateByPlanAsync(plan);

            await _authManager.AddAdminLogAsync("解锁学习任务", plan.PlanName);
            await _authManager.AddStatLogAsync(StatType.StudyPlanUpdate, "解锁学习任务", plan.Id, plan.PlanName);

            return new BoolResult
            {
                Value = true
            };
        }

    }
}
