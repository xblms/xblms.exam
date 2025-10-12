using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Users
{
    public partial class UsersController
    {
        [HttpGet, Route(RouteDoc)]
        public async Task<ActionResult<GetResults>> GetDoc([FromQuery] GetRequest request)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var config = await _configRepository.GetAsync();
            var adminAuth = await _authManager.GetAdminAuth();
            var group = await _userGroupRepository.GetAsync(request.GroupId);
            var (total, list) = await _userRepository.GetListAsync(adminAuth, request.OrganId, request.OrganType, group, request.LastActivityDate, request.Keyword, request.Order, request.Offset, request.Limit);

            if (total > 0)
            {
                foreach (var user in list)
                {
                    await _organManager.GetUser(user);

                    if (config.SystemCode == SystemCode.Elearning)
                    {
                        var totalCredit = await _databaseManager.StudyCourseUserRepository.Analysis_GetTotalCreditAsync(user.Id, request.DateFrom, request.DateTo);
                        var totalDuration = await _databaseManager.StudyCourseWareUserRepository.Analysis_GetTotalDurationAsync(user.Id, request.DateFrom, request.DateTo);
                        var (planTotal, planOverTotal, planDabiaoTotal) = await _databaseManager.StudyPlanUserRepository.Analysis_GetTotalAsync(user.Id, request.DateFrom, request.DateTo);
                        var (courseTotal, courseOverTotal) = await _databaseManager.StudyCourseUserRepository.Analysis_GetTotalAsync(user.Id, request.DateFrom, request.DateTo);
                        user.Set("Credit", totalCredit);
                        user.Set("Duration", totalDuration);
                        user.Set("Plan", $"{planTotal}/{planOverTotal}/{planDabiaoTotal}");
                        user.Set("Course", $"{courseTotal}/{courseOverTotal}");
                    }

                    var totalCer = await _databaseManager.ExamCerUserRepository.Analysis_GetTotalAsync(user.Id, request.DateFrom, request.DateTo);
                    var (answerTotal, rightTotal) = await _databaseManager.ExamPracticeRepository.Analysis_GetTotalAsync(user.Id, request.DateFrom, request.DateTo);
                    var (assTotal, assSubmitTotal) = await _databaseManager.ExamAssessmentUserRepository.Analysis_GetTotalAsync(user.Id, request.DateFrom, request.DateTo);
                    var (qTotal, qSubmitTotal) = await _databaseManager.ExamQuestionnaireUserRepository.Analysis_GetTotalAsync(user.Id, request.DateFrom, request.DateTo);
                    var (examTotal, examPassTotal) = await _databaseManager.ExamPaperStartRepository.Analysis_GetTotalAsync(user.Id, false, request.DateFrom, request.DateTo);
                    var (examMoniTotal, examMoniPassTotal) = await _databaseManager.ExamPaperStartRepository.Analysis_GetTotalAsync(user.Id, true, request.DateFrom, request.DateTo);
                    var loginTotal = await _databaseManager.LogRepository.Analysis_GetTotalAsync(user.Id, request.DateFrom, request.DateTo);
                    var pointTotal = await _databaseManager.PointLogRepository.Analysis_GetTotalAsync(user.Id, request.DateFrom, request.DateTo);
                    var pointSurplusTotal = await _databaseManager.PointShopUserRepository.Analysis_GetListAsync(user.Id, request.DateFrom, request.DateTo);

                    user.Set("PointTotal", $"{pointTotal}/{pointSurplusTotal}");
                    user.Set("LoginTotal", loginTotal);
                    user.Set("Exam", $"{examTotal}/{examPassTotal}");
                    user.Set("ExamMoni", $"{examMoniTotal}/{examMoniPassTotal}");
                    user.Set("ExamQ", $"{qTotal}/{qSubmitTotal}");
                    user.Set("ExamAss", $"{assTotal}/{assSubmitTotal}");
                    user.Set("Practice", $"{answerTotal}/{rightTotal}");
                    user.Set("Cer", totalCer);
                }
            }
            return new GetResults
            {
                Users = list,
                Count = total,
                SystemCode = config.SystemCode
            };
        }
    }
}
