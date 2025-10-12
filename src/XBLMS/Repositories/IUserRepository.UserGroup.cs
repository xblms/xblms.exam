using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IUserRepository
    {
        Task<int> UserGroupGetUserCountAsync(UserGroup userGroup);
        Task<List<int>> UserGroupGetUserIdsAsync(UserGroup userGroup);
        Task<(int total, List<User> list)> UserGroupRnageGetUserListAsync(AdminAuth auth, int organId, string organType, int groupId, int range, int dayOfLastActivity, string keyword, string order, int pageIndex, int pageSize);
    }
}
