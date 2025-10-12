using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Home
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.User)]
    [Route(Constants.ApiHomePrefix)]
    public partial class DashboardController : ControllerBase
    {
        private const string Route = "dashboard";

        private readonly ISettingsManager _settingsManager;
        private readonly IConfigRepository _configRepository;
        private readonly IAuthManager _authManager;
        private readonly IOrganManager _organManager;
        private readonly IExamManager _examManager;
        private readonly IExamQuestionnaireUserRepository _examQuestionnaireUserRepository;
        private readonly IExamCerUserRepository _examCerUserRepository;

        private readonly IExamAssessmentUserRepository _examAssessmentUserRepository;
        private readonly IStudyPlanUserRepository _studyPlanUserRepository;
        private readonly IStudyCourseUserRepository _studyCourseUserRepository;

        public DashboardController(IConfigRepository configRepository,
            ISettingsManager settingsManager,
            IOrganManager organManager,
            IAuthManager authManager,
            IExamManager examManager,
            IExamQuestionnaireUserRepository examQuestionnaireUserRepository,
            IExamCerUserRepository examCerUserRepository,
            IExamAssessmentUserRepository examAssessmentUserRepository,
            IStudyPlanUserRepository studyPlanUserRepository,
            IStudyCourseUserRepository studyCourseUserRepository)
        {
            _settingsManager = settingsManager;
            _configRepository = configRepository;
            _authManager = authManager;
            _organManager = organManager;
            _examManager = examManager;
            _examQuestionnaireUserRepository = examQuestionnaireUserRepository;
            _examCerUserRepository = examCerUserRepository;
            _examAssessmentUserRepository = examAssessmentUserRepository;
            _studyPlanUserRepository = studyPlanUserRepository;
            _studyCourseUserRepository = studyCourseUserRepository;
        }

        public class GetRequest
        {
            public bool IsApp { get; set; }
        }

        public class GetResult
        {
            public string VersionName { get; set; }
            public SystemCode SystemCode { get; set; }
            public PointNotice PointNotice { get; set; }
            public User User { get; set; }
            public double AllPercent { get; set; }
            public double ExamPercent { get; set; }
            public double ExamMoniPercent { get; set; }
            public int ExamTotal { get; set; }
            public int ExamMoniTotal { get; set; }
            public int ExamCerTotal { get; set; }
            public int ExamQTotal { get; set; }
            public int ExamAssTotal { get; set; }

            public int PracticeAnswerTmTotal { get; set; }
            public double PracticeAnswerPercent { get; set; }
            public int PracticeAllTmTotal { get; set; }
            public double PracticeAllPercent { get; set; }
            public int PracticeCollectTmTotal { get; set; }
            public double PracticeCollectPercent { get; set; }
            public int PracticeWrongTmTotal { get; set; }
            public double PracticeWrongPercent { get; set; }

            public decimal StudyPlanTotalCredit { get; set; }
            public decimal StudyPlanTotalOverCredit { get; set; }
            public int TotalCourse { get; set; }
            public int TotalOverCourse { get; set; }
            public long TotalDuration { get; set; }
            public string Version { get; set; }

        }

    }
}
