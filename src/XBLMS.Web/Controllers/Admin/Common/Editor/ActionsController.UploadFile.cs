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
        [HttpPost, Route(RouteActionsUploadFile)]
        public async Task<ActionResult<UploadFileResult>> UploadFile([FromForm] IFormFile file)
        {

            if (file == null)
            {
                return new UploadFileResult
                {
                    Error = Constants.ErrorUpload
                };
            }

            var original = Path.GetFileName(file.FileName);
            var fileName = _pathManager.GetUploadFileName(original);

            if (!_pathManager.IsFileExtensionAllowed(PathUtils.GetExtension(fileName)))
            {
                return new UploadFileResult
                {
                    Error = Constants.ErrorFileExtensionAllowed
                };
            }
            if (!_pathManager.IsFileSizeAllowed(file.Length))
            {
                return new UploadFileResult
                {
                    Error = Constants.ErrorFileSizeAllowed
                };
            }

            var filePath =  _pathManager.GetEditUploadFilesPath(fileName);

            await _pathManager.UploadAsync(file, filePath);

            var fileUrl =  _pathManager.GetRootUrlByPath(filePath);


            return new UploadFileResult
            {
                State = "SUCCESS",
                Url = fileUrl,
                Title = original,
                Original = original,
                Error = null
            };
        }

        public class UploadFileResult
        {
            public string State { get; set; }
            public string Url { get; set; }
            public string Title { get; set; }
            public string Original { get; set; }
            public string Error { get; set; }
        }
    }
}
