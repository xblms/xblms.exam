using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public partial class OrganDepartmentRepository : IOrganDepartmentRepository
    {
        private readonly ISettingsManager _settingsManager;
        private readonly Repository<OrganDepartment> _repository;
        private readonly string _cacheKey;

        public OrganDepartmentRepository(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _repository = new Repository<OrganDepartment>(settingsManager.Database, settingsManager.Redis);
            _cacheKey = CacheUtils.GetEntityKey(TableName);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<int> InsertAsync(OrganDepartment department)
        {
            return await _repository.InsertAsync(department, Q.CachingRemove(_cacheKey));
        }

        public async Task<bool> UpdateAsync(OrganDepartment department)
        {
            return await _repository.UpdateAsync(department, Q.CachingRemove(_cacheKey));
        }
        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id, Q.CachingRemove(_cacheKey));
        }
    }
}
