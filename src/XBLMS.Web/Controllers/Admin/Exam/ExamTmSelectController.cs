using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Core.Services;
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
    public partial class ExamTmSelectController : ControllerBase
    {
        private const string Route = "exam/examTmSelect";
        private const string RouteGetIn = Route + "/getIn";
        private const string RouteSelect = Route + "/setGroupTm";
        private const string RouteRemove = Route + "/delGroupTm";


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

        public ExamTmSelectController(IAuthManager authManager, IPathManager pathManager, IDatabaseManager databaseManager, ICacheManager cacheManager,
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
        public class GetSeletRemoveRequest
        {
            public List<int> Ids { get; set; }
            public int Id { get; set; }
        }
        public class GetSelectResult
        {
            public List<ExamTm> Items { get; set; }
        }
        public class GetSearchResults
        {
            public List<ExamTm> Items { get; set; }
            public int Total { get; set; }

        }
        public class GetSearchRequest
        {
            public int Id { get; set; }
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
    }
}
