using Datory;
using Microsoft.AspNetCore.Mvc;
using System;
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
        [HttpGet, Route(RouteUser)]
        public async Task<ActionResult<GetUserResult>> Submit([FromQuery] GetUserRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Manage))
            {
                return this.NoAuth();
            }

            var planCourse = await _studyPlanCourseRepository.GetAsync(request.PlanId, request.Id);
            if (request.PlanId > 0)
            {
                var planUserIds = await _studyPlanUserRepository.GetUserIdsAsync(request.PlanId);
                if (planUserIds != null && planUserIds.Count > 0)
                {
                    foreach (var userId in planUserIds)
                    {
                        var exsist = await _studyCourseUserRepository.ExistsAsync(userId, request.PlanId, request.Id);
                        if (!exsist && !planCourse.IsSelectCourse)
                        {
                            var user = await _userRepository.GetByUserIdAsync(userId);
                            await _studyCourseUserRepository.InsertAsync(new StudyCourseUser
                            {
                                UserId = user.Id,
                                PlanId = request.PlanId,
                                CourseId = request.Id,
                                IsSelectCourse = planCourse.IsSelectCourse,
                                CompanyId = user.CompanyId,
                                DepartmentId = user.DepartmentId,
                                CreatorId = user.CreatorId,
                                State = StudyStatType.Weikaishi,
                                TotalDuration = 0,
                                BeginStudyDateTime = DateTime.Now,
                                LastStudyDateTime = DateTime.Now,
                                KeyWordsAdmin = await _organManager.GetUserKeyWords(user.Id),
                                KeyWords = planCourse.CourseName,
                            });
                        }

                    }
                }
            }

            var course = await _studyCourseRepository.GetAsync(request.Id);
            var (total, list) = await _studyCourseUserRepository.GetListAsync(request.PlanId, request.Id, request.KeyWords, request.State, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    var user = await _organManager.GetUser(item.UserId);
                    if (user == null)
                    {
                        user = new User();
                    }

                    item.Set("User", user);
                    item.Set("StateStr", item.State.GetDisplayName());


                    decimal maxCj = -1;
                    if (request.PlanId > 0)
                    {
                        if ((planCourse != null && planCourse.ExamId > 0))
                        {
                            maxCj = await _examPaperStartRepository.GetMaxScoreAsync(item.UserId, planCourse.ExamId, request.PlanId, item.CourseId);
                        }
                    }
                    else
                    {
                        if (course.ExamId > 0)
                        {
                            maxCj = await _examPaperStartRepository.GetMaxScoreAsync(item.UserId, course.ExamId, 0, course.Id);
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

                }
            }

            return new GetUserResult
            {
                Course = course,
                List = list,
                Total = total,
            };
        }


        [HttpPost, Route(RouteUserExport)]
        public async Task<ActionResult<StringResult>> UserExport([FromBody] GetUserRequest request)
        {
            var course = await _studyCourseRepository.GetAsync(request.Id);
            if (request.PlanId > 0)
            {
                var plancCourse = await _studyPlanCourseRepository.GetAsync(request.PlanId, request.Id);
                course.Name = plancCourse.CourseName;
            }

            var fileName = $"{course.Name}-学员列表.xlsx";
            var filePath = _pathManager.GetDownloadFilesPath(fileName);

            DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(filePath));
            FileUtils.DeleteFileIfExists(filePath);

            var head = new List<string>
            {
                "序号",
                "账号",
                "姓名",
                "组织",
                "学分",
                "学时",
                "成绩",
                "评价",
                "状态",
                "完成时间"
            };
            if (course.OffLine)
            {
                head.Add("上课状态");
            }
            var rows = new List<List<string>>();

            var (total, list) = await _studyCourseUserRepository.GetListAsync(request.PlanId, request.Id, request.KeyWords, request.State, 1, int.MaxValue);
            if (total > 0)
            {
                var index = 1;

                foreach (var item in list)
                {

                    var user = await _organManager.GetUser(item.UserId);
                    if (user == null)
                    {
                        user = new User();
                    }

                    decimal maxCj = -1;
                    if (request.PlanId > 0)
                    {
                        var planCourse = await _studyPlanCourseRepository.GetAsync(request.PlanId, item.CourseId);
                        if ((planCourse != null && planCourse.ExamId > 0))
                        {
                            maxCj = await _examPaperStartRepository.GetMaxScoreAsync(item.UserId, planCourse.ExamId, request.PlanId, item.CourseId);
                        }
                    }
                    else
                    {
                        if (course.ExamId > 0)
                        {
                            maxCj = await _examPaperStartRepository.GetMaxScoreAsync(item.UserId, course.ExamId, 0, course.Id);
                        }
                    }
                    var rowValue = new List<string>() {

                                index.ToString(),
                                user.UserName,
                                user.DisplayName,
                                user.Get("OrganNames").ToString(),
                                $"{item.Credit}",
                                 $"{TranslateUtils.ToMinuteAndSecond(item.TotalDuration)}/{TranslateUtils.ToMinuteAndSecond(course.TotalEvaluation)}",
                               maxCj >= 0 ? maxCj.ToString() : "/",
                               item.AvgEvaluation.ToString(),
                               item.State.GetDisplayName(),
                              item.OverStudyDateTime.HasValue ? item.OverStudyDateTime.Value.ToString() : "/"
                            };
                    if (course.OffLine)
                    {
                        if (item.IsSignin)
                        {
                            rowValue.Add("已上课");
                        }
                        else
                        {
                            rowValue.Add("未上课");
                        }
                    }
                    rows.Add(rowValue);

                    index++;

                }
            }

            ExcelUtils.Write(filePath, head, rows);

            var downloadUrl = _pathManager.GetDownloadFilesUrl(fileName);

            await _authManager.AddAdminLogAsync($"导出学员列表(课程：{course.Name})");
            await _authManager.AddStatLogAsync(StatType.Export, $"导出学员列表(课程：{course.Name})", 0, string.Empty, new StringResult { Value = downloadUrl });

            return new StringResult
            {
                Value = downloadUrl
            };
        }

        [HttpPost, Route(RouteUserOfflineOver)]
        public async Task<ActionResult<BoolResult>> OfflineOver([FromBody] GetSetOfflineRequest request)
        {
            var course = await _studyCourseRepository.GetAsync(request.CourseId);
            if (request.PlanId > 0)
            {
                var planCourse = await _studyPlanCourseRepository.GetAsync(request.PlanId, request.CourseId);
                course.Credit = planCourse.Credit;
            }
            if (request.CourseUserIds != null && request.CourseUserIds.Count > 0)
            {

                foreach (var id in request.CourseUserIds)
                {
                    var courseUser = await _studyCourseUserRepository.GetAsync(id);
                    courseUser.Credit = course.Credit;
                    courseUser.IsSignin = true;
                    courseUser.State = StudyStatType.Yiwancheng;
                    courseUser.OverStudyDateTime = DateTime.Now;
                    await _studyCourseUserRepository.UpdateAsync(courseUser);

                    var user = await _userRepository.GetByUserIdAsync(courseUser.UserId);
                    if (user != null)
                    {
                        await _authManager.AddPointsLogAsync(PointType.PointCourseOver, user, course.Id, course.Name, false);
                    }
                }
            }
            else
            {
                var (total, list) = await _studyCourseUserRepository.GetListAsync(request.PlanId, request.CourseId, request.KeyWords, request.State, 1, int.MaxValue);
                if (total > 0)
                {
                    foreach (var item in list)
                    {
                        item.Credit = course.Credit;
                        item.IsSignin = true;
                        item.State = StudyStatType.Yiwancheng;
                        item.OverStudyDateTime = DateTime.Now;
                        await _studyCourseUserRepository.UpdateAsync(item);

                        var user = await _userRepository.GetByUserIdAsync(item.UserId);
                        if (user != null)
                        {
                            await _authManager.AddPointsLogAsync(PointType.PointCourseOver, user, course.Id, course.Name, false);
                        }
                    }
                }
            }
            return new BoolResult
            {
                Value = true
            };
        }
        [HttpPost, Route(RouteUserOfflineSet)]
        public async Task<ActionResult<BoolResult>> OfflineSet([FromBody] GetSetOfflineRequest request)
        {
            if (request.CourseUserIds != null && request.CourseUserIds.Count > 0)
            {
                foreach (var id in request.CourseUserIds)
                {
                    var courseUser = await _studyCourseUserRepository.GetAsync(id);
                    courseUser.IsSignin = true;
                    await _studyCourseUserRepository.UpdateAsync(courseUser);
                }
            }
            else
            {
                var (total, list) = await _studyCourseUserRepository.GetListAsync(request.PlanId, request.CourseId, request.KeyWords, request.State, 1, int.MaxValue);
                if (total > 0)
                {
                    foreach (var item in list)
                    {
                        item.IsSignin = true;
                        await _studyCourseUserRepository.UpdateAsync(item);
                    }
                }
            }
            return new BoolResult
            {
                Value = true
            };
        }
    }


}
