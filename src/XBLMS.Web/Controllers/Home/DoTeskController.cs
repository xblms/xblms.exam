using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Home
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.User)]
    [Route(Constants.ApiHomePrefix)]
    public partial class DoTeskController : ControllerBase
    {
        private const string Route = "doTask";

        private readonly IAuthManager _authManager;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamPaperUserRepository _examPaperUserRepository;
        private readonly IExamQuestionnaireUserRepository _examQuestionnaireUserRepository;
        private readonly IExamQuestionnaireRepository _examQuestionnaireRepository;
        private readonly IExamAssessmentUserRepository _examAssessmentUserRepository;
        private readonly IExamAssessmentRepository _examAssessmentRepository;
        private readonly IStudyPlanUserRepository _studyPlanUserRepository;
        private readonly IStudyManager _studyManager;
        private readonly IStudyPlanRepository _studyPlanRepository;

        public DoTeskController(IAuthManager authManager,
            IExamPaperUserRepository examPaperUserRepository,
            IExamPaperRepository examPaperRepository,
            IExamQuestionnaireUserRepository examQuestionnaireUserRepository,
            IExamQuestionnaireRepository examQuestionnaireRepository,
            IExamAssessmentUserRepository examAssessmentUserRepository,
            IExamAssessmentRepository examAssessmentRepository,
            IStudyPlanUserRepository studyPlanUserRepository,
            IStudyManager studyManager,
            IStudyPlanRepository studyPlanRepository)
        {
            _authManager = authManager;
            _examPaperUserRepository = examPaperUserRepository;
            _examPaperRepository = examPaperRepository;
            _examQuestionnaireUserRepository = examQuestionnaireUserRepository;
            _examQuestionnaireRepository = examQuestionnaireRepository;
            _examAssessmentUserRepository = examAssessmentUserRepository;
            _examAssessmentRepository = examAssessmentRepository;
            _studyPlanUserRepository = studyPlanUserRepository;
            _studyManager = studyManager;
            _studyPlanRepository = studyPlanRepository;
        }

        public class GetResult
        {
            public int Total { get; set; }
            public List<GetResultInfo> List { get; set; }
        }
        public class GetResultInfo
        {
            public string TaskName { get; set; }
            public int TaskTotal { get; set; }
            public List<DoTask> TaskList { get; set; }
        }
    }
}
