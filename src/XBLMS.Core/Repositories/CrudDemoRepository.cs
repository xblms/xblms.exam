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
    public partial class CrudDemoRepository : ICrudDemoRepository
    {
        private readonly ISettingsManager _settingsManager;
        private readonly Repository<CrudDemo> _repository;
        private readonly string _cacheKey;

        public CrudDemoRepository(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _repository = new Repository<CrudDemo>(settingsManager.Database, settingsManager.Redis);
            _cacheKey = CacheUtils.GetEntityKey(TableName);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<int> InsertAsync(CrudDemo info)
        {
            return await _repository.InsertAsync(info);
        }

        public async Task<bool> UpdateAsync(CrudDemo info)
        {
            return await _repository.UpdateAsync(info);
        }
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
        public async Task<CrudDemo> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }
        public async Task<(int total, List<CrudDemo> list)> ListAsync(string title, int pageIndex, int pageSize)
        {
            var qeury = Q.WhereLike(nameof(CrudDemo.title), $"%{title}%").OrderByDesc(nameof(CrudDemo.Id));

            var total = await _repository.CountAsync(qeury);
            var list = await _repository.GetAllAsync(qeury.ForPage(pageIndex, pageSize));
            return (total, list);
        }
    }
}
