using System.Linq;
using System.Threading.Tasks;
using Datory;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Configuration;
using XBLMS.Core.Utils;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home
{
    public partial class ProfileController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get()
        {
            var config = await _configRepository.GetAsync();
            if (config.IsHomeClosed) return this.Error("对不起，用户中心已被禁用！");

            var user = await _authManager.GetUserAsync();
            var entity = new Entity();


            entity.Set(nameof(Models.User.Id), user.Id);
            entity.Set(nameof(Models.User.UserName), user.UserName);
            entity.Set(nameof(Models.User.DisplayName), user.DisplayName);
            entity.Set(nameof(Models.User.AvatarUrl), user.AvatarUrl);
            entity.Set(nameof(Models.User.Mobile), user.Mobile);
            entity.Set(nameof(Models.User.Email), user.Email);

            return new GetResult
            {
                Entity = entity
            };
        }
    }
}
