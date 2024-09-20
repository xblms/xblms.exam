using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Core.Utils;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Logs
{
    public partial class LogsErrorController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<SearchResult>> Get([FromBody] SearchRequest request)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }
            return await GetResultsAsync(request);
        }
    }
}
