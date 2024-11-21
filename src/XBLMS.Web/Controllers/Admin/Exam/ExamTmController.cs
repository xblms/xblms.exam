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
    public partial class ExamTmController : ControllerBase
    {
        private const string Route = "exam/examTm";
        private const string RouteDelete = Route + "/del";
        private const string RouteSearch = Route + "/search";
        private const string RouteImportExcel = Route + "/importExcel";
        private const string RouteImportGetCache = RouteImportExcel + "/getCache";
        private const string RouteExportExcel = Route + "/export";
        private const string RouteDeleteSearch = Route + "/delSearch";
        private const string RouteEdit = Route + "/tmEdit/get";
        private const string RouteEditSubmit = Route + "/tmEdit/submit";
        private const string RouteImportWord = Route + "/importWord";
        private const string RouteExportWord = Route + "/exportWord";


        private readonly IAuthManager _authManager;
        private readonly IPathManager _pathManager;
        private readonly ICacheManager _cacheManager;
        private readonly IDatabaseManager _databaseManager;
        private readonly IConfigRepository _configRepository;
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IExamTxRepository _examTxRepository;
        private readonly IExamTmTreeRepository _examTmTreeRepository;
        private readonly IExamTmRepository _examTmRepository;
        private readonly IStatRepository _statRepository;
        private readonly IExamManager _examManager;
        private readonly IOrganManager _organManager;
        private readonly IExamTmGroupRepository _examTmGroupRepository;

        public ExamTmController(IAuthManager authManager, IPathManager pathManager, IDatabaseManager databaseManager, ICacheManager cacheManager,
            IConfigRepository configRepository, IExamManager examManager,
            IAdministratorRepository administratorRepository, IOrganManager organManager,
            IExamTxRepository examTxRepository, IExamTmTreeRepository examTmTreeRepository, IExamTmRepository examTmRepository, IStatRepository statRepository, IExamTmGroupRepository examTmGroupRepository)
        {
            _organManager = organManager;
            _examManager = examManager;
            _authManager = authManager;
            _pathManager = pathManager;
            _cacheManager = cacheManager;
            _databaseManager = databaseManager;
            _configRepository = configRepository;
            _administratorRepository = administratorRepository;
            _examTxRepository = examTxRepository;
            _examTmTreeRepository = examTmTreeRepository;
            _examTmRepository = examTmRepository;
            _statRepository = statRepository;
            _examTmGroupRepository = examTmGroupRepository;
        }
        public class GetEditResult
        {
            public ExamTm Item { get; set; }
            public List<Cascade<int>> TmTree { get; set; }
            public List<ExamTx> TxList { get; set; }
        }

        public class GetSearchResults
        {
            public List<ExamTm> Items { get; set; }
            public int Total { get; set; }

        }
        public class GetSearchRequest
        {
            public int TmGroupId { get; set; }
            public bool TreeIsChildren { get; set; }
            public int TreeId { get; set; }
            public int TxId { get; set; }
            public int Nandu { get; set; }
            public string Keyword { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
            public string Order { get; set; }
            public string OrderType { get; set; }
            public bool? IsStop { get; set; }
        }
        public class GetDeletesRequest
        {
            public List<int> Ids { get; set; }
        }

        public class GetImportResult
        {
            public bool Value { set; get; }
            public int Success { set; get; }
            public int Failure { set; get; }
            public string ErrorMessage { set; get; }
            public List<string> ErrorMessageList { get; set; }
        }

        public class ImportMessage
        {
            public int RowIndexName { get; set; }
            public string ErrorMessage { get; set; }
        }

        public class GetImportWordResult
        {
            public int OkCount { set; get; }
            public int NoCount { set; get; }
            public List<string> ErrorList { get; set; }
        }
        public class CacheResultImportTm
        {
            public bool IsError { get; set; } = false;
            public bool IsStop { get; set; } = false;
            public bool IsOver { get; set; } = false;
            public int TmTotal { get; set; } = 0;
            public int TmCurrent { get; set; } = 0;
        }
    }
}
