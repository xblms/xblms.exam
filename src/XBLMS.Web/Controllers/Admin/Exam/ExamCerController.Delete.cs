using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

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
                await _authManager.AddAdminLogAsync("删除证书模板", $"{cerInfo.Name}");

                await _authManager.AddStatLogAsync(StatType.ExamCerDelete, "删除证书模板", cerInfo.Id, cerInfo.Name, cerInfo);
                await _authManager.AddStatCount(StatType.ExamCerDelete);
            }

            return new BoolResult
            {
                Value = true
            };
        }

    }
}
