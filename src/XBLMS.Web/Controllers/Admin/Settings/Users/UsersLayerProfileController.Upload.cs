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
            var userName = string.Empty;
            if (userId > 0)
            {
                var user = await _userRepository.GetByUserIdAsync(userId);
                userName = user.UserName;
            }
            var (success, msg, url) = await _uploadManager.UploadAvatar(file, UploadManageType.UserAvatar, userName);
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
