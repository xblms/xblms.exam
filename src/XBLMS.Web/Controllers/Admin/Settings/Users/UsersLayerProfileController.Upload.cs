using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Users
{
    public partial class UsersLayerProfileController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(RouteUpload)]
        public async Task<ActionResult<StringResult>> Upload([FromQuery] int userId, [FromForm] IFormFile file)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }
            var user = await _userRepository.GetByUserIdAsync(userId);
            if (user == null) return NotFound();
            var (success, msg, url) = await _uploadManager.UploadAvatar(file, UploadManageType.AdminAvatar, user.UserName);
            if (success)
            {
                return new StringResult
                {
                    Value = url
                };
            }
            else
            {
                return this.Error(msg);
            }
        }
    }
}
