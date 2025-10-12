using Datory;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class UserDocLayerViewController
    {
        [HttpGet, Route(RouteStudyPlan)]
        public async Task<ActionResult<GetPlanResult>> GetStudyPlan([FromQuery] GetRequest request)
        {
            var user = await _userRepository.GetByUserIdAsync(request.Id);

            var (total, list) = await _databaseManager.StudyPlanUserRepository.Analysis_GetListAsync(request.Id, request.DateFrom, request.DateTo, request.PageIndex, request.PageSize);

            if (total > 0)
            {
                foreach (var item in list)
                {
                    var plan = await _databaseManager.StudyPlanRepository.GetAsync(item.PlanId);
                    if (plan == null) continue;
                    var courseCreditTotal = await _databaseManager.StudyPlanCourseRepository.GetTotalCreditAsync(plan.Id, false);
                    var courseSelectCreditTotal = await _databaseManager.StudyPlanCourseRepository.GetTotalCreditAsync(plan.Id, true);

                    item.Set("Plan", plan);

                    var overCourse = await _databaseManager.StudyCourseUserRepository.GetOverCountAsync(plan.Id, user.Id, false);
                    var overSelectCourse = await _databaseManager.StudyCourseUserRepository.GetOverCountAsync(plan.Id, user.Id, true);
                    item.Set("OverCourse", overCourse);
                    item.Set("OverSelectCourse", overSelectCourse);

                    var overCourseCreditTotal = await _databaseManager.StudyCourseUserRepository.GetOverTotalCreditAsync(plan.Id, user.Id, false);
                    var overSelectCourseCreditTotal = await _databaseManager.StudyCourseUserRepository.GetOverTotalCreditAsync(plan.Id, user.Id, true);
                    item.Set("OverCredit", overCourseCreditTotal);
                    item.Set("OverSelectCredit", overSelectCourseCreditTotal);
                    item.Set("Credit", courseCreditTotal);
                    item.Set("SelectCredit", courseSelectCreditTotal);

                    decimal maxScore = 0;
                    if (plan.ExamId > 0)
                    {
                        maxScore = await _databaseManager.ExamPaperStartRepository.GetMaxScoreAsync(user.Id, plan.ExamId, plan.Id, 0);
                    }
                    else
                    {
                        maxScore = -1;
                    }
                    if (maxScore >= 0)
                    {
                        item.Set("MaxScore", maxScore);
                    }
                    else
                    {
                        item.Set("MaxScore", "/");
                    }

                    item.Set("StateStr", item.State.GetDisplayName());

                }
            }

            return new GetPlanResult
            {
                Total = total,
                List = list
            };
        }

        public class GetPlanResult
        {
            public int Total { get; set; }
            public List<StudyPlanUser> List { get; set; }
        }
    }
}
