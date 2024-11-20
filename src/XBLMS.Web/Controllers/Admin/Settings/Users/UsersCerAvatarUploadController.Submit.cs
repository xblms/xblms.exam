using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Dto;
using XBLMS.Core.Utils;
using XBLMS.Enums;
using XBLMS.Utils;
using Microsoft.AspNetCore.Http;
using XBLMS.Configuration;

namespace XBLMS.Web.Controllers.Admin.Settings.Users
{
    public partial class UsersCerAvatarUploadController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(Route)]
        public async Task<ActionResult<GetResult>> Submit([FromForm] IFormFile file)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }

            var fileName = PathUtils.GetFileName(file.FileName);
            var fileType = PathUtils.GetExtension(fileName);

            if (!FileUtils.IsImage(fileType))
            {
                return this.Error(Constants.ErrorUpload);
            }

            var realFileName = PathUtils.GetFileNameWithoutExtension(fileName);
            var user = await _userRepository.GetByUserNameAsync(realFileName);
            if (user != null)
            {
                var (success, msg, url) = await _uploadManager.UploadAvatar(file, UploadManageType.UserAvatarCer, user.UserName);
                if (!success)
                {
                    return this.Error(msg);
                }
                else
                {
                    user.AvatarCerUrl = url;
                    await _userRepository.UpdateAsync(user);
                    await _authManager.AddAdminLogAsync("上传证件照", user.DisplayName);
                }
            }
            else
            {
                return this.Error("找不到此用户");
            }
      

         

            return new GetResult
            {
                Success = true
            };
        }
    }
}
