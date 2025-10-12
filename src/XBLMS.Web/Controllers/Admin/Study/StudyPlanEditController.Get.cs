using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Study
{
    public partial class StudyPlanEditController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] IdRequest request)
        {
            var auth = await _authManager.GetAdminAuth();

            var plan = new StudyPlan();
            plan.PlanName = "学习任务-" + StringUtils.PadZeroes(await _studyPlanRepository.MaxAsync(), 5);
            plan.CoverImg = _pathManager.DefaultCoursePlanCoverUrl;

            if (request.Id > 0)
            {
                plan = await _studyPlanRepository.GetAsync(request.Id);
            }

            var userGroupList = await _userGroupRepository.GetListAsync(auth);

            var courseList = await _studyPlanCourseRepository.GetListAsync(false, plan.Id);
            var courseSelectList = await _studyPlanCourseRepository.GetListAsync(true, plan.Id);

            return new GetResult
            {
                Item = plan,
                UserGroupList = userGroupList,
                CourseList = courseList,
                CourseSelectList = courseSelectList
            };

        }

    }
}
