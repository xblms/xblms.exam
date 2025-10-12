using Datory;
using System;
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
        public async Task<DbBackup> GetBackupingAsync()
        {
            return await _repository.GetAsync(Q.Where(nameof(DbBackup.Status), 0));
        }
        public async Task<bool> ExistsBackupingAsync()
        {
            return await _repository.ExistsAsync(Q.Where(nameof(DbBackup.Status), 0));
        }
        public async Task<bool> ExistsTodayAsync()
        {
            var query = Q.NewQuery();

            var dateFromStr = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            var dateToStr = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");

            query.Where(nameof(DbBackup.EndTime), ">=", DateUtils.ToString(dateFromStr));
            query.Where(nameof(DbBackup.EndTime), "<=", DateUtils.ToString(dateToStr));

            return await _repository.ExistsAsync(query);
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
    }
}
