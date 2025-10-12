using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace XBLMS.Web.Controllers.Home
{
    public partial class DashboardController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            user = await _organManager.GetUser(user.Id);

            var (allPassPercent, allTotal, moniPassPercent, moniTotal, passPercent, total) = await _examManager.AnalysisMorePass(user.Id);
            var (answerTmTotal, answerPercent, allTmTotal, allPercent, collectTmTotal, collectPercent, wrongTmTotal, wrongPercent) = await _examManager.AnalysisPractice(user.Id);

            var cerTotal = await _examCerUserRepository.CountAsync(user.Id);

            var examQTotal = await _examQuestionnaireUserRepository.CountAsync(user.Id);
            var examAssTotal = await _examAssessmentUserRepository.CountAsync(user.Id);

            var (planTotalCredit, planTotalOverCredit) = await _studyPlanUserRepository.GetCreditAsync(user.Id);
            var (totalCourse, totalOverCourse) = await _studyCourseUserRepository.GetTotalAsync(user.Id);
            var totalDuration = await _studyCourseUserRepository.GetTotalDurationAsync(user.Id);

            var pointNotice = await _authManager.PointNotice(user.Id);
            var config = await _configRepository.GetAsync();

            return new GetResult
            {
                SystemCode = config.SystemCode,
                PointNotice = pointNotice,
                User = user,
                AllPercent = allPassPercent,
                ExamTotal = total,
                ExamPercent = allPassPercent,
                ExamMoniPercent = moniPassPercent,
                ExamMoniTotal = moniTotal,
                ExamCerTotal = cerTotal,
                ExamQTotal = examQTotal,
                ExamAssTotal = examAssTotal,

                PracticeAnswerTmTotal = answerTmTotal,
                PracticeAnswerPercent = answerPercent,
                PracticeAllTmTotal = allTmTotal,
                PracticeAllPercent = allPercent,
                PracticeCollectTmTotal = collectTmTotal,
                PracticeCollectPercent = collectPercent,
                PracticeWrongTmTotal = wrongTmTotal,
                PracticeWrongPercent = wrongPercent,

                StudyPlanTotalCredit = planTotalCredit,
                StudyPlanTotalOverCredit = planTotalOverCredit,
                TotalCourse = totalCourse,
                TotalOverCourse = totalOverCourse,
                TotalDuration = totalDuration,

                Version = _settingsManager.Version,
                VersionName = _settingsManager.VersionName
            };
        }
    }
}
