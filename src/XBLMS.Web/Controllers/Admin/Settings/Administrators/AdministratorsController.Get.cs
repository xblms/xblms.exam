using Datory;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Enums;
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

            var adminAuth = await _authManager.GetAdminAuth();

            var (total, list) = await _administratorRepository.GetAdministratorsAsync(adminAuth, request.OrganId, request.OrganType, request.Role, request.Order, request.LastActivityDate, request.Keyword, request.Offset, request.Limit);
            var admins = new List<Admin>();
            foreach (var administratorInfo in list)
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
                    Roles = administratorInfo.Auth.GetDisplayName(),
                    CreatorId = administratorInfo.CreatorId
                });
            }

            return new GetResult
            {
                Administrators = admins,
                Count = total,
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

            var adminAuth = await _authManager.GetAdminAuth();

            if (adminAuth.AuthType != AuthorityType.Admin)
            {
                roles = roles.Where(r => r.Value != AuthorityType.Admin.GetValue()).ToList();
                if (adminAuth.AuthType == AuthorityType.AdminNormal)
                {
                    roles = roles.Where(r => r.Value != AuthorityType.AdminCompany.GetValue()).ToList();
                }
            }

            return new GetResult
            {
                Roles = roles,
                AdminId = adminId
            };
        }
    }
}
