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
    public partial class ExamCerUsersController : ControllerBase
    {
        private const string Route = "exam/examCerUsers";
        private const string RouteExport = Route + "/export";
        private const string RouteZip = Route + "/zip";

        private readonly IAuthManager _authManager;
        private readonly IPathManager _pathManager;
        private readonly IOrganManager _organManager;
        private readonly ISettingsManager _settingsManager;

        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamCerRepository _examCerRepository;
        private readonly IExamCerUserRepository _examCerUserRepository;
        private readonly IExamPaperStartRepository _examPaperStartRepository;


        public ExamCerUsersController(IAuthManager authManager,
            IPathManager pathManager,
            IOrganManager organManager,
            ISettingsManager settingsManager,
            IExamCerUserRepository examCerUserRepository,
            IExamCerRepository examCerRepository,
            IExamPaperRepository examPaperRepository,
            IExamQuestionnaireRepository examQuestionnaireRepository,
            IExamQuestionnaireUserRepository examQuestionnaireUserRepository,
            IExamPaperStartRepository examPaperStartRepository)
        {
            _authManager = authManager;
            _pathManager = pathManager;
            _organManager = organManager;
            _settingsManager = settingsManager;
            _examPaperRepository = examPaperRepository;
            _examCerRepository = examCerRepository;
            _examCerUserRepository = examCerUserRepository;
            _examPaperStartRepository = examPaperStartRepository;
        }


        public class GetUserRequest
        {
            public int Id { get; set; }
            public string DateFrom { get; set; }
            public string DateTo { get; set; }
            public string Keywords { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetUserResult
        {
            public int Total { get; set; }
            public List<ExamCerUser> List { get; set; }
        }
    }
}
