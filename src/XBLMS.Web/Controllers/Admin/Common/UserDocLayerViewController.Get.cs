using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class UserDocLayerViewController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var user = await _userRepository.GetByUserIdAsync(request.Id);
            var config = await _configRepository.GetAsync();

            return new GetResult
            {
                DisplayName = user.DisplayName,
                AvatarUrl = user.AvatarUrl,
                SystemCode = config.SystemCode
            };
        }
    }
}
