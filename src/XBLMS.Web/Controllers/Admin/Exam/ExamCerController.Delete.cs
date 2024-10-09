using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Dto;
using XBLMS.Core.Utils;
using XBLMS.Utils;
using XBLMS.Enums;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamCerController
    {
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<BoolResult>> Delete([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Delete))
            {
                return this.NoAuth();
            }

            var cerInfo = await _examCerRepository.GetAsync(request.Id);
            if (cerInfo != null)
            {
                var filePath = PathUtils.Combine(_settingsManager.WebRootPath, cerInfo.BackgroundImg);
                var pre_filePath = PathUtils.Combine(_settingsManager.WebRootPath, $"{StringUtils.Replace(cerInfo.BackgroundImg, "cer", "preview_cer")}");
                FileUtils.DeleteFileIfExists(filePath);
                FileUtils.DeleteFileIfExists(pre_filePath);

                await _examCerRepository.DeleteAsync(request.Id);
                await _authManager.AddAdminLogAsync("É¾³ýÖ¤ÊéÄ£°å", $"{cerInfo.Name}");
            }

            return new BoolResult
            {
                Value = true
            };
        }

    }
}
