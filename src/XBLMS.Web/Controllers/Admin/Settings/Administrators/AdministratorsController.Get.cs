using Datory;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Administrators
{
    public partial class AdministratorsController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            if (!await _authManager.HasPermissionsAsync())
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
            var count = await _administratorRepository.GetCountAsync(companyIds, departmentIds, dutyIds, request.Role, request.LastActivityDate, request.Keyword);
            var administrators = await _administratorRepository.GetAdministratorsAsync(companyIds, departmentIds, dutyIds, request.Role, request.Order, request.LastActivityDate, request.Keyword, request.Offset, request.Limit);
            var admins = new List<Admin>();
            foreach (var administratorInfo in administrators)
            {
                admins.Add(new Admin
                {
                    Id = administratorInfo.Id,
                    Guid = administratorInfo.Guid,
                    AvatarUrl = administratorInfo.AvatarUrl,
                    UserName = administratorInfo.UserName,
                    DisplayName = string.IsNullOrEmpty(administratorInfo.DisplayName)
                        ? administratorInfo.UserName
                        : administratorInfo.DisplayName,
                    Mobile = administratorInfo.Mobile,
                    LastActivityDate = administratorInfo.LastActivityDate,
                    CountOfLogin = administratorInfo.CountOfLogin,
                    Locked = administratorInfo.Locked,
                    Auth = administratorInfo.Auth,
                    Roles = administratorInfo.Auth.GetDisplayName()
                });
            }

            return new GetResult
            {
                Administrators = admins,
                Count = count,
            };
        }


        [HttpGet, Route(RouteOtherData)]
        public async Task<ActionResult<GetResult>> GetOtherData([FromQuery] GetRequest request)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }
            var adminId = _authManager.AdminId;

            var roles = _authManager.AuthorityTypes();
            var organs = await _organManager.GetOrganTreeTableDataAsync();

            return new GetResult
            {
                Organs = organs,
                Roles = roles,
                AdminId = adminId
            };
        }
    }
}
