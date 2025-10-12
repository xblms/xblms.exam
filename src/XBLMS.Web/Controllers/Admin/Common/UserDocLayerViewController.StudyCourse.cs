using Datory;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class UserDocLayerViewController
    {
        [HttpGet, Route(RouteStudyCourse)]
        public async Task<ActionResult<GetCourseResult>> GetStudyCourse([FromQuery] GetRequest request)
        {

            var (total, list) = await _databaseManager.StudyCourseUserRepository.Analysis_GetListAsync(request.Id, request.DateFrom, request.DateTo, request.PageIndex, request.PageSize);

            if (total > 0)
            {
                foreach (var item in list)
                {
                    decimal maxCj = -1;

                    var course = await _databaseManager.StudyCourseRepository.GetAsync(item.CourseId);
                    if (course == null) continue;
                    var courseName = $"{course.Name}";

                    if (course.ExamId > 0)
                    {
                        maxCj = await _databaseManager.ExamPaperStartRepository.GetMaxScoreAsync(item.UserId, course.ExamId, 0, course.Id);
                    }

                    item.Set("StateStr", item.State.GetDisplayName());

                    if (item.PlanId > 0)
                    {
                        var plan = await _databaseManager.StudyPlanRepository.GetAsync(item.PlanId);
                        if (plan == null) continue;
                        var planCourse = await _databaseManager.StudyPlanCourseRepository.GetAsync(item.PlanId, item.CourseId);
                        if (planCourse == null) continue;
                        courseName = $"[{plan.PlanName}]{planCourse.CourseName}";

                        if ((planCourse != null && planCourse.ExamId > 0))
                        {
                            maxCj = await _databaseManager.ExamPaperStartRepository.GetMaxScoreAsync(item.UserId, planCourse.ExamId, plan.Id, item.CourseId);
                        }
                    }
             
                    if (maxCj >= 0)
                    {
                        item.Set("MaxScore", maxCj);
                    }
                    else
                    {
                        item.Set("MaxScore", "/");
                    }

                    item.Set("CourseName", courseName);
                    item.Set("Course", course);
                }
            }

            return new GetCourseResult
            {
                Total = total,
                List = list
            };
        }

        public class GetCourseResult
        {
            public int Total { get; set; }
            public List<StudyCourseUser> List { get; set; }
        }
    }
}
