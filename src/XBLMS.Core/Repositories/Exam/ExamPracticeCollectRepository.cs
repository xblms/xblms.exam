using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class ExamPracticeCollectRepository : IExamPracticeCollectRepository
    {
        private readonly Repository<ExamPracticeCollect> _repository;

        public ExamPracticeCollectRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamPracticeCollect>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<ExamPracticeCollect> GetAsync(int userId)
        {
            return await _repository.GetAsync(Q.Where(nameof(ExamPracticeCollect.UserId), userId));
        }

        public async Task<int> InsertAsync(ExamPracticeCollect item)
        {
            return await _repository.InsertAsync(item);
        }

        public async Task UpdateAsync(ExamPracticeCollect item)
        {
            await _repository.UpdateAsync(item);
        }
    }
}
