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
    public partial class ExamQuestionnaireController : ControllerBase
    {
        private const string Route = "exam/examQuestionnaire";
        private const string RouteDelete = Route + "/del";
        private const string RouteLock = Route + "/lock";
        private const string RouteUnLock = Route + "/unLock";

        private readonly IAuthManager _authManager;
        private readonly IExamManager _examManager;
        private readonly IExamQuestionnaireRepository _examQuestionnaireRepository;
        private readonly IExamQuestionnaireUserRepository _examQuestionnaireUserRepository;
        public ExamQuestionnaireController(IAuthManager authManager,
            IExamManager examManager,
            IExamQuestionnaireRepository examQuestionnaireRepository,
            IExamQuestionnaireUserRepository examQuestionnaireUserRepository)
        {
            _authManager = authManager;
            _examManager = examManager;
            _examQuestionnaireRepository = examQuestionnaireRepository;
            _examQuestionnaireUserRepository = examQuestionnaireUserRepository;
        }
        public class GetLockedRequest
        {
            public int Id { get; set; }
            public bool Locked { get; set; }
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
