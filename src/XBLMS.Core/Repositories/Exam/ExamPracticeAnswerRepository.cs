using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class ExamPracticeAnswerRepository : IExamPracticeAnswerRepository
    {
        private readonly Repository<ExamPracticeAnswer> _repository;

        public ExamPracticeAnswerRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamPracticeAnswer>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<ExamPracticeAnswer> GetAsync(int userId,int tmId,int practiceId)
        {
            return await _repository.GetAsync(Q.
                Where(nameof(ExamPracticeAnswer.TmId), tmId).
                Where(nameof(ExamPracticeAnswer.PracticeId), practiceId).
                Where(nameof(ExamPracticeAnswer.UserId), userId));
        }

        public async Task<int> InsertAsync(ExamPracticeAnswer item)
        {
            return await _repository.InsertAsync(item);
        }

        public async Task UpdateAsync(ExamPracticeAnswer item)
        {
            await _repository.UpdateAsync(item);
        }
    }
}
