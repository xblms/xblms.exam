using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Settings.Logs
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class LogsAdminController : ControllerBase
    {
        private const string Route = "settings/logsAdmin";
        private const string RouteExport = "settings/logsAdmin/actions/export";
        private const string RouteDelete = "settings/logsAdmin/actions/delete";

        private readonly IAuthManager _authManager;
        private readonly IPathManager _pathManager;
        private readonly IAdministratorRepository _administratorRepository;
        private readonly ILogRepository _logRepository;

        public LogsAdminController(IAuthManager authManager, IPathManager pathManager, IAdministratorRepository administratorRepository, ILogRepository logRepository)
        {
            _authManager = authManager;
            _pathManager = pathManager;
            _administratorRepository = administratorRepository;
            _logRepository = logRepository;
        }

        public class SearchRequest
        {
            public string UserName { get; set; }
            public string Keyword { get; set; }
            public string DateFrom { get; set; }
            public string DateTo { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }

        public async Task<PageResult<Log>> GetResultsAsync(SearchRequest request)
        {
            var adminAuth = await _authManager.GetAdminAuth();
            var adminIds = new List<int>();
            if (!string.IsNullOrEmpty(request.UserName))
            {
                adminIds = await _administratorRepository.GetAdministratorIdsAsync(request.UserName);
                if (adminIds == null || adminIds.Count == 0)
                {
                    return new PageResult<Log>
                    {
                        Items = new List<Log>(),
                        Count = 0,
                    };
                }
            }

            var (count,allLogs) = await _logRepository.GetAdminLogsAsync(adminAuth, adminIds, request.Keyword, request.DateFrom, request.DateTo, request.PageIndex, request.PageSize);
            var logs = new List<Log>();

            foreach (var log in allLogs)
            {
                var admin = await _administratorRepository.GetByUserIdAsync(log.AdminId);
                if (admin != null)
                {
                    log.Set("displanyName", admin.DisplayName);
                }
                else
                {
                    log.Set("displanyName", "已删除");
                    log.AdminId = 0;
                }
                logs.Add(log);
            }

            return new PageResult<Log>
            {
                Items = logs,
                Count = count
            };
        }
    }
}
