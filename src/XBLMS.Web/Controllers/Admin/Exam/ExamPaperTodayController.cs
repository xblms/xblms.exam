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
    public partial class ExamPaperTodayController : ControllerBase
    {
        private const string Route = "exam/examPaperToday";

        private readonly IAuthManager _authManager;
        private readonly IExamManager _examManager;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamPaperTreeRepository _examPaperTreeRepository;
        private readonly IExamPaperUserRepository _examPaperUserRepository;
        private readonly IExamPaperStartRepository _examPaperStartRepository;
        private readonly IExamTmAnalysisRepository _examTmAnalysisRepository;
        private readonly IExamTmAnalysisTmRepository _examTmAnalysisTmRepository;

        public ExamPaperTodayController(IAuthManager authManager,
            IExamManager examManager,
            IExamPaperRepository examPaperRepository,
            IExamPaperTreeRepository examPaperTreeRepository,
            IExamPaperUserRepository examPaperUserRepository,
            IExamPaperStartRepository examPaperStartRepository,
            IExamTmAnalysisRepository examTmAnalysisRepository,
            IExamTmAnalysisTmRepository examTmAnalysisTmRepository)
        {
            _authManager = authManager;
            _examManager = examManager;
            _examPaperRepository = examPaperRepository;
            _examPaperTreeRepository = examPaperTreeRepository;
            _examPaperUserRepository = examPaperUserRepository;
            _examPaperStartRepository = examPaperStartRepository;
            _examTmAnalysisRepository = examTmAnalysisRepository;
            _examTmAnalysisTmRepository = examTmAnalysisTmRepository;
        }
        public class GetRequest
        {
            public string DateType { get; set; }
        }
        public class GetResult
        {
            public List<ExamPaper> Items { get; set; }

        }
    }
}
