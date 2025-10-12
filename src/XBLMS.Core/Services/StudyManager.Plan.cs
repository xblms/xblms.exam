using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Core.Services
{
    public partial class StudyManager
    {
        public async Task User_GetPlanInfo(StudyPlanUser planUser, bool isDetail = false)
        {
            var studyPlan = await _studyPlanRepository.GetAsync(planUser.PlanId);
            studyPlan.Set("PlanBeginDateTimeStr", studyPlan.PlanBeginDateTime.Value.ToString(DateUtils.FormatStringDateOnlyCN));
            studyPlan.Set("PlanEndDateTimeStr", studyPlan.PlanEndDateTime.Value.ToString(DateUtils.FormatStringDateOnlyCN));

            var planCourseTotal = await _studyPlanCourseRepository.CountAsync(studyPlan.Id, false);
            var planSelectCourseTotal = await _studyPlanCourseRepository.CountAsync(studyPlan.Id, false);

            studyPlan.Set("CourseTotal", planCourseTotal);
            studyPlan.Set("SelectCourseTotal", planSelectCourseTotal);


            var overCourseTotal = 0;
            var overSelectCourseTotal = 0;
            long overSelectCourseDurationTotal = 0;

            var courseList = new List<StudyCourse>();
            var courseSelectList = new List<StudyCourse>();

            var planUserCourseList = await _studyCourseUserRepository.GetListAsync(studyPlan.Id, planUser.UserId);

            if (planUserCourseList != null && planUserCourseList.Count > 0)
            {
                foreach (var planUserCourse in planUserCourseList)
                {

                    var planCourse = await _studyPlanCourseRepository.GetAsync(studyPlan.Id, planUserCourse.CourseId);

                    if (planUserCourse.State == StudyStatType.Yiwancheng || planUserCourse.State == StudyStatType.Yidabiao)
                    {
                        if (planCourse.IsSelectCourse)
                        {
                            overSelectCourseTotal++;
                        }
                        else
                        {
                            overCourseTotal++;
                        }
                    }
                    if (planCourse.IsSelectCourse)
                    {
                        overSelectCourseDurationTotal += planUserCourse.TotalDuration;
                    }

                }
            }

            if (isDetail)
            {
                var planCourseList = await _studyPlanCourseRepository.GetListAsync(false, studyPlan.Id);
                if (planCourseList != null && planCourseList.Count > 0)
                {
                    foreach (var planCourse in planCourseList)
                    {
                        var course = await _studyCourseRepository.GetAsync(planCourse.CourseId);
                        var courseUser = await _studyCourseUserRepository.GetAsync(planUser.UserId, planUser.PlanId, planCourse.CourseId);
                        await User_GetCourseInfoByCourseList(studyPlan.Id, course, courseUser);
                        course.Set("CourseType", "必修课");

                        course.Name = planCourse.CourseName;
                        course.Credit = planCourse.Credit;
                        course.Duration = planCourse.Duration;
                        course.StudyHour = planCourse.StudyHour;

                        courseList.Add(course);
                    }
                }
                var planSelectCourseList = await _studyPlanCourseRepository.GetListAsync(true, studyPlan.Id);
                if (planSelectCourseList != null && planSelectCourseList.Count > 0)
                {
                    foreach (var planCourse in planSelectCourseList)
                    {
                        var course = await _studyCourseRepository.GetAsync(planCourse.CourseId);
                        var courseUser = await _studyCourseUserRepository.GetAsync(planUser.UserId, planUser.PlanId, planCourse.CourseId);
                        await User_GetCourseInfoByCourseList(studyPlan.Id, course, courseUser);
                        course.Set("CourseType", "选修课");

                        course.Name = planCourse.CourseName;
                        course.Credit = planCourse.Credit;
                        course.Duration = planCourse.Duration;
                        course.StudyHour = planCourse.StudyHour;

                        courseSelectList.Add(course);
                    }
                }


                if (planUser.State == StudyStatType.Weikaishi)
                {
                    planUser.State = StudyStatType.Xuexizhong;
                    planUser.BeginStudyDateTime = DateTime.Now;
                    await _studyPlanUserRepository.UpdateAsync(planUser);
                }

            }

            var maxCj = await _examPaperStartRepository.GetMaxScoreAsync(planUser.UserId, studyPlan.ExamId, studyPlan.Id, 0);
            var courseOver = false;
            if (planUser.State != StudyStatType.Yiwancheng || planUser.State != StudyStatType.Yidabiao)
            {
                var overCourse = false;
                var overSelectCourse = false;
                if (studyPlan.TotalCount > 0)
                {
                    if (overCourseTotal >= studyPlan.TotalCount)
                    {
                        overCourse = true;
                    }
                }
                else
                {
                    overCourse = true;
                }

                if (studyPlan.SelectTotalCount > 0)
                {
                    var byCountOver = false;
                    var byDurationOver = false;
                    if (studyPlan.SelectCourseOverByCount)
                    {
                        if (overSelectCourseTotal >= studyPlan.SelectCourseOverCount)
                        {
                            byCountOver = true;
                        }
                    }
                    else
                    {
                        byCountOver = true;
                    }
                    if (studyPlan.SelectCourseOverByDuration)
                    {
                        if (overSelectCourseDurationTotal >= studyPlan.SelectCourseOverMinute * 60)
                        {
                            byDurationOver = true;
                        }
                    }
                    else
                    {
                        byDurationOver = true;
                    }
                    if (byCountOver && byDurationOver)
                    {
                        overSelectCourse = true;
                    }
                }
                else
                {
                    overSelectCourse = true;
                }

                if (overSelectCourse && overCourse)
                {
                    courseOver = true;

                    var examPass = false;

                    if (studyPlan.ExamId > 0)
                    {
                        var examPaper = await _examPaperRepository.GetAsync(studyPlan.ExamId);
                        if (examPaper != null)
                        {
                            if (maxCj > examPaper.PassScore)
                            {
                                examPass = true;
                            }
                        }
                        else
                        {
                            examPass = true;
                        }
                    }
                    else
                    {
                        examPass = true;
                    }

                    if (examPass)
                    {
                        planUser.State = StudyStatType.Yiwancheng;

                        if (planUser.TotalCredit >= studyPlan.PlanCredit)
                        {
                            planUser.State = StudyStatType.Yidabiao;
                        }
                        planUser.LastStudyDateTime = DateTime.Now;
                        planUser.OverStudyDateTime = DateTime.Now;
                        await _studyPlanUserRepository.UpdateAsync(planUser);
                    }

                }
            }

            if (DateTime.Now > studyPlan.PlanEndDateTime)
            {
                if (planUser.State != StudyStatType.Yiwancheng && planUser.State != StudyStatType.Yidabiao && planUser.State != StudyStatType.Weidabiao)
                {
                    planUser.State = StudyStatType.Weidabiao;
                    await _studyPlanUserRepository.UpdateAsync(planUser);
                }
            }

            planUser.Set("OverCourseTotal", overCourseTotal);
            planUser.Set("OverSelectCourseTotal", overSelectCourseTotal);
            planUser.Set("OverSelectCourseDurationTotal", overSelectCourseDurationTotal);
            planUser.Set("CourseList", courseList);
            planUser.Set("CourseSelectList", courseSelectList);
            planUser.Set("CourseOver", courseOver);
            planUser.Set("MaxScore", maxCj);

            var isStudy = true;
            if (studyPlan.PlanBeginDateTime.Value > DateTime.Now || studyPlan.PlanEndDateTime.Value < DateTime.Now)
            {
                isStudy = false;
            }
            planUser.Set("IsStudy", isStudy);
            planUser.Set("Plan", studyPlan);
        }
    }
}
