using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Study
{
    public partial class StudyCourseFilesController
    {
        [HttpGet, Route(RouteActionsDownload)]
        public async Task<ActionResult> ActionsDownload([FromQuery] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Download))
            {
                return this.NoAuth();
            }

            var admin = await _authManager.GetAdminAsync();
            if (admin == null) return Unauthorized();

            var file = await _studyCourseFilesRepository.GetAsync(request.Id);
            if (file == null || string.IsNullOrEmpty(file.Url)) return NotFound();

            var filePath = PathUtils.Combine(_settingsManager.WebRootPath, file.Url);
            if (!FileUtils.IsFileExists(filePath)) return NotFound();

            await _authManager.AddAdminLogAsync("下载课件", file.FileName);
            await _authManager.AddStatLogAsync(StatType.Export, "下载课件", file.Id, file.FileName, new StringResult { Value = _pathManager.GetRootUrlByPath(filePath) });

            return this.Download(filePath);
        }
    }
}
