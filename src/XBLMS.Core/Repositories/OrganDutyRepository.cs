using System.Collections.Generic;
using System.Threading.Tasks;
using Datory;
using XBLMS.Core.Utils;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Repositories
{
    public partial class OrganDutyRepository : IOrganDutyRepository
    {
        private readonly ISettingsManager _settingsManager;
        private readonly Repository<OrganDuty> _repository;
        private readonly string _cacheKey;

        public OrganDutyRepository(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _repository = new Repository<OrganDuty>(settingsManager.Database, settingsManager.Redis);
            _cacheKey = CacheUtils.GetEntityKey(TableName);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<int> InsertAsync(OrganDuty organDuty)
        {
            return await _repository.InsertAsync(organDuty, Q.CachingRemove(_cacheKey));
        }

        public async Task<bool> UpdateAsync(OrganDuty organDuty)
        {
            return await _repository.UpdateAsync(organDuty, Q.CachingRemove(_cacheKey));
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id, Q.CachingRemove(_cacheKey));
        }
    }
}
