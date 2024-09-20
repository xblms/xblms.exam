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
    public partial class ExamQuestionnaireController : ControllerBase
    {
        private const string Route = "exam/examQuestionnaire";

        private readonly IConfigRepository _configRepository;
        private readonly IAuthManager _authManager;
        private readonly IExamQuestionnaireUserRepository _examQuestionnaireUserRepository;
        private readonly IExamQuestionnaireRepository _examQuestionnaireRepository;
        private readonly IExamManager _examManager;

        public ExamQuestionnaireController(IConfigRepository configRepository,
            IAuthManager authManager,
            IExamManager examManager,
            IExamQuestionnaireUserRepository examQuestionnaireUserRepository,
            IExamQuestionnaireRepository examQuestionnaireRepository)
        {
            _configRepository = configRepository;
            _authManager = authManager;
            _examManager = examManager;
            _examQuestionnaireUserRepository = examQuestionnaireUserRepository;
            _examQuestionnaireRepository = examQuestionnaireRepository;
        }
        public class GetRequest
        {
            public string KeyWords { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetResult
        {
            public List<ExamQuestionnaire> List { get; set; }
            public int Total { get; set; }
        }
    }
}
