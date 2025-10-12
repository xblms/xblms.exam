using Datory;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class SelectAdministratorsController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
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
                    Roles = administratorInfo.Auth.GetDisplayName()
                });
            }

            return new GetResult
            {
                Administrators = admins,
                Count = total,
            };
        }
    }
}
