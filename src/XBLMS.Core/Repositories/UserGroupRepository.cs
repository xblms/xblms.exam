using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

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

        public async Task<int> InsertAsync(UserGroup group)
        {
            return await _repository.InsertAsync(group);
        }

        public async Task UpdateAsync(UserGroup group)
        {
            await _repository.UpdateAsync(group);
        }

        public async Task DeleteAsync(int groupId)
        {
            await _repository.DeleteAsync(groupId);
        }
 
        public async Task ResetAsync()
        {
            await _repository.InsertAsync(new UserGroup
            {
                CreatorId = 1,
                GroupName = "全部用户",
                GroupType = UsersGroupType.All
            });
        }

    }
}
