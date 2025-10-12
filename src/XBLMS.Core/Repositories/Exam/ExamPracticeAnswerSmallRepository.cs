using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class ExamPracticeAnswerSmallRepository : IExamPracticeAnswerSmallRepository
    {
        private readonly Repository<ExamPracticeAnswerSmall> _repository;

        public ExamPracticeAnswerSmallRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamPracticeAnswerSmall>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<int> InsertAsync(ExamPracticeAnswerSmall item)
        {
            return await _repository.InsertAsync(item);
        }
        public async Task<bool> UpdateAsync(ExamPracticeAnswerSmall item)
        {
            return await _repository.UpdateAsync(item);
        }
        public async Task<ExamPracticeAnswerSmall> GetAsync(int tmId, int practiceId)
        {
            return await _repository.GetAsync(Q.
                Where(nameof(ExamPracticeAnswerSmall.TmId), tmId).
                Where(nameof(ExamPracticeAnswerSmall.PracticeId), practiceId));
        }
        public async Task<ExamPracticeAnswerSmall> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<List<ExamPracticeAnswerSmall>> GetListAsync(int answerId, int practiceId)
        {
            return await _repository.GetAllAsync(Q.
                Where(nameof(ExamPracticeAnswerSmall.PracticeId), practiceId).
                Where(nameof(ExamPracticeAnswerSmall.AnswerId), answerId));
        }
    }
}
