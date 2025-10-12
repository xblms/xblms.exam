using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Home.Exam
{
    [OpenApiIgnore]
    [Route(Constants.ApiHomePrefix)]
    public partial class ExamQuestionnairingController : ControllerBase
    {
        private const string Route = "exam/examQuestionnairing";
        private const string RouteSubmitPaper = Route + "/submitPaper";

        private readonly IConfigRepository _configRepository;
        private readonly IAuthManager _authManager;
        private readonly IOrganManager _organManager;

        private readonly IExamQuestionnaireRepository _examQuestionnaireRepository;
        private readonly IExamQuestionnaireAnswerRepository _examQuestionnaireAnswerRepository;
        private readonly IExamQuestionnaireTmRepository _examQuestionnaireTmRepository;
        private readonly IExamQuestionnaireUserRepository _examQuestionnaireUserRepository;

        public ExamQuestionnairingController(IConfigRepository configRepository,
            IAuthManager authManager,
            IOrganManager organManager,
            IExamQuestionnaireRepository examQuestionnaireRepository,
            IExamQuestionnaireAnswerRepository examQuestionnaireAnswerRepository,
            IExamQuestionnaireTmRepository examQuestionnaireTmRepository,
            IExamQuestionnaireUserRepository examQuestionnaireUserRepository)
        {
            _configRepository = configRepository;
            _authManager = authManager;
            _organManager = organManager;
            _examQuestionnaireRepository = examQuestionnaireRepository;
            _examQuestionnaireAnswerRepository = examQuestionnaireAnswerRepository;
            _examQuestionnaireTmRepository = examQuestionnaireTmRepository;
            _examQuestionnaireUserRepository = examQuestionnaireUserRepository;
        }

        public class GetRequest
        {
            public int Id { get; set; }
            public string ps { get; set; }
        }

        public class GetResult
        {
            public string Watermark { get; set; }
            public ExamQuestionnaire Item { get; set; }
            public List<ExamQuestionnaireTm> TmList { get; set; }
        }

        public class GetSubmitRequest
        {
            public int PlanId { get; set; }
            public int CourseId { get; set; }
            public int Id { get; set; }
            public List<ExamQuestionnaireTm> TmList { get; set; }
        }
    }
}
