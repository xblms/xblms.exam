using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Configuration;
using XBLMS.Core.Utils;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Common.Editor
{
    public partial class ActionsController
    {
        [HttpPost, Route(RouteActionsUploadScrawl)]
        public async Task<ActionResult<UploadScrawlResult>> UploadScrawl([FromQuery] int siteId, [FromForm] UploadScrawlRequest request)
        {
            var bytes = Convert.FromBase64String(request.File);

            var original = "scrawl.png";
            var fileName = _pathManager.GetUploadFileName(original);

            if (!_pathManager.IsImageExtensionAllowed(PathUtils.GetExtension(fileName)))
            {
                return new UploadScrawlResult
                {
                    Error = Constants.ErrorImageExtensionAllowed
                };
            }
            if (!_pathManager.IsImageSizeAllowed(request.File.Length))
            {
                return new UploadScrawlResult
                {
                    Error = Constants.ErrorImageSizeAllowed
                };
            }

            var filePath =  _pathManager.GetEditUploadFilesPath(fileName);

            await _pathManager.UploadAsync(bytes, filePath);

            var imageUrl =  _pathManager.GetRootUrlByPath(filePath);


            return new UploadScrawlResult
            {
                State = "SUCCESS",
                Url = imageUrl,
                Title = original,
                Original = original,
                Error = null
            };
        }
    }
}
