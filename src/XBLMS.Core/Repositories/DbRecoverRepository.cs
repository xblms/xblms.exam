using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

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
        public async Task DeleteAsync()
        {
            await _repository.DeleteAsync();
        }
    }
}
