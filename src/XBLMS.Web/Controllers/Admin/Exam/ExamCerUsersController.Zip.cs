using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamCerUsersController
    {
        [HttpGet, Route(RouteZip)]
        public async Task<ActionResult<StringResult>> Zip([FromQuery] GetUserRequest request)
        {

            var (total, list) = await _examCerUserRepository.GetListAsync(request.Id, request.Keywords, request.DateFrom, request.DateTo, request.PageIndex, request.PageSize);

            var cer = await _examCerRepository.GetAsync(request.Id);


            var zipFileName = $"{cer.Name}-÷§ È.zip";
            var zipFilePath = _pathManager.GetDownloadFilesPath(zipFileName);

            DirectoryUtils.CreateDirectoryIfNotExists(zipFilePath);

            foreach (var ceruser in list)
            {
                var user = await _organManager.GetUser(ceruser.UserId);

                var cerUrl = ceruser.CerImg;
                var cerPath = PathUtils.Combine(_settingsManager.WebRootPath, cerUrl);
                var cerNameNew = $"{ceruser.CerNumber}_{user.DisplayName}_{user.UserName}.jpg";
                var cerPathNew = PathUtils.Combine(zipFilePath, cer.Name, cerNameNew);
                FileUtils.CopyFile(cerPath, cerPathNew);
            }

            var zipPath = $"{zipFilePath}\\{zipFileName}";

            var filePath = PathUtils.Combine(zipFilePath, cer.Name);
            _pathManager.CreateZip(zipPath, filePath);
            DirectoryUtils.DeleteDirectoryIfExists(filePath);

            var zipUrl = _pathManager.GetRootUrlByPath(zipPath);
            return new StringResult
            {
                Value = zipUrl
            };
        }

    }
}
