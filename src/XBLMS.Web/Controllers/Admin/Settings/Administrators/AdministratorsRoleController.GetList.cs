using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XBLMS.Web.Controllers.Admin.Settings.Administrators
{
    public partial class AdministratorsRoleController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<ListRequest>> GetList()
        {
            var allRoles = await _roleRepository.GetRolesAsync();
            foreach (var role in allRoles)
            {
                var admins = new List<KeyValuePair<int, string>>();

                var adminIds = await _administratorsInRolesRepository.GetUserIdsInRoleAsync(role.Id);
                if (adminIds != null)
                {
                    foreach (var adminId in adminIds)
                    {
                        var admin = await _administratorRepository.GetByUserIdAsync(adminId);
                        if (admin != null)
                        {
                            admins.Add(new KeyValuePair<int, string>(admin.Id, admin.DisplayName));
                        }
                    }
                }
                role.Set("Admins", admins);
                role.Set("AdminCount", adminIds?.Count);
                var creator = await _administratorRepository.GetByUserIdAsync(role.CreatorId);
                if (creator != null)
                {
                    role.Set("CreaterName", creator.DisplayName);
                    role.Set("CreaterId", creator.Id);
                }
    
            }
            

            return new ListRequest
            {
                Roles = allRoles
            };
        }
    }
}
