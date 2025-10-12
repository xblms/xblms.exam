using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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

            if (userId > 0)
            {
                var user = await _userRepository.GetByUserIdAsync(userId);
                var organId = user.CompanyId;
                var organType = "company";
                var organName = "";

                if (user.DepartmentId > 0)
                {
                    var department = await _organManager.GetDepartmentAsync(user.DepartmentId);
                    organId = user.DepartmentId;
                    organType = "department";
                    organName = department.Name;
                }
                else
                {
                    var company = await _organManager.GetCompanyAsync(user.CompanyId);
                    organName = company.Name;
                }

                return new GetResult
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    DisplayName = user.DisplayName,
                    AvatarUrl = user.AvatarUrl,
                    Mobile = user.Mobile,
                    Email = user.Email,
                    OrganId = organId,
                    OrganType = organType,
                    OrganName = organName,
                    Locked = user.Locked,
                    DutyName = user.DutyName
                };
            }
            return new GetResult();


        }
    }
}
