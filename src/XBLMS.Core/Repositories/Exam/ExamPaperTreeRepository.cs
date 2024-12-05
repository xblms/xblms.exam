using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public partial class ExamPaperTreeRepository : IExamPaperTreeRepository
    {
        private readonly ISettingsManager _settingsManager;
        private readonly Repository<ExamPaperTree> _repository;
        private readonly string _cacheKey;

        public ExamPaperTreeRepository(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _repository = new Repository<ExamPaperTree>(settingsManager.Database, settingsManager.Redis);
            _cacheKey = CacheUtils.GetEntityKey(TableName);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<int> InsertAsync(ExamPaperTree item)
        {
            return await _repository.InsertAsync(item, Q.CachingRemove(_cacheKey));
        }

        public async Task<bool> UpdateAsync(ExamPaperTree item)
        {
            return await _repository.UpdateAsync(item, Q.CachingRemove(_cacheKey));
        }

        public async Task<bool> DeleteAsync(List<int> ids)
        {
            return await _repository.DeleteAsync(Q.WhereIn(nameof(ExamPaperTree.Id), ids).CachingRemove(_cacheKey)) > 0;
        }
    }
}
