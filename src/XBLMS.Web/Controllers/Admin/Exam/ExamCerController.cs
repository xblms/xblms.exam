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
    public partial class ExamCerController : ControllerBase
    {
        private const string Route = "exam/examCer";
        private const string RouteDelete = Route + "/del";

        private readonly IAuthManager _authManager;
        private readonly IPathManager _pathManager;
        private readonly ISettingsManager _settingsManager;
        private readonly IExamCerRepository _examCerRepository;
        private readonly IExamCerUserRepository _examCerUserRepository;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IDatabaseManager _databaseManager;

        public ExamCerController(IDatabaseManager databaseManager, IAuthManager authManager, ISettingsManager settingsManager, IPathManager pathManager,
            IExamCerRepository examCerRepository, IExamCerUserRepository examCerUserRepository, IExamPaperRepository examPaperRepository)
        {
            _databaseManager = databaseManager;
            _authManager = authManager;
            _settingsManager = settingsManager;
            _pathManager = pathManager;
            _examCerRepository = examCerRepository;
            _examCerUserRepository = examCerUserRepository;
            _examPaperRepository = examPaperRepository;
        }
        public class GetRequest
        {
            public string Title { get; set; }
        }
        public class GetResult
        {
            public List<ExamCer> Items { get; set; }
        }
    }
}
