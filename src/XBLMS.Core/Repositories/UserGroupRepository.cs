using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Datory;
using XBLMS.Core.Utils;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Repositories
{
    public partial class UserGroupRepository : IUserGroupRepository
    {
        private readonly Repository<UserGroup> _repository;
        private readonly IConfigRepository _configRepository;

        public UserGroupRepository(ISettingsManager settingsManager, IConfigRepository configRepository)
        {
            _repository = new Repository<UserGroup>(settingsManager.Database, settingsManager.Redis);
            _configRepository = configRepository;
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        private string CacheKey => CacheUtils.GetListKey(_repository.TableName);

        public async Task<int> InsertAsync(UserGroup group)
        {
            return await _repository.InsertAsync(group, Q.CachingRemove(CacheKey));
        }

        public async Task UpdateAsync(UserGroup group)
        {
            await _repository.UpdateAsync(group, Q.CachingRemove(CacheKey));
        }

        public async Task ClearCache()
        {
            await _repository.RemoveCacheAsync(CacheKey);
        }

        public async Task DeleteAsync(int groupId)
        {
            await _repository.DeleteAsync(groupId, Q.CachingRemove(CacheKey));
        }
        public async Task ResetAsync()
        {
            await _repository.InsertAsync(new UserGroup
            {
                GroupName="全部用户",
                GroupType=UsersGroupType.All
            }, Q.CachingRemove(CacheKey));
        }

    }
}
