using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;

namespace XBLMS.Web.Controllers.Home.Study
{
    public partial class StudyPlanInfoController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] IdRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) { return Unauthorized(); }

            var planUser = await _studyPlanUserRepository.GetAsync(request.Id);
            var oldState = planUser.State;

            await _studyManager.User_GetPlanInfo(planUser, true);

            var plan = await _studyPlanRepository.GetAsync(planUser.PlanId);

            var pointNotice = new PointNotice();
            if (oldState != planUser.State && (planUser.State == StudyStatType.Yiwancheng || planUser.State == StudyStatType.Yidabiao))
            {
                await _authManager.AddPointsLogAsync(PointType.PointPlanOver, user, plan.Id, plan.PlanName);
                pointNotice = await _authManager.PointNotice(PointType.PointPlanOver, user.Id);
            }

            return new GetResult
            {
                PointNotice = pointNotice,
                Item = planUser
            };
        }
    }
}
