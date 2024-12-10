using Datory;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;

namespace XBLMS.Web.Controllers.Admin.Settings.Administrators
{
    public partial class AdministratorsLayerProfileController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var admin = await _authManager.GetAdminAsync();
            var organs = await _organManager.GetOrganTreeTableDataAsync();

            var auths = _authManager.AuthorityTypes();
            if (admin.Auth == AuthorityType.AdminSelf)
            {
                auths = new List<Dto.Select<string>>();
                auths.Add(new Select<string>(AuthorityType.AdminSelf));
            }

            if (!string.IsNullOrEmpty(request.UserName))
            {
                var administrator = await _administratorRepository.GetByUserNameAsync(request.UserName);
                var company = await _organManager.GetCompanyAsync(administrator.CompanyId);
                var department = await _organManager.GetDepartmentAsync(administrator.DepartmentId);
                var duty = await _organManager.GetDutyAsync(administrator.DutyId);

                var organId = "";
                if (company != null) { organId = company.Guid; }
                if (department != null) { organId = department.Guid; }
                if (duty != null) { organId = duty.Guid; }

                return new GetResult
                {
                    UserId = administrator.Id,
                    UserName = administrator.UserName,
                    DisplayName = administrator.DisplayName,
                    AvatarUrl = administrator.AvatarUrl,
                    Mobile = administrator.Mobile,
                    Email = administrator.Email,
                    Auth = administrator.Auth.GetValue(),
                    OrganId = organId,
                    Auths = auths,
                    Organs = organs
                };
            }

            return new GetResult()
            {
                Auths = auths,
                Organs = organs
            };

        }
    }
}
