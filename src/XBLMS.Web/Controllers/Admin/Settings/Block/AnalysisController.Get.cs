using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Block
{
    public partial class AnalysisController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get()
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }
            var admin = await _authManager.GetAdminAsync();

            var blockedList = await _analysisRepository.GetMonthlyBlockedListAsync();
            var labels = blockedList.Select(x => x.Key).ToList();
            var data = blockedList.Select(x => x.Value).ToList();

            var blockedListadmin = await _analysisRepository.GetMonthlyBlockedListAsync(2);
            var labelsadmin = blockedListadmin.Select(x => x.Key).ToList();
            var dataadmin = blockedListadmin.Select(x => x.Value).ToList();

            return new GetResult
            {
                Days = labels,
                UserCount = data,
                AdminCount = dataadmin
            };
        }
    }
}
