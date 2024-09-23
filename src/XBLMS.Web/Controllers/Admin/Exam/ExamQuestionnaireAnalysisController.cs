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

        private readonly IAuthManager _authManager;
        private readonly IExamManager _examManager;
        private readonly IPathManager _pathManager;
        private readonly IUserGroupRepository _userGroupRepository;

        private readonly IExamQuestionnaireRepository _questionnaireRepository;
        private readonly IExamQuestionnaireTmRepository _questionnaireTmRepository;
        private readonly IExamQuestionnaireUserRepository _questionnaireUserRepository;
        private readonly IExamQuestionnaireAnswerRepository _questionnaireAnswerRepository;


        public ExamQuestionnaireAnalysisController(IAuthManager authManager,
            IPathManager pathManager,
            IExamManager examManager,
            IUserGroupRepository userGroupRepository,
            IExamQuestionnaireRepository examQuestionnaireRepository,
            IExamQuestionnaireTmRepository examQuestionnaireTmRepository,
            IExamQuestionnaireUserRepository examQuestionnaireUserRepository,
            IExamQuestionnaireAnswerRepository questionnaireAnswerRepository)
        {
            _authManager = authManager;
            _examManager = examManager;
            _pathManager = pathManager;
            _userGroupRepository = userGroupRepository;
            _questionnaireRepository = examQuestionnaireRepository;
            _questionnaireTmRepository = examQuestionnaireTmRepository;
            _questionnaireUserRepository = examQuestionnaireUserRepository;
            _questionnaireAnswerRepository = questionnaireAnswerRepository;
        }
        public class GetResult
        {
            public ExamQuestionnaire Item { get; set; }
            public List<ExamQuestionnaireTm> TmList { get; set; }
        }

    }
}

