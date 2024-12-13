using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class ExamTmAnalysisController : ControllerBase
    {
        private const string Route = "exam/examTmAnalysis";
        private const string RouteGetData = Route + "/data";
        private const string RouteGetPaper = Route + "/paper";

        private readonly IAuthManager _authManager;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamTxRepository _examTxRepository;
        private readonly IExamTmRepository _examTmRepository;
        private readonly IExamTmAnalysisRepository _examTmAnalysisRepository;
        private readonly IExamTmAnalysisTmRepository _examTmAnalysisTmRepository;
        private readonly IExamPaperRandomTmRepository _examPaperRandomTmRepository;
        private readonly IExamPaperAnswerRepository _examPaperAnswerRepository;
        private readonly IExamPracticeAnswerRepository _examPracticeAnswerRepository;

        public ExamTmAnalysisController(IAuthManager authManager, IExamPaperRepository examPaperRepository, IExamTxRepository examTxRepository, IExamTmRepository examTmRepository,
            IExamTmAnalysisRepository examTmAnalysisRepository, IExamTmAnalysisTmRepository examTmAnalysisTmRepository,
            IExamPaperRandomTmRepository examPaperRandomTmRepository, IExamPaperAnswerRepository examPaperAnswerRepository, IExamPracticeAnswerRepository examPracticeAnswerRepository)
        {
            _authManager = authManager;
            _examPaperRepository = examPaperRepository;
            _examTxRepository = examTxRepository;
            _examTmRepository = examTmRepository;
            _examTmAnalysisRepository = examTmAnalysisRepository;
            _examTmAnalysisTmRepository = examTmAnalysisTmRepository;
            _examPaperRandomTmRepository = examPaperRandomTmRepository;
            _examPaperAnswerRepository = examPaperAnswerRepository;
            _examPracticeAnswerRepository = examPracticeAnswerRepository;
        }
        public class GetRequest
        {
            public bool ReAnalysis { get; set; }
            public TmAnalysisType Type { get; set; }
            public string OrderType { get; set; }
            public int PaperId { get; set; }
            public string KeyWords { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetResult
        {
            public int Total { get; set; }
            public List<ExamTmAnalysisTm> List { get; set; }
            public int PId { get; set; }
            public string PDate { get; set; }
        }

        public class GetData
        {
            public List<Select<string>> TypeList { get; set; }
        }
        public class GetPaperRequest
        {
            public string Title { get; set; }
        }
        public class GetPaperResult
        {
            public List<KeyValuePair<int, string>> List { get; set; }
        }
    }
}
