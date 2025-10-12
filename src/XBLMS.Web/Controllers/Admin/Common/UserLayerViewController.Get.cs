using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class UserLayerViewController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] IdRequest request)
        {
            User user = null;
            if (request.Id > 0)
            {
                user = await _userRepository.GetByUserIdAsync(request.Id);
            }

            if (user == null) return this.Error(Constants.ErrorNotFound);

            user.Remove("confirmPassword");

            user = await _organManager.GetUser(user.Id);

            var config = await _configRepository.GetAsync();

            return new GetResult
            {
                User = user,
                SystemCode = config.SystemCode
            };
        }
    }
}
