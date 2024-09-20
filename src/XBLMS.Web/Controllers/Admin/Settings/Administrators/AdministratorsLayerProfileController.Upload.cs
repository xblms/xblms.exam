using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Utils;
using XBLMS.Core.Utils;
using XBLMS.Enums;

namespace XBLMS.Web.Controllers.Admin.Settings.Administrators
{
    public partial class AdministratorsLayerProfileController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(RouteUpload)]
        public async Task<ActionResult<StringResult>> Upload([FromQuery] string userName, [FromForm] IFormFile file)
        {

            var (success, msg, url) = await _uploadManager.UploadAvatar(file, UploadManageType.AdminAvatar, userName);
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
