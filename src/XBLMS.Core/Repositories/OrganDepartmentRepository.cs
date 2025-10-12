using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Repositories
{
    public partial class OrganDepartmentRepository : IOrganDepartmentRepository
    {
        private readonly ICacheManager _cacheManager;
        private readonly ISettingsManager _settingsManager;
        private readonly Repository<OrganDepartment> _repository;

        public OrganDepartmentRepository(ISettingsManager settingsManager, ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
            _settingsManager = settingsManager;
            _repository = new Repository<OrganDepartment>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<int> InsertAsync(OrganDepartment department)
        {
            return await _repository.InsertAsync(department);
        }

        public async Task<bool> UpdateAsync(OrganDepartment department)
        {
            return await _repository.UpdateAsync(department, Q.CachingRemove(GetCacheKey(department.Id)));
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

        public async Task<bool> DeleteByCompanyIdsAsync(List<int> ids)
        {
            var allDepartmentIds = await _repository.GetAllAsync<int>(Q.Select(nameof(OrganDepartment.Id)).WhereIn(nameof(OrganDepartment.CompanyId), TranslateUtils.ToSqlInStringWithoutQuote(ids)));
            await DeleteByIdsAsync(allDepartmentIds);
            return true;

        }
    }
}
