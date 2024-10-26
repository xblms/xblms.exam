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
    public partial class OrganCompanyRepository : IOrganCompanyRepository
    {
        private readonly ISettingsManager _settingsManager;
        private readonly Repository<OrganCompany> _repository;
        private readonly string _cacheKey;

        public OrganCompanyRepository(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _repository = new Repository<OrganCompany>(settingsManager.Database, settingsManager.Redis);
            _cacheKey = CacheUtils.GetEntityKey(TableName);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<int> InsertAsync(OrganCompany company)
        {
            return await _repository.InsertAsync(company, Q.CachingRemove(_cacheKey));
        }

        public async Task<bool> UpdateAsync(OrganCompany company)
        {
            return await _repository.UpdateAsync(company, Q.CachingRemove(_cacheKey));
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id, Q.CachingRemove(_cacheKey));
        }
        public async Task ClearAsync()
        {
            await _repository.DeleteAsync(Q.WhereNot(nameof(OrganCompany.Id), 1).CachingRemove(_cacheKey));
        }
    }
}
