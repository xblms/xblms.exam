using Microsoft.AspNetCore.Mvc;
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
