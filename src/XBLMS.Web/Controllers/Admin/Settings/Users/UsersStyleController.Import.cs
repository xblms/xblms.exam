using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Users
{
    public partial class UsersStyleController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(RouteImport)]
        public async Task<ActionResult<BoolResult>> Import([FromForm] IFormFile file)
        {
            var admin = await _authManager.GetAdminAsync();
      

            if (file == null)
            {
                return this.Error(Constants.ErrorUpload);
            }

            var fileName = Path.GetFileName(file.FileName);

            var sExt = PathUtils.GetExtension(fileName);
            if (!StringUtils.EqualsIgnoreCase(sExt, ".zip"))
            {
                return this.Error("导入文件为 Zip 格式，请选择有效的文件上传");
            }

            var filePath = _pathManager.GetTemporaryFilesPath(fileName);
            await _pathManager.UploadAsync(file, filePath);

            var directoryPath = "";

            FileUtils.DeleteFileIfExists(filePath);
            DirectoryUtils.DeleteDirectoryIfExists(directoryPath);

            await _authManager.AddAdminLogAsync("导入用户字段");

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
