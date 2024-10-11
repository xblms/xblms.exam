using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Core.Extensions;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Home
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.User)]
    [Route(Constants.ApiHomePrefix)]
    public partial class DashboardController : ControllerBase
    {
        private const string Route = "dashboard";

        private readonly IConfigRepository _configRepository;
        private readonly IAuthManager _authManager;
        private readonly IOrganManager _organManager;
        private readonly IExamManager _examManager;
        private readonly IExamPaperUserRepository _examPaperUserRepository;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamPaperStartRepository _examPaperStartRepository;
        private readonly IExamQuestionnaireRepository _examQuestionnaireRepository;
        private readonly IExamQuestionnaireUserRepository _examQuestionnaireUserRepository;

        public DashboardController(IConfigRepository configRepository,
            IOrganManager organManager,
            IAuthManager authManager,
            IExamManager examManager,
            IExamPaperUserRepository examPaperUserRepository,
            IExamPaperRepository examPaperRepository,
            IExamPaperStartRepository examPaperStartRepository,
            IExamQuestionnaireRepository examQuestionnaireRepository,
            IExamQuestionnaireUserRepository examQuestionnaireUserRepository)
        {
            _configRepository = configRepository;
            _authManager = authManager;
            _organManager = organManager;
            _examManager = examManager;
            _examPaperUserRepository = examPaperUserRepository;
            _examPaperRepository = examPaperRepository;
            _examPaperStartRepository = examPaperStartRepository;
            _examQuestionnaireRepository = examQuestionnaireRepository;
            _examQuestionnaireUserRepository = examQuestionnaireUserRepository;
        }

        public class GetResult
        {
            public User User { get; set; }
            public double AllPercent { get; set; }
            public double ExamPercent { get; set; }
            public double ExamMoniPercent { get; set; }
            public int ExamTotal { get; set; }
            public int ExamMoniTotal { get; set; }

            public ExamPaper ExamPaper { get; set; }
            public ExamPaper ExamMoni { get; set; }

            public int PracticeAnswerTmTotal { get; set; }
            public double PracticeAnswerPercent { get; set; }
            public int PracticeAllTmTotal { get; set; }
            public double PracticeAllPercent { get; set; }
            public int PracticeCollectTmTotal { get; set; }
            public double PracticeCollectPercent { get; set; }
            public int PracticeWrongTmTotal { get; set; }
            public double PracticeWrongPercent { get; set; }

            public int TaskPaperTotal { get; set; }
            public int TaskQTotal { get; set; }

        }

    }
}
