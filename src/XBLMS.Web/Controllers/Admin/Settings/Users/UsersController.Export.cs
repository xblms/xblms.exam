using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

            var companyIds = new List<int>();
            var departmentIds = new List<int>();
            var dutyIds = new List<int>();
            if (request.OrganId == 0) { }
            else if (request.OrganId == 1 && request.OrganType == "company") { }
            else
            {
                if (request.OrganId != 0)
                {
                    if (request.OrganType == "company")
                    {
                        companyIds = await _organManager.GetCompanyIdsAsync(request.OrganId);
                    }
                    if (request.OrganType == "department")
                    {
                        departmentIds = await _organManager.GetDepartmentIdsAsync(request.OrganId);
                    }
                    if (request.OrganType == "duty")
                    {
                        dutyIds = await _organManager.GetDutyIdsAsync(request.OrganId);
                    }
                }
            }

            var group = await _userGroupRepository.GetAsync(request.GroupId);
            var userIds = new List<int>();
            if (group != null)
            {
                if (group.GroupType == Enums.UsersGroupType.Fixed)
                {
                    userIds = group.UserIds;
                }
                if (group.GroupType == Enums.UsersGroupType.Range)
                {
                    userIds = await _userRepository.GetUserIdsAsync(group.CompanyIds, group.DepartmentIds, group.DutyIds);
                }
            }

            var ids = await _userRepository.GetUserIdsAsync(companyIds, departmentIds, dutyIds, userIds, request.LastActivityDate, request.Keyword, request.Order);

            var fileName = "用户列表.xlsx";
            var filePath = _pathManager.GetDownloadFilesPath(fileName);

            var excelObject = new ExcelObject(_databaseManager, _pathManager, _organManager, _examManager);
            await excelObject.CreateExcelFileForUsersAsync(ids, filePath);

            var downloadUrl = _pathManager.GetDownloadFilesUrl(fileName);

            return new StringResult
            {
                Value = downloadUrl
            };
        }
    }
}
