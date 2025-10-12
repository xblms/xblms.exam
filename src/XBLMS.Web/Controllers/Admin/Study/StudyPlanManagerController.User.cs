using Datory;
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
        [HttpGet, Route(RouteUser)]
        public async Task<ActionResult<GetUserResult>> Submit([FromQuery] GetUserRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Manage))
            {
                return this.NoAuth();
            }
            var (total, list) = await _studyPlanUserRepository.GetListAsync(request.State, request.KeyWords, request.Id, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                var plan = await _studyPlanRepository.GetAsync(request.Id);
                var courseCreditTotal = await _studyPlanCourseRepository.GetTotalCreditAsync(request.Id, false);
                var courseSelectCreditTotal = await _studyPlanCourseRepository.GetTotalCreditAsync(request.Id, true);

                foreach (var item in list)
                {

                    var user = await _organManager.GetUser(item.UserId);
                    if (user == null)
                    {
                        user = new User();
                    }

                    item.Set("User", user);
                    item.Set("Plan", plan);

                    var overCourse = await _studyCourseUserRepository.GetOverCountAsync(plan.Id, user.Id, false);
                    var overSelectCourse = await _studyCourseUserRepository.GetOverCountAsync(plan.Id, user.Id, true);
                    item.Set("OverCourse", overCourse);
                    item.Set("OverSelectCourse", overSelectCourse);

                    var overCourseCreditTotal = await _studyCourseUserRepository.GetOverTotalCreditAsync(plan.Id, user.Id, false);
                    var overSelectCourseCreditTotal = await _studyCourseUserRepository.GetOverTotalCreditAsync(plan.Id, user.Id, true);
                    item.Set("OverCredit", overCourseCreditTotal);
                    item.Set("OverSelectCredit", overSelectCourseCreditTotal);
                    item.Set("Credit", courseCreditTotal);
                    item.Set("SelectCredit", courseSelectCreditTotal);

                    decimal maxScore = 0;
                    if (plan.ExamId > 0)
                    {
                        maxScore = await _examPaperStartRepository.GetMaxScoreAsync(user.Id, plan.ExamId, plan.Id, 0);
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
            return new GetUserResult
            {
                List = list,
                Total = total,
            };
        }


        [HttpPost, Route(RouteUserExport)]
        public async Task<ActionResult<StringResult>> UserExport([FromBody] GetUserRequest request)
        {
            var plan = await _studyPlanRepository.GetAsync(request.Id);

            var fileName = $"{plan.PlanName}-学员列表.xlsx";
            var filePath = _pathManager.GetDownloadFilesPath(fileName);

            DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(filePath));
            FileUtils.DeleteFileIfExists(filePath);

            var head = new List<string>
            {
                "序号",
                "账号",
                "姓名",
                "组织",
                "必修课",
                "选修课",
                "完成学分",
                "必修学分",
                "选修学分",
                "大考成绩",
                "状态",
                "完成时间"
            };
            var rows = new List<List<string>>();

            var (total, list) = await _studyPlanUserRepository.GetListAsync(request.State, request.KeyWords, request.Id, 1, int.MaxValue);
            if (total > 0)
            {
                var index = 1;
                var courseCreditTotal = await _studyPlanCourseRepository.GetTotalCreditAsync(request.Id, false);
                var courseSelectCreditTotal = await _studyPlanCourseRepository.GetTotalCreditAsync(request.Id, true);

                foreach (var item in list)
                {

                    var user = await _organManager.GetUser(item.UserId);
                    if (user == null)
                    {
                        user = new User();
                    }

                    var overCourse = await _studyCourseUserRepository.GetOverCountAsync(plan.Id, user.Id, false);
                    var overSelectCourse = await _studyCourseUserRepository.GetOverCountAsync(plan.Id, user.Id, true);

                    var overCourseCreditTotal = await _studyCourseUserRepository.GetOverTotalCreditAsync(plan.Id, user.Id, false);
                    var overSelectCourseCreditTotal = await _studyCourseUserRepository.GetOverTotalCreditAsync(plan.Id, user.Id, true);

                    decimal maxScore = 0;
                    if (plan.ExamId > 0)
                    {
                        maxScore = await _examPaperStartRepository.GetMaxScoreAsync(user.Id, plan.ExamId, plan.Id, 0);
                    }
                    else
                    {
                        maxScore = -1;
                    }

                    item.Set("StateStr", item.State.GetDisplayName());

                    rows.Add([
                                index.ToString(),
                                user.UserName,
                                user.DisplayName,
                                user.Get("OrganNames").ToString(),
                                $"{overCourse}/{plan.TotalCount}",
                                 $"{overSelectCourse}/{plan.SelectTotalCount}",
                                item.TotalCredit.ToString(),
                                $"{overCourseCreditTotal}/{courseCreditTotal}",
                               $"{overSelectCourseCreditTotal}/{courseSelectCreditTotal}",
                               maxScore >= 0?maxScore.ToString():"/",
                               item.State.GetDisplayName(),
                               item.LastStudyDateTime.ToString()
                            ]);

                    index++;

                }
            }

            ExcelUtils.Write(filePath, head, rows);

            var downloadUrl = _pathManager.GetDownloadFilesUrl(fileName);
            await _authManager.AddAdminLogAsync($"导出学员(计划：{plan.PlanName})");
            await _authManager.AddStatLogAsync(StatType.Export, $"导出学员(计划：{plan.PlanName})", 0, string.Empty, new StringResult { Value = downloadUrl });

            return new StringResult
            {
                Value = downloadUrl
            };
        }
    }


}
