using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Home.Exam
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.User)]
    [Route(Constants.ApiHomePrefix)]
    public partial class ExamPracticeController : ControllerBase
    {
        private const string Route = "exam/examPractice";
        private const string RouteTotal = Route + "/total";
        private const string RouteSubmit = Route + "/submit";
        private const string RouteDelete = Route + "/del";

        private const string RouteReady = Route + "/ready";
        private const string RouteReadySubmit = RouteReady + "/submit";
        private const string RouteReadySearch = RouteReady + "/search";

        private readonly IConfigRepository _configRepository;
        private readonly IAuthManager _authManager;
        private readonly IExamPaperUserRepository _examPaperUserRepository;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamManager _examManager;
        private readonly IExamTmGroupRepository _examTmGroupRepository;
        private readonly IExamTmRepository _examTmRepository;
        private readonly IExamPracticeRepository _examPracticeRepository;
        private readonly IExamPracticeCollectRepository _examPracticeCollectRepository;
        private readonly IExamPracticeWrongRepository _examPracticeWrongRepository;
        private readonly IOrganManager _organManager;
        private readonly IExamTxRepository _examTxRepository;
        private readonly IExamPracticeAnswerRepository _examPracticeAnswerRepository;

        public ExamPracticeController(IConfigRepository configRepository,
            IAuthManager authManager,
            IExamPaperRepository examPaperRepository,
            IExamPaperUserRepository examPaperUserRepository,
            IExamManager examManager,
            IExamTmGroupRepository examTmGroupRepository,
            IExamTmRepository examTmRepository,
            IExamPracticeRepository examPracticeRepository,
            IExamPracticeCollectRepository examPracticeCollectRepository,
            IExamPracticeWrongRepository examPracticeWrongRepository,
            IOrganManager organManager,
            IExamTxRepository examTxRepository,
            IExamPracticeAnswerRepository examPracticeAnswerRepository)
        {
            _configRepository = configRepository;
            _authManager = authManager;
            _examPaperRepository = examPaperRepository;
            _examPaperUserRepository = examPaperUserRepository;
            _examManager = examManager;
            _examTmGroupRepository = examTmGroupRepository;
            _examTmRepository = examTmRepository;
            _examPracticeRepository = examPracticeRepository;
            _examPracticeCollectRepository = examPracticeCollectRepository;
            _examPracticeWrongRepository = examPracticeWrongRepository;
            _organManager = organManager;
            _examTxRepository = examTxRepository;
            _examPracticeAnswerRepository = examPracticeAnswerRepository;
        }
        public class GetSubmitRequest
        {
            public int GroupId { get; set; }
            public PracticeType PracticeType { get; set; }
        }
        public class GetSubmitResult
        {
            public int Id { get; set; }
            public bool Success { get; set; }
            public string Error { get; set; }
        }

        public class GetRequest
        {
            public string KeyWords { get; set; }
            public string Date { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetCreateResult
        {
            public int Total { get; set; }
            public List<ExamPractice> List { get; set; }
        } 
        public class GetTotalResult
        {
            public int CollectTotal { get; set; }
            public int WrongTotal { get; set; }
        }
        public class GetResultItem
        {
            public int Id { get; set; }
            public int TmTotal { get; set; }
            public List<string> Zsds { get; set; }
        }

        public class GetSubmitReadyRequest
        {
            public ExamPractice Item { get; set; }
        }
        public class GetReadyRequest
        {
            public GetReadySearchResult Search { get; set; }
            public List<ExamTx> TxList { get; set; }
            public ExamPractice Item { get; set; }
        }
        public class GetReadySearchRequest
        {
            public List<int> TxIds { get; set; }
            public List<int> Nds { get; set; }
            public List<string> Zsds { get; set; }
        }
        public class GetReadySearchResult
        {
            public int TmCount { get; set; }
            public List<int> TmIds { get; set; }
        }
    }
}
