using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public partial class KnowlegesTreeRepository : IKnowlegesTreeRepository
    {
        private readonly ISettingsManager _settingsManager;
        private readonly Repository<KnowledgesTree> _repository;
        private readonly string _cacheKey;

        public KnowlegesTreeRepository(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _repository = new Repository<KnowledgesTree>(settingsManager.Database, settingsManager.Redis);
            _cacheKey = CacheUtils.GetEntityKey(TableName);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<int> InsertAsync(KnowledgesTree item)
        {
            return await _repository.InsertAsync(item, Q.CachingRemove(_cacheKey));
        }

        public async Task<bool> UpdateAsync(KnowledgesTree item)
        {
            return await _repository.UpdateAsync(item, Q.CachingRemove(_cacheKey));
        }
        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(Q.WhereLike(nameof(KnowledgesTree.ParentPath), $"%'{id}'").CachingRemove(_cacheKey)) > 0;
        }
        public async Task<bool> DeleteAsync(List<int> ids)
        {
            return await _repository.DeleteAsync(Q.WhereIn(nameof(KnowledgesTree.Id), ids).CachingRemove(_cacheKey)) > 0;
        }
    }
}
