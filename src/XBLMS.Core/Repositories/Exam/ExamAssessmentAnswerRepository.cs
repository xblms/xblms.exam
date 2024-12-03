using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public partial class ExamAssessmentAnswerRepository : IExamAssessmentAnswerRepository
    {
        private readonly Repository<ExamAssessmentAnswer> _repository;
        private readonly string _cacheKey;

        public ExamAssessmentAnswerRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamAssessmentAnswer>(settingsManager.Database, settingsManager.Redis);
            _cacheKey = CacheUtils.GetEntityKey(TableName);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;


        public async Task<int> InsertAsync(ExamAssessmentAnswer item)
        {
            return await _repository.InsertAsync(item);
        }
        public async Task<bool> UpdateAsync(ExamAssessmentAnswer item)
        {
            return await _repository.UpdateAsync(item);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task ClearByUserAsync(int userId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamAssessmentAnswer.UserId), userId));
        }
        public async Task ClearByPaperAsync(int assId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamAssessmentAnswer.ExamAssId), assId));
        }

        public async Task ClearByPaperAndUserAsync(int assId, int userId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamAssessmentAnswer.ExamAssId), assId).Where(nameof(ExamAssessmentAnswer.UserId), userId));
        }


        public async Task<List<string>> GetListAnswer(int assId, int tmId)
        {
            return await _repository.GetAllAsync<string>(Q.
                 Select(nameof(ExamAssessmentAnswer.Answer)).
                 Where(nameof(ExamAssessmentAnswer.TmId), tmId).
                 Where(nameof(ExamAssessmentAnswer.ExamAssId), assId).
                 OrderBy(nameof(ExamAssessmentAnswer.Id)));
        }
        public async Task<int> GetCountSubmitUser(int assId, int tmId,string answer)
        {
            answer = $"%{answer}%";

            return await _repository.CountAsync(Q.
                 Select(nameof(ExamAssessmentAnswer.Id)).
                 Where(nameof(ExamAssessmentAnswer.TmId), tmId).
                 WhereLike(nameof(ExamAssessmentAnswer.Answer), answer).
                 Where(nameof(ExamAssessmentAnswer.ExamAssId), assId));
        }
    }
}
