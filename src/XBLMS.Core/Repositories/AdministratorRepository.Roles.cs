using System.Collections.Generic;
using System.Threading.Tasks;

namespace XBLMS.Core.Repositories
{
    public partial class AdministratorRepository
    {

        public async Task<List<string>> GetRoleNames(int adminId)
        {
            var roleIds = await _administratorsInRolesRepository.GetRoleIdsForAdminAsync(adminId);
            var roleNameList = new List<string>();
            foreach (var roleId in roleIds) {
                var role=await _roleRepository.GetRoleAsync(roleId);
                if(role == null) continue;
                roleNameList.Add(role.RoleName);
            }
            return roleNameList;
        }

    }
}
