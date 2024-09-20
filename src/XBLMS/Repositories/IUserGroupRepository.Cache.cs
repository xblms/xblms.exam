using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IUserGroupRepository
    {
        Task<UserGroup> GetAsync(int id);
        Task<List<UserGroup>> GetListAsync();
        Task<List<UserGroup>> GetListWithoutLockedAsync();
    }
}
