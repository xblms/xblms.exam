using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Core.Utils.Office;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Knowledges
{
    public partial class KnowledgesController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(RouteUpload)]
        public async Task<ActionResult<GetUploadResult>> Upload([FromForm] IFormFile file)
        {
            if (_settingsManager.IsSafeMode)
            {
                return new GetUploadResult { Success = false, Msg = Constants.ErrorSafe };
            }
            var fileName = PathUtils.GetFileName(file.FileName);

            var fileType = PathUtils.GetExtension(fileName);

            if (!FileUtils.IsPDF(fileType))
            {
                return new GetUploadResult { Success = false, Msg = Constants.ErrorUpload };
            }


            var realFileName = PathUtils.GetFileNameWithoutExtension(fileName);


            var path = _pathManager.GetKnowledgesUploadFilesPath();
            var filePath = PathUtils.Combine(path, fileName);

            await _pathManager.UploadAsync(file, filePath);

            var url = _pathManager.GetRootUrlByPath(filePath);

            var firstImgPath = PathUtils.Combine(path, $"{StringUtils.GetShortGuid()}.jpg");
            var firstImgUrl = string.Empty;

            try
            {
                PdfManager.GetFirstImg(filePath, firstImgPath);
                firstImgUrl = _pathManager.GetRootUrlByPath(firstImgPath);
            }
            catch
            {
                firstImgUrl = _pathManager.DefaultBookCoverUrl;
            }
     
            return new GetUploadResult
            {
                Success = true,
                FileName = realFileName,
                FilePath = url,
                CoverImagePath = firstImgUrl
            };
       

        }
    }
}
