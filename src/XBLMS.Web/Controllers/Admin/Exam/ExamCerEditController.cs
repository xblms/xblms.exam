using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class ExamCerEditController : ControllerBase
    {
        private const string Route = "exam/examCerEdit";
        private const string RouteUpload = Route + "/upload";
        private const string RouteSubmitPosition = Route + "/positionsSubmit";

        private readonly IDatabaseManager _databaseManager;
        private readonly IAuthManager _authManager;
        private readonly IPathManager _pathManager;
        private readonly ISettingsManager _settingsManager;
        private readonly IExamCerRepository _examCerRepository;
        private readonly IStatRepository _statRepository;

        public ExamCerEditController(IDatabaseManager databaseManager, IAuthManager authManager, IPathManager pathManager, ISettingsManager settingsManager,
            IExamCerRepository examCerRepository, IStatRepository statRepository)
        {
            _databaseManager = databaseManager;
            _authManager = authManager;
            _pathManager = pathManager;
            _settingsManager = settingsManager;
            _examCerRepository = examCerRepository;
            _statRepository = statRepository;
        }

        public class GetResult
        {
            public ExamCer Item { get; set; }
            public List<string> Fonts { get; set; }
        }
        public class SubmitWartmarkPositionData
        {
            public int Id { get; set; }
            public List<ExamCertPosition> Position { get; set; }
        }
    }
}
