using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class ExamQuestionnaireAnalysisController : ControllerBase
    {
        private const string Route = "exam/examQuestionnaireAnalysis";
        private const string RouteExportWord = Route + "/exportWord";

        private readonly IAuthManager _authManager;
        private readonly IPathManager _pathManager;
        private readonly IExamQuestionnaireRepository _questionnaireRepository;
        private readonly IExamQuestionnaireTmRepository _questionnaireTmRepository;
        private readonly IExamQuestionnaireAnswerRepository _questionnaireAnswerRepository;


        public ExamQuestionnaireAnalysisController(IPathManager pathManager,IAuthManager authManager,
            IExamQuestionnaireRepository examQuestionnaireRepository,
            IExamQuestionnaireTmRepository examQuestionnaireTmRepository,
            IExamQuestionnaireAnswerRepository questionnaireAnswerRepository)
        {
            _pathManager = pathManager;
            _authManager = authManager;
            _questionnaireRepository = examQuestionnaireRepository;
            _questionnaireTmRepository = examQuestionnaireTmRepository;
            _questionnaireAnswerRepository = questionnaireAnswerRepository;
        }
        public class GetRequest
        {
            public int Id { get; set; }
            public int PlanId { get; set; }
            public int CourseId { get; set; }
        }
        public class GetResult
        {
            public ExamQuestionnaire Item { get; set; }
            public List<ExamQuestionnaireTm> TmList { get; set; }
        }

    }
}

