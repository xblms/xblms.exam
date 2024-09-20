using System.Collections.Generic;
using System.Threading.Tasks;
using Datory;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IRoleRepository : IRepository
    {
        Task<Role> GetRoleAsync(int roleId);

        Task<List<Role>> GetRolesAsync();


        Task<int> InsertRoleAsync(Role role);

        Task UpdateRoleAsync(Role role);

        Task<bool> DeleteRoleAsync(int roleId);

        Task<bool> IsRoleExistsAsync(string roleName);

        bool IsPredefinedRole(string roleName);
    }
}
