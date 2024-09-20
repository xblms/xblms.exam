using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Datory;
using XBLMS.Core.Utils;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public partial class ExamTmGroupRepository : IExamTmGroupRepository
    {
        private readonly Repository<ExamTmGroup> _repository;
        private readonly IConfigRepository _configRepository;

        public ExamTmGroupRepository(ISettingsManager settingsManager, IConfigRepository configRepository)
        {
            _repository = new Repository<ExamTmGroup>(settingsManager.Database, settingsManager.Redis);
            _configRepository = configRepository;
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        private string CacheKey => CacheUtils.GetListKey(_repository.TableName);

        public async Task<int> InsertAsync(ExamTmGroup group)
        {
            return await _repository.InsertAsync(group, Q.CachingRemove(CacheKey));
        }

        public async Task UpdateAsync(ExamTmGroup group)
        {
            await _repository.UpdateAsync(group, Q.CachingRemove(CacheKey));
        }

        public async Task ClearCache()
        {
            await _repository.RemoveCacheAsync(CacheKey);
        }

        public async Task DeleteAsync(int groupId)
        {
            await _repository.DeleteAsync(groupId, Q.CachingRemove(CacheKey));
        }

        public async Task ResetAsync()
        {
            await _repository.InsertAsync(new ExamTmGroup
            {
                GroupName = "全部题目",
                GroupType = TmGroupType.All
            }, Q.CachingRemove(CacheKey));
        }
    }
}
