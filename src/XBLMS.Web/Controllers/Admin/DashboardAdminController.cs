using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class DashboardAdminController : ControllerBase
    {
        private const string Route = "dashboardAdmin";
        private const string RouteGetData = Route + "/data";

        private readonly IHttpContextAccessor _context;
        private readonly IAntiforgery _antiforgery;
        private readonly ICacheManager _cacheManager;
        private readonly ISettingsManager _settingsManager;
        private readonly IDatabaseManager _databaseManager;
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IAuthManager _authManager;
        private readonly IOrganManager _organManager;
        private readonly IStatLogRepository _statLogRepository;
        private readonly IStatRepository _statRepository;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IConfigRepository _configRepository;

        public DashboardAdminController(IHttpContextAccessor context, IAuthManager authManager, IAntiforgery antiforgery, ICacheManager cacheManager, ISettingsManager settingsManager, IDatabaseManager databaseManager, IAdministratorRepository administratorRepository, IOrganManager organManager, IStatLogRepository statLogRepository, IStatRepository statRepository, IExamPaperRepository examPaperRepository, IConfigRepository configRepository)
        {
            _context = context;
            _authManager = authManager;
            _antiforgery = antiforgery;
            _cacheManager = cacheManager;
            _settingsManager = settingsManager;
            _databaseManager = databaseManager;
            _administratorRepository = administratorRepository;
            _organManager = organManager;
            _statLogRepository = statLogRepository;
            _statRepository = statRepository;
            _examPaperRepository = examPaperRepository;
            _configRepository = configRepository;
        }
        public class GetLogRequest
        {
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetLogResult
        {
            public int Total { get; set; }
            public List<StatLog> List { get; set; }
        }

        public class GetResult
        {
            public Administrator Administrator { get; set; }
        }
        public class GetDataResult
        {
            public List<string> DataTitleList { get; set; }
            public List<GetDataInfo> DataList { get; set; }
            public int ExamTotalToday { get; set; }
            public int ExamTotalWeek { get; set; }
            public int PlanOverTotal { get; set; }
            public int PlanCreateTotal { get; set; }
            public int OffTrainTotal { get; set; }
            public int TotalCompany { get; set; }
            public int TotalAdmin { get; set; }
            public int TotalUser { get; set; }
            public int TotalTm { get; set; }
            public int TotalFile { get; set; }
            public SystemCode SystemCode { get; set; }
        }

        public class GetDataInfo
        {
            public string Name { get; set; }
            public List<int> Data { get; set; }
        }
    }
}
