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
    public partial class LogsUserController : ControllerBase
    {
        private const string Route = "settings/logsUser";
        private const string RouteExport = "settings/logsUser/actions/export";
        private const string RouteDelete = "settings/logsUser/actions/delete";

        private readonly IAuthManager _authManager;
        private readonly IPathManager _pathManager;
        private readonly IUserRepository _userRepository;
        private readonly ILogRepository _logRepository;

        public LogsUserController(IAuthManager authManager, IPathManager pathManager, IUserRepository userRepository, ILogRepository logRepository)
        {
            _authManager = authManager;
            _pathManager = pathManager;
            _userRepository = userRepository;
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
            var userId = 0;
            if (!string.IsNullOrEmpty(request.UserName))
            {
                var user = await _userRepository.GetByUserNameAsync(request.UserName);
                if (user == null)
                {
                    return new PageResult<Log>
                    {
                        Items = new List<Log>(),
                        Count = 0,
                    };
                }
                userId = user.Id;
            }

            var (count, userLogs) = await _logRepository.GetUserLogsAsync(adminAuth, userId, request.Keyword, request.DateFrom, request.DateTo, request.PageIndex, request.PageSize);
            var logs = new List<Log>();

            foreach (var log in userLogs)
            {
                var user = await _userRepository.GetByUserIdAsync(log.UserId);
                if (user == null)
                {
                    log.UserId = 0;
                    log.Set("userName", "已删除");
                }
                else
                {
                    var userName = _userRepository.GetDisplay(user);
                    log.Set("userName", userName);
                    log.Set("userGuid", user.Guid);
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
