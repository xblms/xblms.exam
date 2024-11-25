using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home
{
    public partial class ProfileController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<BoolResult>> Submit([FromBody] User request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            if (request.Id != user.Id) return Unauthorized();

            if (!PageUtils.IsProtocolUrl(request.AvatarUrl))
            {
                user.AvatarUrl = request.AvatarUrl;
            }
            if (!PageUtils.IsProtocolUrl(request.AvatarbgUrl))
            {
                user.AvatarbgUrl = request.AvatarbgUrl;
            }
            user.Mobile = request.Mobile;
            user.Email = request.Email;
            user.DisplayName = request.DisplayName;


            var (success, errorMessage) = await _userRepository.UpdateAsync(user);
            if (!success)
            {
                return this.Error($"修改资料失败：{errorMessage}");
            }

            await _authManager.AddUserLogAsync("修改资料");

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
