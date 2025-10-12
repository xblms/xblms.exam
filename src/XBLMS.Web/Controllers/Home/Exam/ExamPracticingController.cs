using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Home.Exam
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.User)]
    [Route(Constants.ApiHomePrefix)]
    public partial class ExamPracticingController : ControllerBase
    {
        private const string Route = "exam/examPracticing";
        private const string RouteTm = Route + "/tm";
        private const string RouteAnswer = Route + "/answer";
        private const string RouteCollection = Route + "/collection";
        private const string RouteCollectionRemove = Route + "/collectionRemove";
        private const string RouteWrongRemove = Route + "/wrongRemove";

        private const string RoutePricticingTmIds = Route+ "/tmids";
        private const string RoutePricticingSubmit = Route+"/submit";
        private const string RouteCollectionSubmit = Route + "/collectionSubmit";
        private const string RouteError = Route + "home/exam/practice/error";
        private const string RouteErrorDel = Route + "home/exam/practice/error/del";

        private readonly IAuthManager _authManager;
        private readonly IConfigRepository _configRepository;
        private readonly IDatabaseManager _databaseManager;
        private readonly IAdministratorRepository _adminRepository;
        private readonly IExamTmRepository _examTmRepository;
        private readonly IExamPracticeRepository _examPracticeRepository;
        private readonly IExamTxRepository _examTxRepository;
        private readonly IExamManager _examManager;
        private readonly IExamPracticeAnswerRepository _examPracticeAnswerRepository;
        private readonly IExamPracticeCollectRepository _examPracticeCollectRepository;
        private readonly IExamPracticeWrongRepository _examPracticeWrongRepository;
        private readonly IExamPracticeAnswerSmallRepository _examPracticeAnswerSmallRepository;
        private readonly IExamTmSmallRepository _examTmSmallRepository;


        public ExamPracticingController(IAuthManager authManager,
            IConfigRepository configRepository,
            IDatabaseManager databaseManager,
            IExamManager examManager,
            IAdministratorRepository administratorRepository,
            IExamPracticeRepository examPracticeRepository,
            IExamTmRepository examTmRepository,
            IExamTxRepository examTxRepository,
            IExamPracticeAnswerRepository examPracticeAnswerRepository,
            IExamPracticeAnswerSmallRepository examPracticeAnswerSmallRepository,
            IExamPracticeCollectRepository examPracticeCollectRepository,
            IExamPracticeWrongRepository examPracticeWrongRepository,
            IExamTmSmallRepository examTmSmallRepository)
        {
            _authManager = authManager;
            _examManager = examManager;
            _configRepository = configRepository;
            _databaseManager = databaseManager;
            _adminRepository = administratorRepository;
            _examTmRepository = examTmRepository;
            _examPracticeRepository = examPracticeRepository;
            _examTxRepository = examTxRepository;
            _examPracticeWrongRepository = examPracticeWrongRepository;
            _examPracticeCollectRepository = examPracticeCollectRepository;
            _examPracticeAnswerRepository = examPracticeAnswerRepository;
            _examPracticeAnswerSmallRepository = examPracticeAnswerSmallRepository;
            _examTmSmallRepository = examTmSmallRepository;
        }
        public class GetTmResult
        {
            public string Tm { get; set; }
            public string Salt { get; set; }
        }
        public class GetSubmitAnswerRequest
        {
            public int PracticeId { get; set; }
            public int Id { get; set; }
            public string Answer { get; set; }
            public List<string> AnswerValues { get; set; }
            public List<GetSubmitAnswerSmallRequest> SmallList { get; set; }
        }
        public class GetSubmitAnswerSmallRequest
        {
            public int Id { get; set; }
            public string Answer { get; set; }
            public List<string> AnswerValues { get; set; }
        }
        public class GetSubmitAnswerResult
        {
            public PointNotice PointNotice { get; set; }
            public bool IsRight { get; set; }
            public string Answer { get; set; }
            public string Jiexi { get; set; }
        }
        public class GetPracticingRequest
        {
            public int Id { get; set; }
            public string Zsd { get; set; }
        }
        public class GetRequest
        {
            public string keyWord { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetHistoryRequest
        {
            public string Order { get; set; }
            public string Type { get; set; }
            public string Title { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetResult
        {
            public PointNotice PointNotice { get; set; }
            public string Watermark { get; set; }
            public int Total { get; set; }
            public List<int> TmIds { get; set; }
            public string Title { get; set; }
        }
        public class GetCollectionResult
        {
            public int Total { get; set; }
            public List<GetResultZsdForTmCount> List { get; set; }
        }
        public class GetHistoryResult
        {
            public int Total { get; set; }
            public List<GetHistoryResultInfo> List { get; set; }
        }
        public class GetHistoryResultInfo
        {
            public int Id { get; set; }
            public List<string> Zsds { get; set; }
            public string DateTime { get; set; }
            public int TmCount { get; set; }
            public int AnswerCount { get; set; }
            public int RightCount { get; set; }
            public string Source { get; set; }
        }
        public class GetResultInfo
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public List<string> Zsds { get; set; }
            public string DateTime { get; set; }

        }
        public class GetResultZsdForTmCount
        {
            public string Zsd { get; set; }
            public int Count { get; set; }
        }
        public class GetSubmitPracticingRequest
        {
            public int PracticeId { get; set; }
            public int PracticeUserId { get; set; }
            public GetPracticingResultTmInfo Tm { get; set; }
        }
        public class GetPracticingIdsResult
        {
            public List<int> TmIdList { get; set; }
            public int Total { get; set; }
            public int PracticeUserId { get; set; }
            public string Title { get; set; }
        }
        public class GetPracticingResult
        {
            public List<GetPracticingResultTmInfo> List { get; set; }
            public int Total { get; set; }
            public int PracticeUserId { get; set; }
            public string Title { get; set; }
        }
        public class GetPracticingResultTmInfo
        {
            public int Id { get; set; }
            public string Tm { get; set; }
            public List<KeyValuePair<int, string>> TmTitle { get; set; }
            public string Tx { get; set; }
            public string TxType { get; set; }
            public List<string> Options { get; set; }
            public List<string> OptionsValue { get; set; }
            public int ParentId { get; set; }
            public List<GetPracticingResultTmInfo> SmallList { get; set; }
            public string Answer { get; set; }
            public string Zsd { get; set; }
            public string Analysis { get; set; }
            public bool IsRight { get; set; }
            public string RightAnswer { get; set; }
            public bool IsSubmit { get; set; }
            public bool IsCollection { get; set; }
            public bool IsError { get; set; }
            public int ErrorTotal { get; set; }
        }

        public class GetCollectionRequest
        {
            public bool IsBig { get; set; }
            public int TmId { get; set; }
            public bool Collection { get; set; }
        }
    }
}
