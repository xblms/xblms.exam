using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;

namespace XBLMS.Web.Controllers.Admin.Settings.Block
{
    public partial class SettingsController
    {
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<BoolResult>> Delete([FromBody] DeleteRequest request)
        {
            var admin = await _authManager.GetAdminAsync();

            await _ruleRepository.DeleteAsync(request.RuleId);

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
