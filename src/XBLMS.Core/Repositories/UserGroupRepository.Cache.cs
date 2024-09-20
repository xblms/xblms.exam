using Datory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class UserGroupRepository
    {
        public async Task<UserGroup> GetAsync(int id)
        {
            var list = await GetListAsync();
            return list.FirstOrDefault(group => group.Id == id);
        }

        public async Task<List<UserGroup>> GetListAsync()
        {
            var list = (await _repository.GetAllAsync(Q
                .OrderByDesc(nameof(UserGroup.Id))
                .CachingGet(CacheKey)
            )).ToList();

            return list;
        }
        public async Task<List<UserGroup>> GetListWithoutLockedAsync()
        {
            var list = (await _repository.GetAllAsync(Q
                .OrderByDesc(nameof(UserGroup.Id))
                .CachingGet(CacheKey)
            )).ToList();

            return list.Where(g => g.Locked == false).ToList();
        }
    }
}
