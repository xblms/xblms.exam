using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Core.Utils.Office;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Users
{
    public partial class UsersController
    {
        [HttpPost, Route(RouteExport)]
        public async Task<ActionResult<StringResult>> Export([FromBody] GetRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Export))
            {
                return this.NoAuth();
            }

            var adminAuth = await _authManager.GetAdminAuth();
            var group = await _userGroupRepository.GetAsync(request.GroupId);

            var fileName = "用户列表.xlsx";
            var filePath = _pathManager.GetDownloadFilesPath(fileName);

            var excelObject = new ExcelObject(_databaseManager, _pathManager, _organManager, _examManager);
            await excelObject.CreateExcelFileForUsersAsync(adminAuth, request.OrganId, request.OrganType, group, request.LastActivityDate, request.Keyword, request.Order, filePath);

            var downloadUrl = _pathManager.GetDownloadFilesUrl(fileName);

            await _authManager.AddAdminLogAsync("导出用户账号");

            await _authManager.AddStatLogAsync(StatType.Export, "导出用户账号", 0, string.Empty, new StringResult { Value = downloadUrl });

            return new StringResult
            {
                Value = downloadUrl
            };
        }
    }
}
