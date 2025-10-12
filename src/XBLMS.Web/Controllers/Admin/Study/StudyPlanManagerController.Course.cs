using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Study
{
    public partial class StudyPlanManagerController
    {
        [HttpGet, Route(RouteCourse)]
        public async Task<ActionResult<GetCourseResult>> GetCourse([FromQuery] GetCourseRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Manage))
            {
                return this.NoAuth();
            }

            var plan = await _studyPlanRepository.GetAsync(request.Id);

            var resultList = new List<StudyPlanCourse>();

            var list = await _studyPlanCourseRepository.GetListAsync(plan.Id);
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    var totalUser = await _studyPlanUserRepository.GetCountAsync(plan.Id, "");
                    var overUser = await _studyCourseUserRepository.GetOverCountByAnalysisAsync(plan.Id, item.CourseId, true);
                    var studyUser = await _studyCourseUserRepository.GetOverCountByAnalysisAsync(plan.Id, item.CourseId, null);

                    var (starUser, starTotal) = await _studyCourseUserRepository.GetEvaluation(plan.Id, item.CourseId);

                    item.Set("Star", TranslateUtils.ToAvg(starTotal, starUser));
                    item.Set("StarUser", starUser);
                    item.Set("TotalUser", totalUser);
                    item.Set("OverUser", overUser);
                    item.Set("StudyUser", studyUser);

                    if (!string.IsNullOrEmpty(request.KeyWords))
                    {
                        if (item.CourseName.Contains(request.KeyWords))
                        {
                            resultList.Add(item);
                        }
                    }
                    else
                    {
                        resultList.Add(item);
                    }
                }
            }
            return new GetCourseResult
            {
                PlanName = plan.PlanName,
                List = resultList,
            };
        }
        [HttpPost, Route(RouteCourseExport)]
        public async Task<ActionResult<StringResult>> UserExport([FromBody] GetCourseRequest request)
        {
            var plan = await _studyPlanRepository.GetAsync(request.Id);

            var fileName = $"{plan.PlanName}-课程列表.xlsx";
            var filePath = _pathManager.GetDownloadFilesPath(fileName);

            DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(filePath));
            FileUtils.DeleteFileIfExists(filePath);

            var head = new List<string>
            {
                "序号",
                "课程",
                "类型",
                "课时",
                "学时",
                "学分",
                "培训人数",
                "学习人数",
                "完成人数",
                "完成率",
                "课程评价",
            };
            var rows = new List<List<string>>();

            var list = await _studyPlanCourseRepository.GetListAsync(plan.Id);
            if (list != null && list.Count > 0)
            {
                var index = 1;

                foreach (var item in list)
                {

                    var type = "必修课";
                    if (item.IsSelectCourse)
                    {
                        type = "选修课";
                        if (item.OffLine)
                        {
                            type = type + "-线下课";
                        }
                    }
                    else
                    {
                        if (item.OffLine)
                        {
                            type = type + "-线下课";
                        }
                    }

                    var totalUser = await _studyPlanUserRepository.GetCountAsync(plan.Id, "");
                    var overUser = await _studyCourseUserRepository.GetOverCountByAnalysisAsync(plan.Id, item.CourseId, true);
                    var studyUser = await _studyCourseUserRepository.GetOverCountByAnalysisAsync(plan.Id, item.CourseId, null);
                    var (starUser, starTotal) = await _studyCourseUserRepository.GetEvaluation(plan.Id, item.CourseId);

                    item.Set("Star", TranslateUtils.ToAvg(starTotal, starUser));

                    var rowsValues = new List<string>() {
                            index.ToString(),
                            item.CourseName,
                            type,
                             item.StudyHour.ToString(),
                             TranslateUtils.ToMinuteAndSecond(item.Duration, true),
                             item.Credit.ToString(),
                             totalUser.ToString(),
                             studyUser.ToString(),
                             overUser.ToString(),
                             TranslateUtils.ToPercent(overUser, studyUser) + "%",
                             item.StudyCourseEvaluationId > 0 ? $"{TranslateUtils.ToAvg(starTotal, starUser)}星({starUser})" : "/"
                       };

                    if (!string.IsNullOrEmpty(request.KeyWords))
                    {
                        if (item.CourseName.Contains(request.KeyWords))
                        {
                            rows.Add(rowsValues);
                            index++;
                        }
                    }
                    else
                    {
                        rows.Add(rowsValues);
                        index++;
                    }


                }
            }

            ExcelUtils.Write(filePath, head, rows);

            var downloadUrl = _pathManager.GetDownloadFilesUrl(fileName);
            await _authManager.AddAdminLogAsync($"导出课程(计划：{plan.PlanName})");
            await _authManager.AddStatLogAsync(StatType.Export, $"导出课程(计划：{plan.PlanName})", 0, string.Empty, new StringResult { Value = downloadUrl });

            return new StringResult
            {
                Value = downloadUrl
            };
        }
    }
}
