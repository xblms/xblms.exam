using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Study
{
    public partial class StudyCourseManagerController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Manage))
            {
                return this.NoAuth();
            }

            var auth = await _authManager.GetAdminAuth();

            var course = await _studyCourseRepository.GetAsync(request.Id);

            var userOverTotal = await _studyCourseUserRepository.GetOverCountByAnalysisAsync(request.PlanId, request.Id, true);
            var userTotal = await _studyCourseUserRepository.GetOverCountByAnalysisAsync(request.PlanId, request.Id, null);
            if (request.PlanId > 0)
            {
                var planCourse = await _studyPlanCourseRepository.GetAsync(request.PlanId, request.Id);
                if (!planCourse.IsSelectCourse)
                {
                    userTotal = await _studyPlanUserRepository.GetCountAsync(request.PlanId, "");
                }
                course.Name = planCourse.CourseName;
                course.ExamId = planCourse.ExamId;
                course.ExamQuestionnaireId = planCourse.ExamQuestionnaireId;
                course.StudyCourseEvaluationId = planCourse.StudyCourseEvaluationId;

                if (planCourse.StudyCourseEvaluationId > 0)
                {
                    var (starUser, starTotal) = await _studyCourseUserRepository.GetEvaluation(request.PlanId, request.Id);
                    course.TotaEvaluationlUser = starUser;
                    course.TotalAvgEvaluation = starTotal;
                }
            }
            course.Set("EvaluationAvg", TranslateUtils.ToAvg((double)course.TotalAvgEvaluation, course.TotaEvaluationlUser));
            course.Set("EvaluationUser", course.TotaEvaluationlUser);

            var cerId = 0;
            if (course.ExamId > 0)
            {
                var paper = await _examPaperRepository.GetAsync(course.ExamId);
                if (paper != null && paper.CerId > 0)
                {
                    var cer = await _examCerRepository.GetAsync(paper.CerId);
                    if (cer != null)
                    {
                        cerId = cer.Id;
                    }
                }
            }
            course.Set("CerId", cerId);
            course.Set("TotalUser", userTotal);
            course.Set("TotalPassUser", userOverTotal);


            return new GetResult
            {
                Item = course,
            };

        }

    }
}
