using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Points
{
    public partial class UtilitiesPointShopController
    {
        [HttpGet, Route(RouteConfig)]
        public async Task<ActionResult<GetPointConfig>> GetConfig()
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var config = await _configRepository.GetAsync();

            return new GetPointConfig
            {
                SystemCode = config.SystemCode,
                PointLogin = config.PointLogin,
                PointLoginDayMax = config.PointLoginDayMax,
                PointPlanOver = config.PointPlanOver,
                PointPlanOverDayMax = config.PointPlanOverDayMax,
                PointVideo = config.PointVideo,
                PointVideoDayMax = config.PointVideoDayMax,
                PointDocument = config.PointDocument,
                PointDocumentDayMax = config.PointDocumentDayMax,
                PointCourseOver = config.PointCourseOver,
                PointCourseOverDayMax = config.PointCourseOverDayMax,
                PointExam = config.PointExam,
                PointExamDayMax = config.PointExamDayMax,
                PointExamPass = config.PointExamPass,
                PointExamPassDayMax = config.PointExamPassDayMax,
                PointExamFull = config.PointExamFull,
                PointExamFullDayMax = config.PointExamFullDayMax,
                PointExamQ = config.PointExamQ,
                PointExamQDayMax = config.PointExamQDayMax,
                PointExamAss = config.PointExamAss,
                PointExamAssDayMax = config.PointExamAssDayMax,
                PointExamPractice = config.PointExamPractice,
                PointExamPracticeDayMax = config.PointExamPracticeDayMax,
                PointExamPracticeRight = config.PointExamPracticeRight,
                PointExamPracticeRightDayMax = config.PointExamPracticeRightDayMax,
                PointEvaluation = config.PointEvaluation,
                PointEvaluationDayMax = config.PointEvaluationDayMax
            };
        }

        [HttpPost, Route(RouteConfig)]
        public async Task<ActionResult<BoolResult>> SubmitConfig([FromBody] GetPointConfig request)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var config = await _configRepository.GetAsync();

            config.PointLogin = request.PointLogin;
            config.PointLoginDayMax = request.PointLoginDayMax;
            config.PointPlanOver = config.PointPlanOver;
            config.PointPlanOverDayMax = request.PointPlanOverDayMax;
            config.PointVideo = request.PointVideo;
            config.PointVideoDayMax = request.PointVideoDayMax;
            config.PointDocument = request.PointDocument;
            config.PointDocumentDayMax = request.PointDocumentDayMax;
            config.PointCourseOver = request.PointCourseOver;
            config.PointCourseOverDayMax = request.PointCourseOverDayMax;
            config.PointExam = request.PointExam;
            config.PointExamDayMax = request.PointExamDayMax;
            config.PointExamPass = request.PointExamPass;
            config.PointExamPassDayMax = request.PointExamPassDayMax;
            config.PointExamFull = request.PointExamFull;
            config.PointExamFullDayMax = request.PointExamFullDayMax;
            config.PointExamQ = request.PointExamQ;
            config.PointExamQDayMax = request.PointExamQDayMax;
            config.PointExamAss = request.PointExamAss;
            config.PointExamAssDayMax = request.PointExamAssDayMax;
            config.PointExamPractice = request.PointExamPractice;
            config.PointExamPracticeDayMax = request.PointExamPracticeDayMax;
            config.PointExamPracticeRight = request.PointExamPracticeRight;
            config.PointExamPracticeRightDayMax = request.PointExamPracticeRightDayMax;
            config.PointEvaluation = request.PointEvaluation;
            config.PointEvaluationDayMax = request.PointEvaluationDayMax;


            await _configRepository.UpdateAsync(config);

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
