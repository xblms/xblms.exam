using Datory;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home.Study
{
    public partial class StudyCourseInfoController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) { return Unauthorized(); }

            var course = await _studyCourseRepository.GetAsync(request.CourseId);
            if (course == null) { return this.Error("课程已下架"); }

            var userCourse = await _studyCourseUserRepository.GetAsync(user.Id, request.PlanId, request.CourseId);
            if (userCourse == null)
            {
                var isSelectCourse = false;
                if (request.PlanId > 0)
                {
                    var planCourse = await _studyPlanCourseRepository.GetAsync(request.PlanId, request.CourseId);
                    if (planCourse != null && planCourse.IsSelectCourse)
                    {
                        isSelectCourse = true;
                    }
                }
                await _studyCourseUserRepository.InsertAsync(new StudyCourseUser
                {
                    UserId = user.Id,
                    PlanId = request.PlanId,
                    CourseId = request.CourseId,
                    IsSelectCourse = isSelectCourse,
                    CompanyId = user.CompanyId,
                    DepartmentId = user.DepartmentId,
                    CreatorId = user.Id,
                    State = StudyStatType.Xuexizhong,
                    TotalDuration = 0,
                    BeginStudyDateTime = DateTime.Now,
                    LastStudyDateTime = DateTime.Now,
                    KeyWordsAdmin = await _organManager.GetUserKeyWords(user.Id),
                    KeyWords = course.Name,
                    OffLine = course.OffLine,
                    CompanyParentPath = user.CompanyParentPath,
                    DepartmentParentPath = user.DepartmentParentPath,
                });
                userCourse = await _studyCourseUserRepository.GetAsync(user.Id, request.PlanId, request.CourseId);
                await _studyCourseRepository.IncrementTotalUserAsync(course.Id);
            }
            userCourse.LastStudyDateTime = DateTime.Now;
            await _studyCourseUserRepository.UpdateAsync(userCourse);

            var courseWareList = await _studyCourseWareRepository.GetListAsync(request.CourseId);
            if (courseWareList != null && courseWareList.Count > 0)
            {
                foreach (var courseWare in courseWareList)
                {
                    if (!await _studyCourseWareUserRepository.ExistsAsync(user.Id, request.PlanId, request.CourseId, courseWare.Id))
                    {
                        await _studyCourseWareUserRepository.InsertAsync(new StudyCourseWareUser
                        {
                            PlanId = request.PlanId,
                            CourseId = request.CourseId,
                            CourseWareId = courseWare.Id,
                            UserId = user.Id,
                            State = StudyStatType.Weikaishi,
                            TotalDuration = 0,
                            CompanyId = user.CompanyId,
                            DepartmentId = user.DepartmentId,
                            CreatorId = user.CreatorId,
                            CompanyParentPath = user.CompanyParentPath,
                            DepartmentParentPath = user.DepartmentParentPath,
                        });
                    }

                    courseWare.Url = await _pathManager.GetServerFileUrl(courseWare.Url);
                }
            }

            var wareTotal = 0;
            var wareOverTotal = 0;

            var userCourseWareList = await _studyCourseWareUserRepository.GetListAsync(user.Id, request.PlanId, request.CourseId);
            if (userCourseWareList != null && userCourseWareList.Count > 0)
            {
                var withCureent = false;
                foreach (var courseWare in userCourseWareList)
                {
                    wareTotal++;
                    if (courseWare.State == StudyStatType.Yiwancheng)
                    {
                        wareOverTotal++;
                    }
                    if (courseWare.StudyCurrent)
                    {
                        withCureent = true;
                    }
                    courseWare.Set("CourseWareInfo", await _studyCourseWareRepository.GetAsync(courseWare.CourseWareId));

                }
                if (!withCureent)
                {
                    userCourseWareList[0].StudyCurrent = true;
                }
            }

            if (request.PlanId > 0)
            {
                var planCourse = await _studyPlanCourseRepository.GetAsync(request.PlanId, request.CourseId);
                if (planCourse != null)
                {
                    course.Name = planCourse.CourseName;
                    course.Credit = planCourse.Credit;
                    course.Duration = planCourse.Duration;
                    course.StudyHour = planCourse.StudyHour;
                    course.ExamId = planCourse.ExamId;
                    course.ExamName = planCourse.ExamName;
                    course.ExamQuestionnaireId = planCourse.ExamQuestionnaireId;
                    course.ExamQuestionnaireName = planCourse.ExamQuestionnaireName;
                    course.StudyCourseEvaluationId = planCourse.StudyCourseEvaluationId;
                    course.StudyCourseEvaluationName = planCourse.StudyCourseEvaluationName;
                }
            }



            var courseState = userCourse.State.GetDisplayName();
            var courseStateStr = userCourse.State.GetDisplayName();

            var boolWare = wareOverTotal >= wareTotal;
            var boolExamPass = false;
            var boolExamQSubmit = false;
            var boolEvaluationSubmit = false;

            decimal maxCj = 0;
            if (course.ExamId > 0)
            {
                maxCj = await _examPaperStartRepository.GetMaxScoreAsync(user.Id, course.ExamId, request.PlanId, request.CourseId);
            }

            await _authManager.AddPointsLogAsync(PointType.PointVideo, user);
            var pointNotice = await _authManager.PointNotice(PointType.PointVideo, user.Id);

            if (userCourse.State != StudyStatType.Yiwancheng)
            {
                if (boolWare)
                {
                    if (course.ExamId > 0)
                    {
                        var paper = await _examPaperRepository.GetAsync(course.ExamId);
                        if (paper != null)
                        {
                            if (maxCj >= paper.PassScore)
                            {
                                boolExamPass = true;
                            }
                        }
                        else
                        {
                            boolExamPass = true;
                        }
                    }
                    else
                    {
                        boolExamPass = true;
                    }
                    if (course.ExamQuestionnaireId > 0)
                    {
                        var paperQUser = await _examQuestionnaireUserRepository.GetAsync(request.PlanId, request.CourseId, course.ExamQuestionnaireId, user.Id);
                        if (paperQUser != null && paperQUser.SubmitType == SubmitType.Submit)
                        {
                            boolExamQSubmit = true;
                        }
                    }
                    else
                    {
                        boolExamQSubmit = true;
                    }
                    if (course.StudyCourseEvaluationId > 0)
                    {
                        var evaluationUser = await _studyCourseEvaluationUserRepository.GetAsync(request.PlanId, request.CourseId, course.StudyCourseEvaluationId, user.Id);
                        if (evaluationUser != null)
                        {
                            boolEvaluationSubmit = true;
                        }
                    }
                    else
                    {
                        boolEvaluationSubmit = true;
                    }
                    if (!boolExamPass)
                    {
                        courseStateStr = "请完成课后考试并及格";
                    }
                    else
                    {
                        if (!boolEvaluationSubmit)
                        {
                            courseStateStr = "请完成课程评价";
                        }
                        if (!boolExamQSubmit)
                        {
                            courseStateStr = "请完成课后问卷";
                        }

                    }

                    if (boolExamPass && boolExamQSubmit && boolEvaluationSubmit)
                    {
                        userCourse.State = StudyStatType.Yiwancheng;
                        userCourse.Credit = course.Credit;
                        userCourse.OverStudyDateTime = DateTime.Now;

                        await _studyCourseUserRepository.UpdateAsync(userCourse);
                        courseStateStr = userCourse.State.GetDisplayName();

                        await _authManager.AddPointsLogAsync(PointType.PointCourseOver, user, course.Id, course.Name);
                        pointNotice = await _authManager.PointNotice(PointType.PointCourseOver, user.Id);

                        if (request.PlanId > 0)
                        {
                            var planUser = await _studyPlanUserRepository.GetAsync(request.PlanId, user.Id);
                            planUser.TotalCredit += course.Credit;
                            await _studyPlanUserRepository.UpdateAsync(planUser);
                        }
                    }
                }
                else
                {
                    courseStateStr = "请完成章节学习";
                }
            }

            course.Set("CourseUserInfo", userCourse);
            course.Set("CourseWareList", userCourseWareList);
            course.Set("WareTotal", wareTotal);
            course.Set("WareOverTotal", wareOverTotal);
            course.Set("State", userCourse.State == StudyStatType.Yiwancheng);
            course.Set("StateStr", userCourse.State.GetDisplayName());
            course.Set("StateStrMsg", courseStateStr);
            course.Set("MaxScore", maxCj);
            course.Set("BoolWare", boolWare);
            course.Set("BoolExam", boolExamPass);
            course.Set("BoolExamQ", boolExamQSubmit);
            course.Set("BoolEvaluation", boolEvaluationSubmit);

            return new GetResult
            {
                PointNotice = pointNotice,
                CourseInfo = course
            };
        }
    }
}
