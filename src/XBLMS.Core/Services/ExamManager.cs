using System;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Models;
using XBLMS.Services;

namespace XBLMS.Core.Services
{
    public partial class ExamManager : IExamManager
    {
        private readonly ICreateManager _createManager;
        private readonly ITaskManager _taskManager;
        private readonly IOrganManager _organManager;
        private readonly IDatabaseManager _databaseManager;

        public ExamManager(IOrganManager organManager,
            ICreateManager createManager,
            ITaskManager taskManager,
            IDatabaseManager databaseManager)
        {
            _organManager = organManager;
            _createManager = createManager;
            _taskManager = taskManager;
            _databaseManager = databaseManager;
        }



        public async Task GetPaperInfo(ExamPaper paper, User user, int planId = 0, int courseId = 0, bool cjList = false)
        {
            var myExamTimes = await _databaseManager.ExamPaperStartRepository.CountAsync(paper.Id, user.Id);
            var startId = await _databaseManager.ExamPaperStartRepository.GetNoSubmitIdAsync(paper.Id, user.Id);
            var cerName = "";
            if (paper.CerId > 0)
            {
                var cer = await _databaseManager.ExamCerRepository.GetAsync(paper.CerId);
                if (cer != null)
                {
                    cerName = cer.Name;
                }
            }

            var examUser = await _databaseManager.ExamPaperUserRepository.GetAsync(paper.Id, user.Id);

            var courseName = "学习任务：xxxxx";
            if (courseId > 0 || planId > 0)
            {
                if (examUser == null)
                {
                    var adminKeyWords = await _organManager.GetUserKeyWords(user.Id);

                    if (planId > 0)
                    {
                        var plan = await _databaseManager.StudyPlanRepository.GetAsync(planId);
                        if (plan != null)
                        {
                            adminKeyWords = $"{adminKeyWords}-{plan.PlanName}";
                        }

                    }
                    if (courseId > 0)
                    {
                        var course = await _databaseManager.StudyPlanCourseRepository.GetAsync(planId, courseId);
                        if (course != null)
                        {
                            adminKeyWords = $"{adminKeyWords}-{course.CourseName}";
                        }
                    }

                    var examUserId = await _databaseManager.ExamPaperUserRepository.InsertAsync(new ExamPaperUser
                    {
                        PlanId = planId,
                        CourseId = courseId,
                        ExamTimes = paper.ExamTimes,
                        ExamBeginDateTime = paper.ExamBeginDateTime,
                        ExamEndDateTime = paper.ExamEndDateTime,
                        ExamPaperId = paper.Id,
                        UserId = user.Id,
                        KeyWordsAdmin = await _organManager.GetUserKeyWords(user.Id),
                        KeyWords = paper.Title,
                        Locked = paper.Locked,
                        Moni = paper.Moni,
                        CompanyId = user.CompanyId,
                        DepartmentId = user.DepartmentId,
                        CreatorId = user.CreatorId,
                        CompanyParentPath = user.CompanyParentPath,
                        DepartmentParentPath = user.DepartmentParentPath,
                    });
                    examUser = await _databaseManager.ExamPaperUserRepository.GetAsync(examUserId);
                }
                if (planId > 0)
                {
                    if (courseId > 0)
                    {
                        var course = await _databaseManager.StudyPlanCourseRepository.GetAsync(planId, courseId);
                        if (course != null)
                        {
                            courseName = course.CourseName;
                        }
                    }
                    else
                    {
                        var plan = await _databaseManager.StudyPlanRepository.GetAsync(planId);
                        if (plan != null)
                        {
                            courseName = plan.PlanName;
                        }
                    }
                }
                else
                {
                    if (courseId > 0)
                    {
                        var course = await _databaseManager.StudyCourseRepository.GetAsync(courseId);
                        if (course != null)
                        {
                            courseName = course.Name;
                        }
                    }
                }

            }

            GetPaperStatus(examUser, paper);

            paper.Set("CourseName", courseName);
            paper.Set("ExamUserId", examUser.Id);
            paper.Set("PlanId", planId);
            paper.Set("CourseId", courseId);
            paper.Set("StartId", startId);
            paper.Set("CerName", cerName);
            paper.Set("ExamStartDateTimeStr", DateUtils.Format(examUser.ExamBeginDateTime, DateUtils.FormatStringDateTimeCN));
            paper.Set("ExamEndDateTimeStr", DateUtils.Format(examUser.ExamEndDateTime, DateUtils.FormatStringDateTimeCN));

            paper.Set("MyExamTimes", myExamTimes > examUser.ExamTimes ? examUser.ExamTimes : myExamTimes);
            paper.Set("UserExamTimes", examUser.ExamTimes);

            double longTime = 0;
            if (examUser.ExamBeginDateTime.Value > DateTime.Now)
            {
                var timeSpan = DateUtils.DateTimeToUnixTimestamp(examUser.ExamBeginDateTime.Value);
                longTime = timeSpan;
            }
            paper.Set("ExamStartDateTimeLong", longTime);

            var taskStartIds = _createManager.GetTaskStartIds();
            if (taskStartIds.Contains(startId))
            {
                paper.Set("ExamSubmiting", true);
            }
            else
            {
                paper.Set("ExamSubmiting", false);
            }
            if (cjList)
            {
                var cjlist = await _databaseManager.ExamPaperStartRepository.GetListAsync(paper.Id, user.Id);
                if (cjlist != null && cjlist.Count > 0)
                {
                    foreach (var cj in cjlist)
                    {
                        if (!paper.SecrecyScore)
                        {
                            cj.Score = 0;
                        }
                        cj.Set("UseTime", DateUtils.SecondToHms(cj.ExamTimeSeconds));

                    }
                }
                paper.Set("CjList", cjlist);
            }
        }
        public async Task GetPaperInfo(ExamPaper paper, User user, ExamPaperStart start)
        {
            var myExamTimes = await _databaseManager.ExamPaperStartRepository.CountAsync(paper.Id, user.Id);
            var startId = await _databaseManager.ExamPaperStartRepository.GetNoSubmitIdAsync(paper.Id, user.Id);
            var cerName = "";
            if (paper.CerId > 0)
            {
                var cer = await _databaseManager.ExamCerRepository.GetAsync(paper.CerId);
                if (cer != null)
                {
                    cerName = cer.Name;
                }
            }

            var examUser = await _databaseManager.ExamPaperUserRepository.GetAsync(paper.Id, user.Id);

            GetPaperStatus(examUser, paper);
            paper.Set("StartId", startId);
            paper.Set("CerName", cerName);
            paper.Set("ExamStartDateTimeStr", DateUtils.Format(start.BeginDateTime, DateUtils.FormatStringDateTimeCN));
            paper.Set("ExamEndDateTimeStr", DateUtils.Format(start.EndDateTime, DateUtils.FormatStringDateTimeCN));

            paper.Set("MyExamTimes", myExamTimes > examUser.ExamTimes ? examUser.ExamTimes : myExamTimes);
            paper.Set("UserExamTimes", examUser.ExamTimes);
        }
        public void GetPaperStatus(ExamPaperUser paperUser, ExamPaper paper)
        {
            var paperStatus = "success";
            var paperStatusStr = "未参加考试";
            if (paperUser.ExamEndDateTime.Value <= DateTime.Now)
            {
                paperStatus = "info";
                paperStatusStr = "已过期";
            }
            else if (paperUser.ExamBeginDateTime.Value >= DateTime.Now)
            {
                paperStatus = "danger";
                paperStatusStr = "未开始";
            }
            else
            {
                if (paperUser.ExamTimesSubmit > 0)
                {
                    paperStatus = "success";
                    paperStatusStr = "已参加";
                }
                else
                {
                    paperStatus = "warning";
                    paperStatusStr = "请参加考试";
                }
            }
            paper.Set("PaperStatus", paperStatus);
            paper.Set("PaperStatusStr", paperStatusStr);
        }
        public async Task GetQuestionnaireInfo(ExamQuestionnaire paper, User user)
        {
            var paperUser = await _databaseManager.ExamQuestionnaireUserRepository.GetAsync(0, 0, paper.Id, user.Id);
            paper.Set("ExamStartDateTimeStr", DateUtils.Format(paper.ExamBeginDateTime, DateUtils.FormatStringDateTimeCN));
            paper.Set("ExamEndDateTimeStr", DateUtils.Format(paper.ExamEndDateTime, DateUtils.FormatStringDateTimeCN));
            paper.Set("SubmitType", paperUser.SubmitType);
            paper.Set("State", DateTime.Now >= paper.ExamBeginDateTime.Value && DateTime.Now <= paper.ExamEndDateTime.Value);
        }
        public void GetExamAssessmentInfo(ExamAssessment ass, ExamAssessmentUser assUser, User user)
        {
            ass.Set("ExamStartDateTimeStr", DateUtils.Format(ass.ExamBeginDateTime, DateUtils.FormatStringDateTimeCN));
            ass.Set("ExamEndDateTimeStr", DateUtils.Format(ass.ExamEndDateTime, DateUtils.FormatStringDateTimeCN));
            ass.Set("SubmitType", assUser.SubmitType);
            ass.Set("State", DateTime.Now >= ass.ExamBeginDateTime.Value && DateTime.Now <= ass.ExamEndDateTime.Value);
            ass.Set("ConfigId", assUser.ConfigId);
            ass.Set("ConfigName", assUser.ConfigName);
        }
        public async Task<(bool Success, string msg)> CheckExam(int paperId, int userId)
        {
            var paper = await _databaseManager.ExamPaperRepository.GetAsync(paperId);
            var paperUser = await _databaseManager.ExamPaperUserRepository.GetAsync(paperId, userId);
            if (paper != null || paperUser != null)
            {
                var myTimes = await _databaseManager.ExamPaperStartRepository.CountAsync(paperId, userId);
                var times = paperUser.ExamTimes;
                if (times - myTimes <= 0)
                {
                    return (false, "剩余考试次数不足");
                }

                if (paperUser.ExamBeginDateTime.Value > DateTime.Now)
                {
                    return (false, "考试未开始，请耐心等待");
                }

                if (paperUser.ExamEndDateTime.Value < DateTime.Now)
                {
                    return (false, "考试已过期");
                }

                if (paperUser.Locked || paper.Locked)
                {
                    return (false, "考试已停用");
                }
            }
            else
            {
                return (false, "未找到试卷");
            }

            return (true, string.Empty);
        }

    }
}
