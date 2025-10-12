using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace XBLMS.Web.Controllers.Home
{
    public partial class MineController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get()
        {
            var user = await _authManager.GetUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }
            return new GetResult
            {
                User = user,
                Version = _settingsManager.Version,
            };
        }
    }
}
