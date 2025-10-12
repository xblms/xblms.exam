using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IUserGroupRepository
    {
        Task<bool> ExistsAsync(string name, int companyId);
        Task<UserGroup> GetAsync(int id);
        Task<List<UserGroup>> GetListAsync(AdminAuth auth, bool withoutLocked = false);
        Task<List<UserGroup>> GetListAsync(AdminAuth auth, string keyWords);
    }
}
