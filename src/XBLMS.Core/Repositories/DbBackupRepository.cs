using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class DbBackupRepository : IDbBackupRepository
    {
        private readonly Repository<DbBackup> _repository;

        public DbBackupRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<DbBackup>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;
        private string CacheKey => CacheUtils.GetListKey(_repository.TableName);
        public async Task<DbBackup> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }
        public async Task<(int total, List<DbBackup> list)> GetListAsync(int pageIndex,int pageSize)
        {
            var total=await _repository.CountAsync();
            var list= await _repository.GetAllAsync(Q
                 .OrderByDesc(nameof(DbBackup.Id)).ForPage(pageIndex, pageSize));
            return (total, list);
        }
        public async Task<int> InsertAsync(DbBackup info)
        {
            return await _repository.InsertAsync(info);
        }
        public async Task<bool> UpdateAsync(DbBackup info)
        {
            return await _repository.UpdateAsync(info);
        }
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
        public async Task<bool> ExcutionAsync()
        {
            return await _repository.ExistsAsync(Q.Where(nameof(DbBackup.Status), 0));
        }
    }
}
