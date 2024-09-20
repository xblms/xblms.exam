using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Configuration;
using XBLMS.Core.Utils;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Users
{
    public partial class UsersLayerProfileController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] int userId)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var organs = await _organManager.GetOrganTreeTableDataAsync();
            if (userId > 0)
            {
                var user = await _userRepository.GetByUserIdAsync(userId);
                var company = await _organManager.GetCompanyAsync(user.CompanyId);
                var department = await _organManager.GetDepartmentAsync(user.DepartmentId);
                var duty = await _organManager.GetDutyAsync(user.DutyId);

                var organId = "";
                if (company != null) { organId = company.Guid; }
                if (department != null) { organId = department.Guid; }
                if (duty != null) { organId = duty.Guid; }

                return new GetResult
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    DisplayName = user.DisplayName,
                    AvatarUrl = user.AvatarUrl,
                    Mobile = user.Mobile,
                    Email = user.Email,
                    OrganId = organId,
                    Organs = organs,
                };
            }
            return new GetResult
            {
                Organs = organs,
            };


        }
    }
}
