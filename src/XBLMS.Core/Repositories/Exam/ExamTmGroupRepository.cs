using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<int> InsertAsync(ExamTmGroup group)
        {
            return await _repository.InsertAsync(group);
        }

        public async Task UpdateAsync(ExamTmGroup group)
        {
            await _repository.UpdateAsync(group);
        }
        public async Task DeleteAsync(int groupId)
        {
            await _repository.DeleteAsync(groupId);
        }

        public async Task ResetAsync()
        {
            await _repository.InsertAsync(new ExamTmGroup
            {
                GroupName = "全部题目",
                GroupType = TmGroupType.All,
                CompanyId = 1,
                CreatorId = 1
            });
        }
    }
}
