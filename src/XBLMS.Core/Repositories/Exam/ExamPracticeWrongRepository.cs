using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class ExamPracticeWrongRepository : IExamPracticeWrongRepository
    {
        private readonly Repository<ExamPracticeWrong> _repository;

        public ExamPracticeWrongRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamPracticeWrong>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<ExamPracticeWrong> GetAsync(int userId)
        {
            return await _repository.GetAsync(Q.Where(nameof(ExamPracticeWrong.UserId), userId));
        }

        public async Task<int> InsertAsync(ExamPracticeWrong item)
        {
            return await _repository.InsertAsync(item);
        }

        public async Task UpdateAsync(ExamPracticeWrong item)
        {
            await _repository.UpdateAsync(item);
        }
    }
}
