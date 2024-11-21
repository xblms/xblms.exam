using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Settings.Logs
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class LogsErrorController : ControllerBase
    {
        private const string Route = "settings/logsError";
        private const string RouteExport = "settings/logsError/actions/export";
        private const string RouteDelete = "settings/logsError/actions/delete";

        private readonly IAuthManager _authManager;
        private readonly IPathManager _pathManager;
        private readonly IErrorLogRepository _errorLogRepository;

        public LogsErrorController(IAuthManager authManager, IPathManager pathManager, IErrorLogRepository errorLogRepository)
        {
            _authManager = authManager;
            _pathManager = pathManager;
            _errorLogRepository = errorLogRepository;
        }

        public class SearchRequest : PageRequest
        {
            public string Category { get; set; }
            public string Keyword { get; set; }
            public string DateFrom { get; set; }
            public string DateTo { get; set; }
        }

        public class SearchResult : PageResult<ErrorLog>
        {
            public List<Select<string>> Categories { get; set; }
        }

        public async Task<SearchResult> GetResultsAsync(SearchRequest request)
        {
            var count = await _errorLogRepository.GetCountAsync(request.Category, request.Keyword, request.DateFrom, request.DateTo);
            var logs = await _errorLogRepository.GetAllAsync(request.Category, request.Keyword, request.DateFrom, request.DateTo, request.Offset, request.Limit);

            var categories = new List<Select<string>>();
            foreach (var category in LogUtils.AllCategoryList.Value)
            {
                categories.Add(new Select<string>(category.Key, category.Value));
            }

            return new SearchResult
            {
                Items = logs,
                Count = count,
                Categories = categories
            };
        }
    }
}
