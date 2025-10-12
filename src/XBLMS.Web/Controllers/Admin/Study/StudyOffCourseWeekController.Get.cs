using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Models;
using XBLMS.Utils;
namespace XBLMS.Web.Controllers.Admin.Study
{
    public partial class StudyOffCourseWeekController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get()
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var auth = await _authManager.GetAdminAuth();

            var resulList = new List<StudyCourse>();

            var (total, list) = await _studyCourseRepository.GetOffTrinListByWeekAsync(auth, false);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    item.Set("BeginDateTimeStr", item.OfflineBeginDateTime.Value.ToString(DateUtils.FormatStringDateOnlyCN));
                    item.Set("EndDateTimeStr", item.OfflineEndDateTime.Value.ToString(DateUtils.FormatStringDateOnlyCN));
                    item.Set("PlanId", 0);
                    item.Set("CouresName", $"[公共课]{item.Name}");
                    item.Set("DayCount", DateUtils.DateDiff("day", item.OfflineBeginDateTime.Value, item.OfflineEndDateTime.Value));
                    resulList.Add(item);
                }
            }
            var (planoffTrainTotal, planoffTrainList) = await _studyPlanCourseRepository.GetOffTrinListByWeekAsync(auth);
            if (planoffTrainTotal > 0)
            {
                DateTime today = DateTime.Now;
                DateTime startOfWeek = today;
                DateTime endOfWeek = today;
                int dayOfWeek = (int)today.DayOfWeek;
                if (dayOfWeek == 0)
                {
                    startOfWeek = today.AddDays(-6);
                }
                else
                {
                    startOfWeek = today.AddDays(1 - dayOfWeek);
                }
                endOfWeek = startOfWeek.AddDays(6);
                foreach (var item in planoffTrainList)
                {
                    var offTrain = await _studyCourseRepository.GetByIdAsync(item.CourseId);
                    if (offTrain != null && offTrain.OfflineBeginDateTime.Value >= startOfWeek && offTrain.OfflineBeginDateTime <= endOfWeek)
                    {
                        var plan = await _studyPlanRepository.GetAsync(item.PlanId);
                        offTrain.ExamId = item.ExamId;
                        offTrain.ExamQuestionnaireId = item.ExamQuestionnaireId;
                        offTrain.StudyCourseEvaluationId = item.StudyCourseEvaluationId;
                        

                        offTrain.Set("BeginDateTimeStr", offTrain.OfflineBeginDateTime.Value.ToString(DateUtils.FormatStringDateOnlyCN));
                        offTrain.Set("EndDateTimeStr", offTrain.OfflineEndDateTime.Value.ToString(DateUtils.FormatStringDateOnlyCN));
                        offTrain.Set("PlanId", item.PlanId);
                        offTrain.Set("CouresName", plan != null ? $"[{plan.PlanName}]{offTrain.Name}" : $"[计划内]{offTrain.Name}");
                        offTrain.Set("DayCount", DateUtils.DateDiff("day", offTrain.OfflineBeginDateTime.Value, offTrain.OfflineEndDateTime.Value));
                        offTrain.Guid = item.Guid;
                        resulList.Add(offTrain);
                    }
                }
            }
            return new GetResult
            {
                Total = total,
                List = resulList,
            };
        }

    }
}
