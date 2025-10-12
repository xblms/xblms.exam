using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public partial class OrganCompanyRepository : IOrganCompanyRepository
    {
        private readonly ISettingsManager _settingsManager;
        private readonly Repository<OrganCompany> _repository;

        public OrganCompanyRepository(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _repository = new Repository<OrganCompany>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<int> InsertAsync(OrganCompany company)
        {
            return await _repository.InsertAsync(company);
        }

        public async Task<bool> UpdateAsync(OrganCompany company)
        {
            return await _repository.UpdateAsync(company, Q.CachingRemove(GetCacheKey(company.Id)));
        }

        public async Task<bool> DeleteByIdsAsync(List<int> ids)
        {
            if (ids != null)
            {
                foreach (int id in ids)
                {
                    await _repository.DeleteAsync(id, Q.CachingRemove(GetCacheKey(id)));
                }
            }
            return true;
        }
    }
}
