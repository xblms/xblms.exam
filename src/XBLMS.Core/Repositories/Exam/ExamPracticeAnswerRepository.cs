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

        public async Task<int> InsertAsync(ExamPracticeAnswer item)
        {
            return await _repository.InsertAsync(item);
        }
        public async Task DeleteByTmIdAsync(int tmId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamPracticeAnswer.TmId), tmId));
        }
        public async Task DeleteByUserId(int userId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamPracticeAnswer.UserId), userId));
        }
        public async Task DeleteByPracticeIdAsync(int practiceId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamPracticeAnswer.PracticeId), practiceId));
        }
        public async Task<(int rightCount, int wrongCount)> CountAsync(int tmId)
        {
            var rightCount = await _repository.CountAsync(Q.
                Where(nameof(ExamPracticeAnswer.TmId), tmId).
                WhereTrue(nameof(ExamPracticeAnswer.IsRight)));

            var wrongCount = await _repository.CountAsync(Q.
                Where(nameof(ExamPracticeAnswer.TmId), tmId).
                WhereNullOrFalse(nameof(ExamPracticeAnswer.IsRight)));

            return (rightCount, wrongCount);
        }

        public async Task<List<ExamPracticeAnswer>> GetListAsync(int practiceId)
        {
            var list= await _repository.GetAllAsync(Q.Where(nameof(ExamPracticeAnswer.PracticeId), practiceId).OrderBy(nameof(ExamPracticeAnswer.Id)));
            return list;
        }
    }
}
