using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Datory;
using XBLMS.Core.Utils;
using XBLMS.Services;
using XBLMS.Models;
using XBLMS.Repositories;

namespace XBLMS.Core.Repositories
{
    public class DbRecoverRepository : IDbRecoverRepository
    {
        private readonly Repository<DbRecover> _repository;

        public DbRecoverRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<DbRecover>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;
        private string CacheKey => CacheUtils.GetListKey(_repository.TableName);
        public async Task<DbRecover> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }
        public async Task<(int total, List<DbRecover> list)> GetListAsync(int pageIndex, int pageSize)
        {
            var total = await _repository.CountAsync();
            var list = await _repository.GetAllAsync(Q.OrderByDesc(nameof(DbRecover.Id)).ForPage(pageIndex, pageSize));
            return (total, list);
        }
        public async Task<int> InsertAsync(DbRecover info)
        {
            return await _repository.InsertAsync(info);
        }
        public async Task<bool> UpdateAsync(DbRecover info)
        {
            return await _repository.UpdateAsync(info);
        }
        public async Task DeleteAsync()
        {
            await _repository.DeleteAsync();
        }
    }
}
