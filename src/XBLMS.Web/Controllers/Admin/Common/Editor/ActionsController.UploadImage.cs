using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Common.Editor
{
    public partial class ActionsController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(RouteActionsUploadImage)]
        public async Task<ActionResult<UploadImageResult>> UploadImage([FromForm] IFormFile file)
        {
            if (file == null)
            {
                return new UploadImageResult
                {
                    Error = Constants.ErrorUpload
                };
            }

            var original = Path.GetFileName(file.FileName);
            var fileName = _pathManager.GetUploadFileName(original);

            if (!_pathManager.IsImageExtensionAllowed(PathUtils.GetExtension(fileName)))
            {
                return new UploadImageResult
                {
                    Error = Constants.ErrorImageExtensionAllowed
                };
            }
            if (!_pathManager.IsImageSizeAllowed(file.Length))
            {
                return new UploadImageResult
                {
                    Error = Constants.ErrorImageSizeAllowed
                };
            }

            var filePath = _pathManager.GetEditUploadFilesPath(fileName);

            await _pathManager.UploadAsync(file, filePath);

            var fileUrl = _pathManager.GetRootUrlByPath(filePath);

            return new UploadImageResult
            {
                State = "SUCCESS",
                Url = fileUrl,
                Title = original,
                Original = original,
                Error = null
            };
        }

        public class UploadImageResult
        {
            public string State { get; set; }
            public string Url { get; set; }
            public string Title { get; set; }
            public string Original { get; set; }
            public string Error { get; set; }
        }
    }
}
