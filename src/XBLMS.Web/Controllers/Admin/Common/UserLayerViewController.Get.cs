using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Models;
using XBLMS.Configuration;
using XBLMS.Utils;
using XBLMS.Core.Utils;
using XBLMS.Dto;

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

            return new GetResult
            {
                User = user
            };
        }
    }
}
