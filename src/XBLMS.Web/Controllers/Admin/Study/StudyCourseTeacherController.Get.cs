using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Models;
using XBLMS.Utils;
namespace XBLMS.Web.Controllers.Admin.Study
{
    public partial class StudyCourseTeacherController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var resulttotal = 0;
            var resultList = new List<StudyCourse>();

            var auth = await _authManager.GetAdminAuth();

            if (request.Type == "plan")
            {
                var (total, list) = await _studyPlanCourseRepository.GetListByTeacherAsync(auth.AdminId, request.Keyword, request.PageIndex, request.PageSize);
                if (total > 0)
                {
                    resulttotal = total;
                    foreach (var item in list)
                    {
                        var (totalUser, overTotalUser) = await _studyCourseUserRepository.GetOverCountByAnalysisAsync(item.PlanId, item.CourseId);

                        var plan = await _studyPlanRepository.GetAsync(item.PlanId);
                        var course = await _studyCourseRepository.GetAsync(item.CourseId);
                        if (course != null)
                        {
                            course.Name = item.CourseName;
                            course.Credit = item.Credit;
                            course.StudyHour = item.StudyHour;

                            course.Set("StudyUser", totalUser);
                            course.Set("OverUser", overTotalUser);
                            course.Set("Plan", plan?.PlanName);
                            course.Set("PlanId", plan?.Id);
                            course.Set("OfflineBeginDateTimeStr", course.OfflineBeginDateTime.Value.ToString(DateUtils.FormatStringDateTimeCN));
                            course.Set("OfflineEndDateTimeStr", course.OfflineEndDateTime.Value.ToString(DateUtils.FormatStringDateTimeCN));
                            resultList.Add(course);
                        }
                    }
                }
            }
            else
            {

                var (total, list) = await _studyCourseRepository.GetListByTeacherAsync(auth.AdminId, request.Keyword, request.PageIndex, request.PageSize);
                if (total > 0)
                {
                    resulttotal = total;
                    foreach (var item in list)
                    {
                        if (item != null)
                        {
                            var (totalUser, overTotalUser) = await _studyCourseUserRepository.GetOverCountByAnalysisAsync(0, item.Id);

                            item.Set("StudyUser", totalUser);
                            item.Set("OverUser", overTotalUser);
                            item.Set("Plan", "公共课");
                            item.Set("PlanId", 0);
                            item.Set("OfflineBeginDateTimeStr", item.OfflineBeginDateTime.Value.ToString(DateUtils.FormatStringDateTimeCN));
                            item.Set("OfflineEndDateTimeStr", item.OfflineEndDateTime.Value.ToString(DateUtils.FormatStringDateTimeCN));
                            resultList.Add(item);
                        }
                    }
                }
            }


            return new GetResult
            {
                Total = resulttotal,
                List = resultList,
            };
        }

    }
}
