using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home
{
    public partial class ProfileController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(RouteUpload)]
        public async Task<ActionResult<StringResult>> Upload([FromForm] IFormFile file)
        {
            var (success, msg, url) = await _uploadManager.UploadAvatar(file, UploadManageType.UserAvatar, _authManager.UserName);
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
