using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private readonly IExamPracticeAnswerRepository _examPracticeAnswerRepository;
        private readonly IExamPracticeCollectRepository _examPracticeCollectRepository;
        private readonly IExamPracticeWrongRepository _examPracticeWrongRepository;
        private readonly IExamPracticeRepository _examPracticeRepository;
        private readonly IExamTmAnalysisTmRepository _examTmAnalysisTmRepository;

        public ExamTmController(IAuthManager authManager, IPathManager pathManager, IDatabaseManager databaseManager, ICacheManager cacheManager,
            IConfigRepository configRepository, IExamManager examManager,
            IAdministratorRepository administratorRepository, IOrganManager organManager,
            IExamTxRepository examTxRepository, IExamTmTreeRepository examTmTreeRepository, IExamTmRepository examTmRepository, IStatRepository statRepository, IExamTmGroupRepository examTmGroupRepository,
            IExamPracticeRepository examPracticeRepository,
            IExamPracticeAnswerRepository examPracticeAnswerRepository,
            IExamPracticeCollectRepository examPracticeCollectRepository,
            IExamPracticeWrongRepository examPracticeWrongRepository,
            IExamTmAnalysisTmRepository examTmAnalysisTmRepository)
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
            _examPracticeRepository = examPracticeRepository;
            _examPracticeAnswerRepository = examPracticeAnswerRepository;
            _examPracticeCollectRepository = examPracticeCollectRepository;
            _examPracticeWrongRepository = examPracticeWrongRepository;
            _examTmAnalysisTmRepository = examTmAnalysisTmRepository;
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

        private async Task DeleteTm(List<int> tmIds)
        {
            var groupList = await _examTmGroupRepository.GetListAsync();
            foreach (var group in groupList)
            {
                if (group.GroupType == TmGroupType.Fixed && group.TmIds != null && group.TmIds.Count > 0)
                {
                    if (tmIds != null && tmIds.Count > 0)
                    {
                        var isUpdate = false;
                        foreach (var tmId in tmIds)
                        {
                            if (group.TmIds.Contains(tmId))
                            {
                                group.TmIds.Remove(tmId);
                                isUpdate = true;
                            }
                        }
                        if (isUpdate)
                        {
                            await _examTmGroupRepository.UpdateAsync(group);
                        }
                    }
                }
            }

            foreach (var tmId in tmIds)
            {
                await _examPracticeAnswerRepository.DeleteByTmIdAsync(tmId);
                await _examTmAnalysisTmRepository.DeleteByTmIdAsync(tmId);
            }

            var practiceList = await _examPracticeRepository.GetListAsync();
            foreach (var p in practiceList)
            {
                if (p.TmIds != null && tmIds.Count > 0)
                {
                    var isUpdate = false;
                    foreach (var tmId in tmIds)
                    {
                        if (p.TmIds.Contains(tmId))
                        {
                            p.TmIds.Remove(tmId);
                            isUpdate = true;
                        }
                    }
                    if (isUpdate)
                    {
                        await _examPracticeRepository.UpdateAsync(p);
                    }
                }
            }

            var wrongList = await _examPracticeWrongRepository.GetListAsync();
            foreach (var wrong in wrongList)
            {
                if (wrong.TmIds != null && wrong.TmIds.Count > 0)
                {
                    var isUpdate = false;
                    foreach (var tmId in tmIds)
                    {
                        if (wrong.TmIds.Contains(tmId))
                        {
                            wrong.TmIds.Remove(tmId);
                            isUpdate = true;
                        }
                    }
                    if (isUpdate)
                    {
                        await _examPracticeWrongRepository.UpdateAsync(wrong);
                    }
                }
            }

            var collection = await _examPracticeCollectRepository.GetListAsync();
            foreach (var collect in collection)
            {
                if (collect.TmIds != null && collect.TmIds.Count > 0)
                {
                    var isUpdate = false;
                    foreach (var tmId in tmIds)
                    {
                        if (collect.TmIds.Contains(tmId))
                        {
                            collect.TmIds.Remove(tmId);
                            isUpdate = true;
                        }
                    }
                    if (isUpdate)
                    {
                        await _examPracticeCollectRepository.UpdateAsync(collect);
                    }
                }
            }
        }
    }
}
