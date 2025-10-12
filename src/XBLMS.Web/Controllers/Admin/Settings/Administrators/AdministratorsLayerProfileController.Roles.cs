using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XBLMS.Web.Controllers.Admin.Settings.Administrators
{
    public partial class AdministratorsLayerProfileController
    {
        [HttpGet, Route(RouteRoles)]
        public async Task<ActionResult<GetRolesResult>> GetRoles()
        {
            var adminAuth = await _authManager.GetAdminAuth();

            var resultRoles = new List<GetRolesResultInfo>();

            var roles = await _roleRepository.GetRolesAsync(adminAuth);
            if (roles != null && roles.Count > 0)
            {
                foreach (var item in roles)
                {
                    resultRoles.Add(new GetRolesResultInfo { Name = item.RoleName, Id = item.Id });
                }
            }

            return new GetRolesResult()
            {
                List = resultRoles
            };

        }
    }
}
