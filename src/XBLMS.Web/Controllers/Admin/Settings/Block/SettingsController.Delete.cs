using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace XBLMS.Web.Controllers.Admin.Settings.Block
{
    public partial class SettingsController
    {
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<GetResult>> Delete([FromBody] DeleteRequest request)
        {
            var admin = await _authManager.GetAdminAsync();


            await _ruleRepository.DeleteAsync(request.RuleId);

            var rules = await _ruleRepository.GetAllAsync();


            return new GetResult
            {
                Rules = rules
            };
        }
    }
}
