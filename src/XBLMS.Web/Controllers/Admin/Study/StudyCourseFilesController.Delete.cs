using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Study
{
    public partial class StudyCourseFilesController
    {
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<BoolResult>> Delete([FromBody] IdRequest request)
        {
            if (_settingsManager.IsSafeMode)
            {
                return this.Error(Constants.ErrorSafe);
            }

            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Delete))
            {
                return this.NoAuth();
            }

            var admin = await _authManager.GetAdminAsync();

            var info = await _studyCourseFilesRepository.GetAsync(request.Id);
            if (info == null) return NotFound();
            await _studyCourseFilesRepository.DeleteAsync(info.Id);

            await _authManager.AddAdminLogAsync("删除课件", info.FileName);
            await _authManager.AddStatLogAsync(StatType.StudyFileDelete, "删除课件", info.Id, info.FileName);
            await _authManager.AddStatCount(StatType.StudyFileDelete);

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
