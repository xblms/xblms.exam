using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamCerEditController
    {
        [HttpPost, Route(RouteUpload)]
        public async Task<ActionResult<StringResult>> Upload([FromForm] IFormFile file)
        {
            var admin = await _authManager.GetAdminAsync();

            if (file == null) return this.Error(Constants.ErrorUpload);
            var extension = PathUtils.GetExtension(file.FileName);
            if (!FileUtils.IsImage(extension))
            {
                return this.Error(Constants.ErrorImageExtensionAllowed);
            }
            var fileName = _pathManager.GetUploadFileName(file.FileName);
            var filePath = _pathManager.GetCerUploadFilesPath(fileName);
            await _pathManager.UploadAsync(file, filePath);

            var url = _pathManager.GetRootUrlByPath(filePath);

            return new StringResult
            {
                Value = url
            };
        }
    }
}
