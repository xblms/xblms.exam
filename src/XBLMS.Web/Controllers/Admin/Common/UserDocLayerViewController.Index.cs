using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class UserDocLayerViewController
    {
        [HttpGet, Route(RouteIndex)]
        public async Task<ActionResult<GetIndexResult>> GetIndex([FromQuery] GetRequest request)
        {
            var config = await _configRepository.GetAsync();
            var user = await _userRepository.GetByUserIdAsync(request.Id);
            var totalCredit = await _databaseManager.StudyCourseUserRepository.Analysis_GetTotalCreditAsync(request.Id, request.DateFrom, request.DateTo);
            var totalDuration = await _databaseManager.StudyCourseWareUserRepository.Analysis_GetTotalDurationAsync(request.Id, request.DateFrom, request.DateTo);
            var totalCer = await _databaseManager.ExamCerUserRepository.Analysis_GetTotalAsync(request.Id, request.DateFrom, request.DateTo);
            var (planTotal, planOverTotal, planDabiaoTotal) = await _databaseManager.StudyPlanUserRepository.Analysis_GetTotalAsync(request.Id, request.DateFrom, request.DateTo);
            var (courseTotal, courseOverTotal) = await _databaseManager.StudyCourseUserRepository.Analysis_GetTotalAsync(request.Id, request.DateFrom, request.DateTo);
            var (answerTotal, rightTotal) = await _databaseManager.ExamPracticeRepository.Analysis_GetTotalAsync(request.Id, request.DateFrom, request.DateTo);
            var (assTotal, assSubmitTotal) = await _databaseManager.ExamAssessmentUserRepository.Analysis_GetTotalAsync(request.Id, request.DateFrom, request.DateTo);
            var (qTotal, qSubmitTotal) = await _databaseManager.ExamQuestionnaireUserRepository.Analysis_GetTotalAsync(request.Id, request.DateFrom, request.DateTo);
            var (examTotal, examPassTotal) = await _databaseManager.ExamPaperStartRepository.Analysis_GetTotalAsync(request.Id, false, request.DateFrom, request.DateTo);
            var (examMoniTotal, examMoniPassTotal) = await _databaseManager.ExamPaperStartRepository.Analysis_GetTotalAsync(request.Id, true, request.DateFrom, request.DateTo);
            var loginTotal = await _databaseManager.LogRepository.Analysis_GetTotalAsync(user.Id, request.DateFrom, request.DateTo);
            var pointTotal = await _databaseManager.PointLogRepository.Analysis_GetTotalAsync(user.Id, request.DateFrom, request.DateTo);
            var pointSurplusTotal = await _databaseManager.PointShopUserRepository.Analysis_GetListAsync(user.Id, request.DateFrom, request.DateTo);

            return new GetIndexResult
            {
                SystemCode = config.SystemCode,
                TotalCers = totalCer.ToString(),
                TotalCredit = totalCredit.ToString(),
                TotalDuration = totalDuration.ToString(),
                TotalLogins = loginTotal.ToString(),
                TotalPoints = $"{pointTotal}/{pointSurplusTotal}",
                PlanTotal = planTotal,
                PlanDabiaoTotal = planDabiaoTotal,
                PlanOverTotal = planOverTotal,
                CourseOverTotal = courseOverTotal,
                CourseTotal = courseTotal,
                ExamPracticeAnswerTotal = answerTotal,
                ExamPracticeRightTotal = rightTotal,
                ExamAssTotal = assTotal,
                ExamAssSubmitTotal = assSubmitTotal,
                ExamQTotal = qTotal,
                ExamQSubmitTotal = qSubmitTotal,
                ExamTotal = examTotal,
                ExamPassTotal = examPassTotal,
                ExamMoinPasaTotal = examMoniPassTotal,
                ExamMoniTotal = examMoniTotal,
            };
        }
    }
}
