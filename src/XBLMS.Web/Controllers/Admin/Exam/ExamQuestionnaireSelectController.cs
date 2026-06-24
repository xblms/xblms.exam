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
    public partial class ExamQuestionnaireSelectController : ControllerBase
    {
        private const string Route = "exam/examQuestionnaireSelect";

        private readonly IAuthManager _authManager;
        private readonly IExamQuestionnaireRepository _examQuestionnaireRepository;
        public ExamQuestionnaireSelectController(IAuthManager authManager,IExamQuestionnaireRepository examQuestionnaireRepository)
        {
            _authManager = authManager;
            _examQuestionnaireRepository = examQuestionnaireRepository;
        }
        public class GetRequest
        {
            public string Keyword { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetResult
        {
            public List<ExamQuestionnaire> Items { get; set; }
            public int Total { get; set; }

        }
    }
}
