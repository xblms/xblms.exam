using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Study
{
    public partial class StudyCourseManagerController
    {
        [HttpGet, Route(RouteEvaluation)]
        public async Task<ActionResult<GetEvaluationResult>> GetEvaluation([FromQuery] GetEvaluationRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Manage))
            {
                return this.NoAuth();
            }

            var course = await _studyCourseRepository.GetAsync(request.Id);
            if (request.PlanId > 0)
            {
                var planCourse = await _studyPlanCourseRepository.GetAsync(request.PlanId, request.Id);
                course.StudyCourseEvaluationId = planCourse.StudyCourseEvaluationId;
            }

            var total = 0;
            var list = new List<StudyCourseEvaluationUser>();
            var items = new List<StudyCourseEvaluationItem>();

            if (course.StudyCourseEvaluationId > 0)
            {
                var evaluation = await _studyCourseEvaluationRepository.GetAsync(course.StudyCourseEvaluationId);
                if (evaluation != null)
                {
                    items = await _studyCourseEvaluationItemRepository.GetListAsync(evaluation.Id);
                    (total, list) = await _studyCourseEvaluationUserRepository.GetListAsync(request.PlanId, request.Id, request.KeyWords, request.PageIndex, request.PageSize);

                    if (total > 0)
                    {
                        foreach (var euser in list)
                        {
                            var user = await _organManager.GetUser(euser.UserId);
                            var userCourse = await _studyCourseUserRepository.GetAsync(user.Id, request.PlanId, request.Id);

                            if (userCourse != null)
                            {
                                euser.Set("AvgEvaluation", userCourse.AvgEvaluation);
                            }
                            else
                            {
                                euser.Set("AvgEvaluation", 0);
                            }

                            if (items != null && items.Count > 0)
                            {
                                foreach (var item in items)
                                {
                                    var userItemAnswer = await _studyCourseEvaluationItemUserRepository.GetAsync(request.PlanId, request.Id, euser.UserId, euser.EvaluationId, item.Id);
                                    if (userItemAnswer != null)
                                    {
                                        item.Set("StarValue", userItemAnswer.StarValue);
                                        item.Set("ContentValue", userItemAnswer.TextContent);
                                    }
                                    else
                                    {
                                        item.Set("StarValue", 0);
                                        item.Set("ContentValue", "");
                                    }
                                    item.Set("MaxStar", evaluation.MaxStar);
                                }
                            }
                            euser.Set("Items", items);
                            euser.Set("User", user);
                        }
                    }
                }
            }

            return new GetEvaluationResult
            {
                Total = total,
                List = list,
                Items = items,
            };

        }

        [HttpPost, Route(RouteEvaluationExport)]
        public async Task<ActionResult<StringResult>> EvaluationExport([FromBody] GetEvaluationRequest request)
        {
            var course = await _studyCourseRepository.GetAsync(request.Id);
            if (request.PlanId > 0)
            {
                var planCourse = await _studyPlanCourseRepository.GetAsync(request.PlanId, request.Id);
                course.StudyCourseEvaluationId = planCourse.StudyCourseEvaluationId;
            }

            var fileName = $"{course.Name}-课程评价.xlsx";
            var filePath = _pathManager.GetDownloadFilesPath(fileName);

            var head = new List<string>
                    {
                        "序号",
                        "账号",
                        "姓名",
                        "组织",
                        "平均分"
                    };
            var rows = new List<List<string>>();

            var total = 0;
            var list = new List<StudyCourseEvaluationUser>();

            if (course.StudyCourseEvaluationId > 0)
            {
                var evaluation = await _studyCourseEvaluationRepository.GetAsync(course.StudyCourseEvaluationId);
                if (evaluation != null)
                {
                    var items = await _studyCourseEvaluationItemRepository.GetListAsync(evaluation.Id);

                    (total, list) = await _studyCourseEvaluationUserRepository.GetListAsync(request.PlanId, request.Id, request.KeyWords, request.PageIndex, request.PageSize);

                    if (total > 0)
                    {
                        var index = 1;

                        foreach (var euser in list)
                        {
                            var user = await _organManager.GetUser(euser.UserId);
                            var userCourse = await _studyCourseUserRepository.GetAsync(user.Id, request.PlanId, request.Id);
                            decimal avgEvaluation = 0;
                            if (userCourse != null)
                            {
                                avgEvaluation = userCourse.AvgEvaluation;
                            }

                            var rowValue = new List<string>
                                    {
                                        index.ToString(),
                                        user.UserName,
                                        user.DisplayName,
                                        user.Get("OrganNames").ToString(),
                                        $"{avgEvaluation}星",
                                    };
                            if (items != null && items.Count > 0)
                            {
                                foreach (var item in items)
                                {
                                    head.Add(item.Title);

                                    var userItemAnswer = await _studyCourseEvaluationItemUserRepository.GetAsync(request.PlanId, request.Id, euser.UserId, euser.EvaluationId, item.Id);
                                    if (userItemAnswer != null)
                                    {
                                        if (item.TextContent)
                                        {
                                            rowValue.Add($"{userItemAnswer.TextContent}");
                                        }
                                        else
                                        {
                                            rowValue.Add($"{userItemAnswer.StarValue}星");
                                        }
                                    }
                                    else
                                    {
                                        rowValue.Add("");
                                    }
                                }
                                rows.Add(rowValue);
                                index++;
                            }
                        }
                    }
                }
            }



            DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(filePath));
            FileUtils.DeleteFileIfExists(filePath);

            ExcelUtils.Write(filePath, head, rows);

            var downloadUrl = _pathManager.GetDownloadFilesUrl(fileName);

            await _authManager.AddAdminLogAsync($"导出评价(课程：{course.Name})");
            await _authManager.AddStatLogAsync(StatType.Export, $"导出评价(课程：{course.Name})", 0, string.Empty, new StringResult { Value = downloadUrl });

            return new StringResult
            {
                Value = downloadUrl
            };
        }

    }
}
