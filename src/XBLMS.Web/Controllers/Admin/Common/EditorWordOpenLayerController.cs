using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Core.Utils;
using XBLMS.Core.Utils.Office;
using XBLMS.Dto;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Common
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class EditorWordOpenLayerController : ControllerBase
    {
        private const string Route = "common/editorWordOpenLayer";

        private readonly IHttpContextAccessor _context;
        private readonly IPathManager _pathManager;

        public EditorWordOpenLayerController(IHttpContextAccessor context, IPathManager pathManager)
        {
            _context = context;
            _pathManager = pathManager;
        }

        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(Route)]
        public async Task<ActionResult<StringResult>> Upload([FromForm] IFormFile file)
        {
            if (file == null)
            {
                return this.Error(Constants.ErrorUpload);
            }

            var fileName = PathUtils.GetFileName(file.FileName);
            var fileType = PathUtils.GetExtension(fileName);

            if (!FileUtils.IsWord(fileType))
            {
                return this.Error(Constants.ErrorUpload);
            }

            var filePath = _pathManager.GetImportFilesPath(_pathManager.GetUploadFileName(file.FileName));
            await _pathManager.UploadAsync(file, filePath);


            string imageDirectoryPath;
            string imageDirectoryUrl;

            imageDirectoryPath = _pathManager.GetEditUploadFilesPath();
            imageDirectoryUrl = _pathManager.GetEditUploadFilesUrl();

            WordManager wordManager = new WordManager(filePath, imageDirectoryPath, imageDirectoryUrl);
            var wordContent = await wordManager.ParseAsync();

            wordContent = HtmlUtils.ClearFontFamily(wordContent);
            wordContent = HtmlUtils.ClearFontSize(wordContent);
            wordContent = HtmlUtils.ClearFormat(wordContent);



            return new StringResult
            {
                Value = wordContent.ToString()
            };
        }

    }
}
