using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Points
{
    public partial class UtilitiesPointShopController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(RouteItemUpload)]
        public async Task<ActionResult<GetUploadResult>> Upload([FromForm] IFormFile file)
        {
            var (success, msg, url) = await _uploadManager.UploadCover(file);
            if (success)
            {
                return new GetUploadResult
                {
                    Name = msg,
                    Url = url
                };
            }
            else
            {
                return this.Error(msg);
            }
        }
    }
}
