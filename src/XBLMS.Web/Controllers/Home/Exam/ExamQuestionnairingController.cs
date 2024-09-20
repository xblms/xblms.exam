using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = Types.Roles.User)]
    [Route(Constants.ApiHomePrefix)]
    public partial class ExamQuestionnairingController : ControllerBase
    {
        private const string Route = "exam/examQuestionnairing";
        private const string RouteSubmitPaper = Route + "/submitPaper";

        private readonly IConfigRepository _configRepository;
        private readonly IAuthManager _authManager;

        private readonly IExamQuestionnaireRepository _examQuestionnaireRepository;
        private readonly IExamQuestionnaireAnswerRepository _examQuestionnaireAnswerRepository;
        private readonly IExamQuestionnaireTmRepository _examQuestionnaireTmRepository;
        private readonly IExamQuestionnaireUserRepository _examQuestionnaireUserRepository;

        public ExamQuestionnairingController(IConfigRepository configRepository,
            IAuthManager authManager,
            IExamQuestionnaireRepository examQuestionnaireRepository,
            IExamQuestionnaireAnswerRepository examQuestionnaireAnswerRepository,
            IExamQuestionnaireTmRepository examQuestionnaireTmRepository,
            IExamQuestionnaireUserRepository examQuestionnaireUserRepository)
        {
            _configRepository = configRepository;
            _authManager = authManager;
            _examQuestionnaireRepository = examQuestionnaireRepository;
            _examQuestionnaireAnswerRepository = examQuestionnaireAnswerRepository;
            _examQuestionnaireTmRepository = examQuestionnaireTmRepository;
            _examQuestionnaireUserRepository = examQuestionnaireUserRepository;
        }
        public class GetResult
        {
            public string Watermark { get; set; }
            public ExamQuestionnaire Item { get; set; }
            public List<ExamQuestionnaireTm> TmList { get; set; }
        }

        public class GetSubmitRequest
        {
            public int PapaerId { get; set; }
            public List<ExamQuestionnaireTm> TmList { get; set; }
        }
    }
}
