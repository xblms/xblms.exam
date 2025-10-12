using Datory;
using System;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class StudyManager
    {
        public async Task User_GetCourseInfoByCourseList(int planId, StudyCourse course, StudyCourseUser courseUser)
        {
            var courseType = "";
            if (courseUser != null)
            {
                course.Set("State", courseUser.State);
                course.Set("StateStr", courseUser.State.GetDisplayName());
                if (planId > 0)
                {
                    var planCourse = await _studyPlanCourseRepository.GetAsync(planId, course.Id);
                    if (planCourse != null)
                    {
                        if (planCourse.IsSelectCourse)
                        {
                            courseType = "选修课";
                        }
                        else
                        {
                            courseType = "必修课";
                        }
                    }

                }
                else
                {
                    courseType = "公共课";
                }
            }
            else
            {
                course.Set("State", "");
                course.Set("StateStr", "未学");
                courseType = "公共课";
            }

            course.Set("CourseType", courseType);
            course.Set("EvaluationAvg", TranslateUtils.ToAvg(Convert.ToDouble(course.TotalAvgEvaluation), course.TotaEvaluationlUser));
            course.Set("PlanId", planId);
        }
    }
}
