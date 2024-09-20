using System.Collections.Generic;
using System.Threading.Tasks;
using Datory;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IUserGroupRepository : IRepository
    {
        Task<int> InsertAsync(UserGroup group);

        Task UpdateAsync(UserGroup group);

        Task DeleteAsync(int groupId);

        Task ClearCache();
        Task ResetAsync();
    }
}
